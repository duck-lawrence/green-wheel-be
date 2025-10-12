
using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.Invoice.Response;
using Application.Dtos.RentalContract.Request;
using Application.Dtos.RentalContract.Respone;
using Application.Helpers;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Application
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractUow _uow;
        private readonly IMapper _mapper;
        private readonly EmailSettings _emailSettings;
        public RentalContractService(IRentalContractUow uow, IMapper mapper, IOptions<EmailSettings> emailSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSettings = emailSettings.Value;
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

            ////Update vehicle
            //if(vehicle.Status == (int)VehicleStatus.Available)
            //{
            //    vehicle.Status = (int)VehicleStatus.Unavaible;
            //    await _uow.VehicleRepository.UpdateAsync(vehicle);
            //}

            Guid handoverInvoiceId = Guid.NewGuid();    
            Guid reservationInvoiceId = Guid.NewGuid();

            var handoverInvoice = new Invoice()
            {
                Id = handoverInvoiceId,
                ContractId = contractId,
                Status = (int)InvoiceStatus.Pending,
                Tax = Common.Tax.BaseRentalVAT, //10% dạng decimal
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
            handoverInvoice.Subtotal = InvoiceHelper.CalculateTotalAmount([baseRentalItem]);


            await _uow.SaveChangesAsync();
            //lấy vehicle có join bảng model để in ra view res
            // vehicle = await _uow.VehicleRepository.GetByIdOptionAsync(vehicle.Id, true);
            // contract.Vehicle = vehicle;
            // var rentalContractViewRespone = _mapper.Map<RentalContractViewRes>(contract);
            // return rentalContractViewRespone;
        }

        public async Task<IEnumerable<RentalContractViewRes>> GetByCustomerPhoneAndContractStatus(int? status = null, string? phone = null)
        {
            var contracts = await _uow.RentalContractRepository.GetAllAsync(status, phone);
            if (contracts == null)
            {
            throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);

            }
            var contractViewRes = _mapper.Map<IEnumerable<RentalContractViewRes>>(contracts);
            return contractViewRes;

        }

        public async Task<RentalContractViewRes?> GetContractByUserId(ClaimsPrincipal userClaims)
        {
            var userId = userClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var contracts = await _uow.RentalContractRepository.GetByCustomerAsync(Guid.Parse(userId));
            return _mapper.Map<RentalContractViewRes>(contracts);
        }

        public async Task HandoverRentalContractAsync(ClaimsPrincipal staffClaims, Guid id, HandoverContractReq req)
        {
            var staffId = staffClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var contract = await _uow.RentalContractRepository.GetByIdAsync(id);
            if (contract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            var vehicle = await _uow.VehicleRepository.GetByIdAsync((Guid)contract.VehicleId);
            if (vehicle == null)
            {
                throw new NotFoundException(Message.VehicleMessage.VehicleNotFound);
            }
            var invoice = (await _uow.InvoiceRepository.GetByContractAsync(id)).Where(i => i.Type == (int)InvoiceType.Handover).FirstOrDefault();
            if(invoice == null)
            {
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
            }
            
            if(contract.Status == (int)RentalContractStatus.Active && invoice.Status == (int)InvoiceStatus.Paid)
            {
                vehicle.Status = (int)VehicleStatus.Rented;
                await _uow.VehicleRepository.UpdateAsync(vehicle);
                //lụm xe đi chơi đi bạn
            }
            else
            {
                invoice.PaidAmount = req.Amount;
                invoice.PaidAt = DateTimeOffset.UtcNow;
                invoice.Status = (int)InvoiceStatus.Paid;
                vehicle.Status = (int)VehicleStatus.Rented;
                await _uow.VehicleRepository.UpdateAsync(vehicle);
                await _uow.InvoiceRepository.UpdateAsync(invoice);
            }
            contract.IsSignedByStaff = req.IsSignedByStaff;
            contract.IsSignedByCustomer = req.IsSignedByCustomer;
            contract.ActualStartDate = DateTimeOffset.UtcNow;
            contract.HandoverStaffId = Guid.Parse(staffId);
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.SaveChangesAsync();
        }

        public async Task<InvoiceViewRes?> ReturnRentalContractAsync(ClaimsPrincipal staffClaims, Guid contractId)
        {
            var staffId = staffClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var contract = await _uow.RentalContractRepository.GetByIdAsync(contractId);
            contract.Status = (int)RentalContractStatus.Completed;
            await _uow.RentalContractRepository.UpdateAsync(contract);
            var actual_end_date = contract.EndDate.AddHours(2);
            if (contract == null) throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            var hours = (actual_end_date - contract.EndDate).TotalHours;
            if (hours <= Common.Policy.MaxLateHours)
            {
                await _uow.SaveChangesAsync();
                return null;
            }
            //Create invoice
            Guid invoiceId;
            do
            {
                invoiceId = Guid.NewGuid();

            } while (await _uow.InvoiceRepository.GetByIdOptionAsync(invoiceId) != null);
            var invoice = new Invoice()
            {
                Id = invoiceId,
                ContractId = contractId,
                Status = (int)InvoiceStatus.Pending,
                Tax = Common.Tax.NoneVAT, //10% dạng decimal
                Notes = $"GreenWheel – Invoice for your order {contractId}"
            };
            await _uow.InvoiceRepository.AddAsync(invoice);
            Guid invoiceItemId;
            do
            {
                invoiceItemId = Guid.NewGuid();

            } while ((await _uow.InvoiceItemRepository.GetByIdAsync(invoiceItemId)) != null);
            var items = new InvoiceItem()
            {
                Id = invoiceItemId,
                InvoiceId = invoiceId,
                Quantity = (int)hours,
                UnitPrice = Common.Fee.LateReturn,
                Type = (int)InvoiceItemType.LateReturn,
            };
            invoice.Type = (int)InvoiceType.Return;
            invoice.Subtotal = InvoiceHelper.CalculateTotalAmount([items]);
            await _uow.InvoiceItemRepository.AddRangeAsync([items]);
            await _uow.SaveChangesAsync();
            return _mapper.Map<InvoiceViewRes>(invoice);
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

                body = body.Replace("{CustomerName}", customer.LastName + " " + customer.FirstName)
                           .Replace("{BookingId}", rentalContract.Id.ToString())
                           .Replace("{VehicleModelName}", vehicleModel.Name)
                           .Replace("{LisencePlate}", vehicle.LicensePlate)
                           .Replace("{StationName}", station.Name)
                           .Replace("{StartDate}", rentalContract.StartDate.ToString("dd/MM/yyyy"))
                           .Replace("{EndDate}", rentalContract.EndDate.ToString("dd/MM/yyyy"))
                           .Replace("{AmountDue}", (invoice.First().Tax * invoice.First().Subtotal).ToString("N0") + "VNĐ")
                           .Replace("{PaymentLink}", "https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=RDdQw4w9WgXcQ&start_radio=1")
                           .Replace("{Deadline}", "....");
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
             await EmailHelper.SendEmailAsync(_emailSettings, customer.Email, subject, body);
            await _uow.SaveChangesAsync();
        }
    }
}
