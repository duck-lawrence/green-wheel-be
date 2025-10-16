using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.RentalContract.Request;
using Application.Dtos.RentalContract.Respone;
using Application.Helpers;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
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
        public RentalContractService(IRentalContractUow uow, IMapper mapper, IOptions<EmailSettings> emailSettings, IEmailSerivce emailService)
        {
            _uow = uow;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<RentalContractViewRes> GetByIdAsync(Guid id)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id);
            if(contract == null) throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
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
            //ktra xem có cccd hay chưa
            //var citizenIdentity = await _uow.CitizenIdentityRepository.GetByUserIdAsync(userID);
            //if(citizenIdentity == null)
            //{
            //    throw new ForbidenException(Message.User.NotHaveCitizenIdentity) ;
            //}
            //var driverLisence = await _uow.DriverLicenseRepository.GetByUserId(userID);
            //if (driverLisence == null)
            //{
            //    throw new ForbidenException(Message.User.NotHaveDriverLicense);
            //}
            //---------

            //ktra có đơn đặt xe chưa
            if (await _uow.RentalContractRepository.HasActiveContractAsync(userID))
            {
                throw new BusinessException(Message.RentalContractMessage.UserAlreadyHaveContract);
            }
            var station = await _uow.StationRepository.GetByIdAsync(createReq.StationId) ??
                                                        throw new NotFoundException(Message.StationMessage.StationNotFound);
            var vehicles = await _uow.VehicleRepository.GetVehicles(createReq.StationId,
                                                        createReq.ModelId) ??
                                                        throw new NotFoundException(Message.VehicleMessage.VehicleNotFound);
            Vehicle vehicle = null;
            foreach(var _vehicle in vehicles)
            {
                if(_vehicle.Status == (int)VehicleStatus.Available)
                {
                    vehicle = _vehicle;
                    break;
                }
                if(_vehicle.Status == (int)VehicleStatus.Unavaible || _vehicle.Status == (int)VehicleStatus.Rented)
                {
                    var isOverlap = false;
                    foreach(var rentalContract in _vehicle.RentalContracts)
                    {
                        if(rentalContract.StartDate <= createReq.EndDate.AddDays(10)
                            && rentalContract.EndDate >= createReq.StartDate.AddDays(-10))
                        {
                            isOverlap = true;
                            break;
                        } 
                    }
                    if (isOverlap == false)
                    {
                        vehicle = _vehicle;
                        break;
                    }
                }
            }
            if (vehicle == null) throw new NotFoundException(Message.VehicleMessage.VehicleNotFound);
            var model = await _uow.VehicleModelRepository.GetByIdAsync(createReq.ModelId);

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
                $"\r\nThe vehicle will be reserved at {station.Name} from {createReq.StartDate} to {createReq.EndDate}.",
                Notes = $"Customer rented the vehicle for {days} days.",
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
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                DeletedAt = null
            };
            
            await _uow.DepositRepository.AddAsync(deposit);
            handoverInvoice.Subtotal = InvoiceHelper.CalculateSubTotalAmount([baseRentalItem]);
            reservationInvoice.Subtotal = InvoiceHelper.CalculateSubTotalAmount([reservationRentalItem]);


            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<RentalContractViewRes>> GetAll(GetAllRentalContactReq req)
        {
            var contracts = await _uow.RentalContractRepository.GetAllAsync(req.Status, req.Phone,
                req.CitizenIdentityNumber, req.DriverLicenseNumber, req.checkListId);
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
            var staffId = staffClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            if (contract.ActualStartDate != null) throw new BusinessException(Message.RentalContractMessage.ContractAlreadyComplete);
            var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)contract.VehicleId) 
                ?? throw new NotFoundException(Message.VehicleMessage.VehicleNotFound);

            var handoverInvoice = (await _uow.InvoiceRepository.GetByContractAsync(id))
                .Where(i => i.Type == (int)InvoiceType.Handover).FirstOrDefault()
                    ?? throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
            
            
            if(contract.Status == (int)RentalContractStatus.Active && handoverInvoice.Status == (int)InvoiceStatus.Paid)
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
        }

        public async Task<Guid> ReturnProcessRentalContractAsync(ClaimsPrincipal staffClaims, Guid contractId)
        {
            var staffId = staffClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var contract = await _uow.RentalContractRepository.GetByIdAsync(contractId) 
               ?? throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            if (contract.Status == (int)RentalContractStatus.Completed) throw new BusinessException(Message.RentalContractMessage.ContractAlreadyComplete);
            contract.Status = (int)RentalContractStatus.Completed;
            var actual_end_date = contract.EndDate.AddHours(2); // test
            //var actual_end_date = DateTimeOffset.UtcNow;
            if (contract == null) throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            var hours = (actual_end_date - contract.EndDate).TotalHours; //tính thời gian trể
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

            //tạo hoá đơn refund
            Guid refundInvoiceId = Guid.NewGuid();
            var refundInvoice = new Invoice()
            {
                Id = returnInvoiceId,
                ContractId = contractId,
                Status = (int)InvoiceStatus.Pending,
                Tax = Common.Tax.NoneVAT, //10% dạng decimal
                Notes = $"GreenWheel – Invoice for your order {contractId}",
                Type = (int)InvoiceType.Refund
            };
            invoices = invoices.Append(refundInvoice);
            var deposit = await _uow.DepositRepository.GetByContractIdAsync(contractId);
            refundInvoice.Subtotal = deposit.Amount;
            returnInvoice.Subtotal = InvoiceHelper.CalculateSubTotalAmount(returnInvoiceItems);
            await _uow.InvoiceRepository.AddRangeAsync(invoices);
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.InvoiceItemRepository.AddRangeAsync(returnInvoiceItems);
            await _uow.SaveChangesAsync();
            return returnInvoice.Id;
        }

        public async Task CancelRentalContract(Guid id)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id);
            if (contract == null) throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);

            if (contract.Status != (int)RentalContractStatus.PaymentPending || contract.Status != (int)RentalContractStatus.RequestPeding)
            {
                throw new BadRequestException(Message.RentalContractMessage.CanNotCancel);
            }
            if (contract.Status == (int)RentalContractStatus.Completed) throw new BusinessException(Message.RentalContractMessage.ContractAlreadyComplete);
            contract.Status = (int)RentalContractStatus.Cancelled;
            await _uow.RentalContractRepository.UpdateAsync(contract);
        }
        public async Task UpdateStatusAsync(Guid id)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id);
            if(contract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            var invoices = contract.Invoices.Where( i => i.Type == (int)InvoiceType.Reservation || i.Type == (int)InvoiceType.Handover);
            if(invoices.Any(i => i.Status == (int)InvoiceStatus.Paid && contract.Status == (int)RentalContractStatus.PaymentPending)){
                contract.Status = (int)RentalContractStatus.Active;
                var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)contract.VehicleId!);
                if(vehicle.Status == (int)VehicleStatus.Available)
                {
                    vehicle.Status = (int)VehicleStatus.Unavaible;
                    await _uow.VehicleRepository.UpdateAsync(vehicle);
                }
                await _uow.RentalContractRepository.UpdateAsync(contract);
                await _uow.SaveChangesAsync();
            }
        }

        
        public async Task VerifyRentalContract(Guid id, bool hasVehicle = true, int? vehicleStatus = null)
        {
            var rentalContract = await _uow.RentalContractRepository.GetByIdAsync(id);
            if (rentalContract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            //Lấy customer
            var customer = (await _uow.RentalContractRepository.GetAllAsync(new Expression<Func<RentalContract, object>>[]
            {
                rc => rc.Customer
            })).Where(rc => rc.Id == id)
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
                templatePath = Path.Combine(basePath, "Templates", "CancelRentalContractEmailTempate.html");
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

        
    }
}
