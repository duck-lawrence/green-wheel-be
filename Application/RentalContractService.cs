using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.RentalContract.Request;
using Application.Dtos.RentalContract.Respone;
using Application.Helpers;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractUow _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSerivce _emailService;
        private readonly IVehicleCheckListRepository _vehicleCheckListRepository;
        public RentalContractService(IRentalContractUow uow, IMapper mapper,
            IOptions<EmailSettings> emailSettings, IEmailSerivce emailService,
            IVehicleCheckListRepository vehicleCheckListRepository)
        {
            _uow = uow;
            _mapper = mapper;
            _emailService = emailService;
            _vehicleCheckListRepository = vehicleCheckListRepository;
        }

        public async Task<RentalContractViewRes> GetByIdAsync(Guid id)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id);
            if(contract == null) throw new NotFoundException(Message.RentalContractMessage.NotFound);
            var reservationInvoice = (await _uow.InvoiceRepository.GetByContractAsync(id))
                            .Where(i => i.Type == (int)InvoiceType.Reservation).FirstOrDefault();
            var reservationFee = 0;
            if(reservationInvoice != null && reservationInvoice.Status == (int)InvoiceStatus.Paid)
            {
                reservationFee = (int)reservationInvoice.Subtotal;
            }
            return _mapper.Map<RentalContractViewRes>(contract, otp => otp.Items["ReservationFee"] = reservationFee);
        }
        public async Task CreateRentalContractAsync(Guid userID, CreateRentalContractReq createReq)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                //ktra xem có cccd hay chưa
                //var citizenIdentity = await _uow.CitizenIdentityRepository.GetByUserIdAsync(userID);
                //if (citizenIdentity == null)
                //{
                //    throw new ForbidenException(Message.UserMessage.CitizenIdentityNotFound);
                //}
                //var driverLisence = await _uow.DriverLicenseRepository.GetByUserIdAsync(userID);
                //if (driverLisence == null)
                //{
                //    throw new ForbidenException(Message.UserMessage.DriverLicenseNotFound);
                //}
                //---------
                //ktra có đơn đặt xe chưa
                if (await _uow.RentalContractRepository.HasActiveContractAsync(userID))
                {
                    throw new BusinessException(Message.RentalContractMessage.UserAlreadyHaveContract);
                }
                var station = await _uow.StationRepository.GetByIdAsync(createReq.StationId) ??
                                                            throw new NotFoundException(Message.StationMessage.NotFound);
                var model = (await _uow.VehicleModelRepository.GetByIdAsync(createReq.ModelId
                                        , createReq.StationId, createReq.StartDate, createReq.EndDate));

                if (model!.Vehicles == null || model.Vehicles.Count == 0) throw new NotFoundException(Message.VehicleMessage.NotFound);
                var vehicle = model.Vehicles.FirstOrDefault();
                var days = (int)Math.Ceiling((createReq.EndDate - createReq.StartDate).TotalDays);
                Guid contractId;
                do
                {
                    contractId = Guid.NewGuid();

                } while (await _uow.RentalContractRepository.GetByIdAsync(contractId) != null);
                var contract = new RentalContract()
                {
                    Id = contractId,
                    Description = $"This contract was created by the customer through the online booking system." +
                    $"\r\nThe vehicle will be reserved at {station.Name} from {createReq.StartDate} to {createReq.EndDate}. Customer rented the vehicle for {days} days.",
                    Notes = createReq.Notes,
                    StartDate = createReq.StartDate,
                    ActualStartDate = null,
                    EndDate = createReq.EndDate,
                    ActualEndDate = null,
                    IsSignedByCustomer = false,
                    IsSignedByStaff = false,
                    Status = (int)RentalContractStatus.RequestPeding,
                    VehicleId = vehicle.Id,
                    CustomerId = userID,
                    StationId = station.Id,
                    HandoverStaffId = null,
                    ReturnStaffId = null,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    DeletedAt = null,
                };
                await _uow.RentalContractRepository.AddAsync(contract);

                Guid handoverInvoiceId = Guid.NewGuid();
                Guid reservationInvoiceId = Guid.NewGuid();

                var handoverInvoice = new Invoice()
                {
                    Id = handoverInvoiceId,
                    ContractId = contractId,
                    Status = (int)InvoiceStatus.Pending,
                    Tax = Common.Tax.BaseVAT, //10% dạng decimal
                    Type = (int)InvoiceType.Handover,
                    Notes = $"GreenWheel – Invoice for your order {contractId}"
                };
                var reservationInvoice = new Invoice()
                {
                    Id = reservationInvoiceId,
                    ContractId = contractId,
                    Status = (int)InvoiceStatus.Pending,
                    Tax = Common.Tax.NoneVAT, //10% dạng decimal
                    Type = (int)InvoiceType.Reservation,
                    Notes = $"GreenWheel – Invoice for your order {contractId}"
                };
                await _uow.InvoiceRepository.AddRangeAsync([handoverInvoice, reservationInvoice]);
                var baseRentalItem = new InvoiceItem()
                {
                    InvoiceId = handoverInvoiceId,
                    Quantity = days,
                    UnitPrice = model.CostPerDay,
                    Type = (int)InvoiceItemType.BaseRental,
                };
                var reservationRentalItem = new InvoiceItem()
                {
                    InvoiceId = reservationInvoiceId,
                    Quantity = 1,
                    UnitPrice = model.ReservationFee,
                    Type = (int)InvoiceItemType.Other,
                };

                await _uow.InvoiceItemRepository.AddRangeAsync([baseRentalItem, reservationRentalItem]);
                var deposit = new Deposit
                {
                    InvoiceId = handoverInvoiceId,
                    Amount = model.DepositFee,
                    Status = (int)DepositStatus.Pending,
                };

                await _uow.DepositRepository.AddAsync(deposit);
                handoverInvoice.Subtotal = InvoiceHelper.CalculateSubTotalAmount([baseRentalItem]);
                reservationInvoice.Subtotal = InvoiceHelper.CalculateSubTotalAmount([reservationRentalItem]);


                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
                await _uow.BeginTransactionAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<RentalContractViewRes>> GetAll(GetAllRentalContactReq req)
        {
            var contracts = await _uow.RentalContractRepository.GetAllAsync(req.Status, req.Phone,
                req.CitizenIdentityNumber, req.DriverLicenseNumber, req.StationId);
            return _mapper.Map<IEnumerable<RentalContractViewRes>>(contracts) ?? []; 
        }

        public async Task<IEnumerable<RentalContractViewRes>> GetMyContracts(ClaimsPrincipal userClaims, int? status)
        {
            var userId = userClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var contracts = await _uow.RentalContractRepository.GetByCustomerAsync(Guid.Parse(userId), status);
            return _mapper.Map<IEnumerable<RentalContractViewRes>>(contracts) ?? [];
        }

        public async Task HandoverProcessRentalContractAsync(ClaimsPrincipal staffClaims, Guid id, HandoverContractReq req)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var staffId = staffClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
                var contract = await _uow.RentalContractRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException(Message.RentalContractMessage.NotFound);
                if (contract.ActualStartDate != null) throw new BusinessException(Message.RentalContractMessage.ContractAlreadyProcess);
                var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)contract.VehicleId)
                    ?? throw new NotFoundException(Message.VehicleMessage.NotFound);

                var handoverInvoice = (await _uow.InvoiceRepository.GetByContractAsync(id))
                    .Where(i => i.Type == (int)InvoiceType.Handover).FirstOrDefault()
                        ?? throw new NotFoundException(Message.InvoiceMessage.NotFound);

                if (contract.VehicleChecklists == null ||
                    !contract.VehicleChecklists.Any(c => c.Type == (int)VehicleChecklistType.Handover))
                {
                    throw new NotFoundException(Message.VehicleChecklistMessage.NotFound);
                }
                if (contract.Status == (int)RentalContractStatus.Active && handoverInvoice.Status == (int)InvoiceStatus.Paid)
                {
                    vehicle.Status = (int)VehicleStatus.Rented;
                    await _uow.VehicleRepository.UpdateAsync(vehicle);
                    //lụm xe đi chơi đi bạn
                }
                else
                {
                    throw new BusinessException(Message.InvoiceMessage.NotHandoverPayment);
                }
                contract.IsSignedByStaff = req.IsSignedByStaff;
                contract.IsSignedByCustomer = req.IsSignedByCustomer;
                contract.ActualStartDate = DateTimeOffset.UtcNow;
                contract.HandoverStaffId = Guid.Parse(staffId);
                await _uow.RentalContractRepository.UpdateAsync(contract);
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<Guid> ReturnProcessRentalContractAsync(ClaimsPrincipal staffClaims, Guid contractId)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var staffId = staffClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
                var contract = await _uow.RentalContractRepository.GetByIdAsync(contractId)
                   ?? throw new NotFoundException(Message.RentalContractMessage.NotFound);
                if (contract.Status == (int)RentalContractStatus.Returned) throw new BusinessException(Message.RentalContractMessage.ContractAlreadyProcess);
                contract.Status = (int)RentalContractStatus.Returned;
                contract.ReturnStaffId = Guid.Parse(staffId);
                var actualEndDate = contract.EndDate.AddHours(2); // test
                                                                  //var actual_end_date = DateTimeOffset.UtcNow;
                if (contract == null) throw new NotFoundException(Message.RentalContractMessage.NotFound);
                contract.ActualEndDate = actualEndDate;
                var hours = (actualEndDate - contract.EndDate).TotalHours; //tính thời gian trể
                hours = Double.Ceiling(hours);
                IEnumerable<Invoice> invoices = [];
                Guid returnInvoiceId = Guid.NewGuid();
                var returnInvoice = new Invoice()
                {
                    Id = returnInvoiceId,
                    ContractId = contractId,
                    Status = (int)InvoiceStatus.Pending,
                    Tax = Common.Tax.BaseVAT, //10% dạng decimal
                    Notes = $"GreenWheel – Invoice for your order {contractId}",
                    Type = (int)InvoiceType.Return
                };
                invoices = invoices.Append(returnInvoice);

                IEnumerable<InvoiceItem> returnInvoiceItems = []; //tạo trước invoice item

                if (hours > Common.Policy.MaxLateHours)
                {
                    //phí trể giờ
                    returnInvoiceItems = returnInvoiceItems.Append(new InvoiceItem()
                    {
                        InvoiceId = returnInvoiceId,
                        Quantity = (int)hours,
                        UnitPrice = Common.Fee.LateReturn,
                        Type = (int)InvoiceItemType.LateReturn,
                    });
                }
                //phí dọn dẹp
                returnInvoiceItems = returnInvoiceItems.Append(new InvoiceItem()
                {
                    InvoiceId = returnInvoiceId,
                    Quantity = 1,
                    UnitPrice = Common.Fee.Cleaning,
                    Type = (int)InvoiceItemType.Cleaning,
                });
                returnInvoice.Subtotal = InvoiceHelper.CalculateSubTotalAmount(returnInvoiceItems);
                var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)contract.VehicleId!);
                vehicle!.Status = (int)VehicleStatus.Maintenance;
                await _uow.VehicleRepository.UpdateAsync(vehicle);
                await _uow.InvoiceRepository.AddRangeAsync(invoices);
                await _uow.RentalContractRepository.UpdateAsync(contract);
                await _uow.InvoiceItemRepository.AddRangeAsync(returnInvoiceItems);
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
                return returnInvoice.Id;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw;
            } 
        }

        public async Task CancelRentalContract(Guid id)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.RentalContractMessage.NotFound);

            if (contract.Status != (int)RentalContractStatus.PaymentPending && contract.Status != (int)RentalContractStatus.RequestPeding)
            {
                throw new BadRequestException(Message.RentalContractMessage.CanNotCancel);
            }
            contract.Status = (int)RentalContractStatus.Cancelled;
            await _uow.RentalContractRepository.UpdateAsync(contract);
        }
        public async Task UpdateStatusAsync(Guid id)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var contract = await _uow.RentalContractRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.RentalContractMessage.NotFound);

                if (contract.Status == (int)RentalContractStatus.PaymentPending)
                {
                    var invoices = contract.Invoices.Where(i => i.Type == (int)InvoiceType.Reservation || i.Type == (int)InvoiceType.Handover);
                    if (invoices != null && invoices.Any(i => i.Status == (int)InvoiceStatus.Paid && contract.Status == (int)RentalContractStatus.PaymentPending))
                    {
                        contract.Status = (int)RentalContractStatus.Active;
                        var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)contract.VehicleId!)
                            ?? throw new NotFoundException(Message.VehicleMessage.NotFound);
                        if (vehicle.Status == (int)VehicleStatus.Available)
                        {
                            vehicle.Status = (int)VehicleStatus.Unavaible;
                            await _uow.VehicleRepository.UpdateAsync(vehicle);
                        }
                        var anotherContract = (await _uow.RentalContractRepository.GetByVehicleIdAsync(vehicle.Id))
                                                .Where(c => c.Id != contract.Id
                                                    && (c.Status == (int)RentalContractStatus.PaymentPending
                                                        || c.Status == (int)RentalContractStatus.RequestPeding)
                                                );
                        if (anotherContract != null && anotherContract.Any())
                        {
                            var startBuffer = contract.StartDate.AddDays(-10);
                            var endBuffer = contract.EndDate.AddDays(10);
                            foreach (var contract_ in anotherContract)
                            {
                                if (startBuffer <= contract_.EndDate &&
                                endBuffer >= contract_.StartDate)
                                {
                                    contract_.Status = (int)RentalContractStatus.Cancelled;
                                    contract_.Description += ". Booking was canceled as another customer successfully paid for the same vehicle earlier.";
                                    var subject = "[GreenWheel] Your Booking Has Been Canceled";
                                    var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "CancelAutoEmailTemplate.html");
                                    var body = System.IO.File.ReadAllText(templatePath);
                                    var customer = contract_.Customer;
                                    var station = contract_.Station;
                                    var vehicleToCancel = contract_.Vehicle
                                        ?? throw new NotFoundException(Message.VehicleMessage.NotFound);
                                    var model = vehicleToCancel.Model;

                                    var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")
                                        ?? "http://localhost:3000/";
                                    var contractDetailUrl = $"{frontendOrigin}/vehicle-models";

                                    body = body.Replace("{CustomerName}", $"{customer.LastName} {customer.FirstName}")
                                               .Replace("{ContractCode}", contract_.Id.ToString())
                                               .Replace("{VehicleName}", model.Name)
                                               .Replace("{LisencePlate}", vehicleToCancel.LicensePlate)
                                               .Replace("{StationName}", station.Name)
                                               .Replace("{StartDate}", contract_.StartDate.ToString("dd/MM/yyyy"))
                                               .Replace("{EndDate}", contract_.EndDate.ToString("dd/MM/yyyy"))
                                               .Replace("{BookingLink}", contractDetailUrl);
                                    await _emailService.SendEmailAsync(customer.Email, subject, body);
                                    await _uow.RentalContractRepository.UpdateAsync(contract_);
                                }
                            }
                        }
                    }
                }
                else if (contract.Status == (int)RentalContractStatus.Returned)
                {
                    var invoices = contract.Invoices.Where(i => i.Type == (int)InvoiceType.Refund);
                    if (invoices != null && invoices.Any(i => i.Status == (int)InvoiceStatus.Paid))
                    {
                        contract.Status = (int)RentalContractStatus.Completed;
                    }
                }
                await _uow.RentalContractRepository.UpdateAsync(contract);
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
        
        public async Task VerifyRentalContract(Guid id, bool hasVehicle = true, int? vehicleStatus = null)
        {
            var rentalContract = await _uow.RentalContractRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException(Message.RentalContractMessage.NotFound);
            //Lấy customer
            var customer = (await _uow.RentalContractRepository.GetAllAsync(
                [rc => rc.Customer]))
                .Where(rc => rc.Id == id)
                .Select(rc => rc.Customer).FirstOrDefault();

            var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)rentalContract.VehicleId);
            var vehicleModel = await _uow.VehicleModelRepository.GetByIdAsync(vehicle.ModelId);
            var station = await _uow.StationRepository.GetByIdAsync(vehicle.StationId);
            string subject;
            string templatePath;
            string body;
            var basePath = AppContext.BaseDirectory;

            if (rentalContract.Status != (int)RentalContractStatus.RequestPeding)
            {
                throw new BadRequestException(Message.RentalContractMessage.ThisRentalContractAlreadyProcess);
            }
            if (hasVehicle)
            {
                rentalContract.Status = (int)RentalContractStatus.PaymentPending;
                await _uow.RentalContractRepository.UpdateAsync(rentalContract);
                //Lấy invoice
                var invoice = (await _uow.RentalContractRepository.GetAllAsync(new Expression<Func<RentalContract, object>>[]
                {
                rc => rc.Invoices
                })).Where(rc => rc.Id == id)
                .Select(rc => rc.Invoices).FirstOrDefault();

                subject = "[GreenWheel] Confirm Your Booking by Completing Payment";
                templatePath = Path.Combine(basePath, "Templates", "PaymentEmailTemplate.html");
                body = System.IO.File.ReadAllText(templatePath);

                var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")
                    ?? "http://localhost:3000/";
                var contractDetailUrl = $"{frontendOrigin}/rental-contracts/{rentalContract.Id}";

                body = body.Replace("{CustomerName}", customer.LastName + " " + customer.FirstName)
                           .Replace("{BookingId}", rentalContract.Id.ToString())
                           .Replace("{VehicleModelName}", vehicleModel.Name)
                           .Replace("{LisencePlate}", vehicle.LicensePlate)
                           .Replace("{StationName}", station.Name)
                           .Replace("{StartDate}", rentalContract.StartDate.ToString("dd/MM/yyyy"))
                           .Replace("{EndDate}", rentalContract.EndDate.ToString("dd/MM/yyyy"))
                           .Replace("{PaymentLink}", contractDetailUrl);
            }
            else
            {
                if (rentalContract.Status == (int)RentalContractStatus.RequestPeding)
                {
                    rentalContract.Status = (int) RentalContractStatus.Cancelled;
                    await _uow.RentalContractRepository.UpdateAsync(rentalContract);
                }
                subject = "[GreenWheel] Vehicle Unavailable, Booking Cancelled";
                templatePath = Path.Combine(basePath, "Templates", "RejectRentalContractEmailTempate.html");
                body = System.IO.File.ReadAllText(templatePath);
                if(vehicleStatus != null)
                {
                    vehicle.Status = (int)vehicleStatus;
                    await _uow.VehicleRepository.UpdateAsync(vehicle);
                }
                body = body.Replace("{CustomerName}", customer.LastName + " " + customer.FirstName)
                           .Replace("{VehicleModelName}", vehicleModel.Name)
                           .Replace("{StationName}", station.Name)
                           .Replace("{StartDate}", rentalContract.StartDate.ToString("dd/MM/yyyy"))
                           .Replace("{EndDate}", rentalContract.EndDate.ToString("dd/MM/yyyy"));
            }
            await _emailService.SendEmailAsync(customer.Email, subject, body);
            await _uow.SaveChangesAsync();
        }

        public async Task ChangeVehicleAsync(Guid id)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id);
            var returnChecklist = contract.VehicleChecklists
                .FirstOrDefault(c => c.Type == (int)(VehicleChecklistType.Return));

            if(returnChecklist!.MaintainedUntil != null)
            {
                 //lấy những hợp đồng có cùng xe với hợp đồng này mà có trạng thái là đang active
                 var otherContracts = (await _uow.RentalContractRepository.GetByVehicleIdAsync(id))
                                    .Where(c => c.Status == (int)RentalContractStatus.RequestPeding
                                    ||
                                    c.Status == (int)RentalContractStatus.PaymentPending
                                    ||
                                    c.Status == (int)RentalContractStatus.Active);
                //nếu có hợp đồng cùng xe thì tục
                if(otherContracts != null)
                {
                    IEnumerable<RentalContract> flagContract = [];
                    foreach(var contract_ in  otherContracts)
                    {
                        if(contract.StartDate <= returnChecklist.MaintainedUntil)
                        {
                            flagContract = flagContract.Append(contract_);
                        }
                    }
                    if(flagContract.Any())
                    {
                        await _uow.BeginTransactionAsync();
                        try
                        {
                            foreach (var contract_ in flagContract)
                            {
                                if (contract_.Status == (int)RentalContractStatus.RequestPeding
                                    || contract_.Status == (int)RentalContractStatus.PaymentPending)
                                {
                                    await CancelContractAndSendEmail(contract_,
                                                            ". Booking was canceled because vehicle was maintained");
                                }
                                else if (contract_.Status == (int)RentalContractStatus.Active)
                                {
                                    contract.Status = (int)RentalContractStatus.UnavailableVehicle;
                                    var model = (await _uow.VehicleModelRepository.GetByIdAsync(contract_.Vehicle!.ModelId
                                            , contract_.StationId, contract_.StartDate, contract_.EndDate));

                                    if (model!.Vehicles == null || model.Vehicles.Count == 0)
                                        throw new NotFoundException(Message.VehicleMessage.NotFound);
                                    var vehicle = model.Vehicles.FirstOrDefault();
                                    if (vehicle != null)
                                    {
                                        contract.VehicleId = vehicle.Id;
                                    }
                                    contract.VehicleId = null;
                                    var subject = "[GreenWheel] Issue Detected in Your GreenWheel Rental Contract";
                                    var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "VehicleIssueNotification.html");
                                    var body = System.IO.File.ReadAllText(templatePath);
                                    var customer = contract_.Customer;
                                    if (customer.Email != null)
                                    {
                                        var station = contract_.Station;
                                        var vehicleToCancel = contract_.Vehicle
                                            ?? throw new NotFoundException(Message.VehicleMessage.NotFound);
                                        var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")
                                            ?? "http://localhost:3000/";
                                        var contractDetailUrl = $"{frontendOrigin}";

                                        body = body = body.Replace("{CustomerName}", $"{customer.LastName} {customer.FirstName}")
                                                   .Replace("{ContractCode}", contract_.Id.ToString())
                                                   .Replace("{VehicleName}", model.Name)
                                                   .Replace("{LisencePlate}", vehicleToCancel.LicensePlate)
                                                   .Replace("{StationName}", station.Name)
                                                   .Replace("{StartDate}", contract_.StartDate.ToString("dd/MM/yyyy"))
                                                   .Replace("{EndDate}", contract_.EndDate.ToString("dd/MM/yyyy"))
                                                   .Replace("{ResolveLink}", contractDetailUrl);

                                        await _emailService.SendEmailAsync(customer.Email!, subject, body);
                                    }
                                    await _uow.RentalContractRepository.UpdateAsync(contract_);
                                    var invoiceId = Guid.NewGuid();
                                    var invoice = new Invoice()
                                    {
                                        Id = invoiceId,
                                        ContractId = contract_.Id,
                                        Status = (int)InvoiceStatus.Pending,
                                        Tax = Common.Tax.NoneVAT, //10% dạng decimal
                                        Notes = $"GreenWheel – Invoice for your order {contract_.Id}",
                                        Type = (int)InvoiceType.Refund
                                    };
                                    var item = new InvoiceItem()
                                    {
                                        InvoiceId = invoiceId,
                                        Quantity = 1,
                                        UnitPrice = 0,
                                        Description = "Refund for order {contract_.Id}",
                                        Type = (int)InvoiceItemType.Refund,
                                    };
                                    var handoverInvoice = contract_.Invoices.FirstOrDefault(i => i.Status == (int)InvoiceType.Handover);
                                    var reservation = contract_.Invoices.FirstOrDefault(i => i.Status == (int)InvoiceType.Reservation);
                                    if(handoverInvoice!.Status == (int)InvoiceStatus.Paid && reservation!.Status == (int)InvoiceStatus.Paid)
                                    {
                                        item.UnitPrice = (decimal)handoverInvoice.PaidAmount! + (decimal)reservation.PaidAmount!;
                                    }else if(handoverInvoice!.Status == (int)InvoiceStatus.Paid)
                                    {
                                        item.UnitPrice = (decimal)handoverInvoice.PaidAmount!;
                                    }
                                    else
                                    {
                                        item.UnitPrice = (decimal)reservation!.PaidAmount!;
                                    }
                                    await _uow.InvoiceRepository.AddAsync(invoice);
                                    await _uow.InvoiceItemRepository.AddAsync(item);
                                }
                            }
                            await _uow.SaveChangesAsync();
                            await _uow.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await _uow.RollbackAsync();
                            throw;
                        }
                    }
                } 
            }
        }

        private async Task CancelContractAndSendEmail(RentalContract contract_, string description
                                                   )
        {
            contract_.Status = (int)RentalContractStatus.Cancelled;
            contract_.Description += description;
            var subject = "[GreenWheel] Your Booking Has Been Canceled";
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "CancelAutoEmailTemplate.html");
            var body = System.IO.File.ReadAllText(templatePath);
            var customer = contract_.Customer;
            if(customer.Email != null)
            {
                var station = contract_.Station;
                var vehicleToCancel = contract_.Vehicle
                    ?? throw new NotFoundException(Message.VehicleMessage.NotFound);
                var model = vehicleToCancel.Model;

                var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")
                    ?? "http://localhost:3000/";
                var contractDetailUrl = $"{frontendOrigin}/vehicle-models";

                body = body.Replace("{CustomerName}", $"{customer.LastName} {customer.FirstName}")
                           .Replace("{ContractCode}", contract_.Id.ToString())
                           .Replace("{VehicleName}", model.Name)
                           .Replace("{LisencePlate}", vehicleToCancel.LicensePlate)
                           .Replace("{StationName}", station.Name)
                           .Replace("{StartDate}", contract_.StartDate.ToString("dd/MM/yyyy"))
                           .Replace("{EndDate}", contract_.EndDate.ToString("dd/MM/yyyy"))
                           .Replace("{BookingLink}", contractDetailUrl);
                await _emailService.SendEmailAsync(customer.Email!, subject, body);
            }
            await _uow.RentalContractRepository.UpdateAsync(contract_);
        }

        public async Task ProcessCustomerConfirm(Guid id, int ResolutionOption)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.RentalContractMessage.NotFound);
            if(ResolutionOption == (int)VehicleIssueResolutionOption.ChangeVehicle)
            {
                contract.Status = (int)RentalContractStatus.Active;
            }
            else
            {
                //kiểm tra 3 trường thông tin chuyển khoản xem có chưa, chưa có thì bắt nhập
                //có rồi thì tiếp tục

                //------------
                contract.Status = (int)RentalContractStatus.Cancelled;
                var subject = "[GreenWheel] Your Booking Has Been Canceled";
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "CancelAutoEmailTemplate.html");
                var body = System.IO.File.ReadAllText(templatePath);
                var customer = contract.Customer;
                if (customer.Email != null)
                {
                    var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")
                        ?? "http://localhost:3000/";
                    var contractDetailUrl = $"{frontendOrigin}/vehicle-models";

                    body = body.Replace("{CustomerName}", $"{customer.LastName} {customer.FirstName}")
                           .Replace("{ContractCode}", contract.Id.ToString())
                           .Replace("{SupportLink}", contractDetailUrl);

                    await _emailService.SendEmailAsync(customer.Email!, subject, body);
                }

            }
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.SaveChangesAsync();
        }
    }
}
