
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
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
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
        public async Task<RentalContractViewRes> CreateRentalContractAsync(Guid userID, CreateRentalContractReq createReq)
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
                HandoverStaffId = null,
                ReturnStaffId = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                DeletedAt = null,
            };
            await _uow.RentalContractRepository.AddAsync(contract);

            //Update vehicle
            if(vehicle.Status == (int)VehicleStatus.Available)
            {
                vehicle.Status = (int)VehicleStatus.Unavaible;
                await _uow.VehicleRepository.UpdateAsync(vehicle);
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
                Tax = 0.1m, //10% dạng decimal
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                DeletedAt = null,
                Notes = $"GreenWheel – Invoice for your order {contractId}"
            };
            await _uow.InvoiceRepository.AddAsync(invoice);

            //Create invoice Item
            var InvoiceItemNote = AutoGenerateHelper.GenerateInvoiceItemNotes(InvoiceItemType.BaseRental, days);
            Guid invoiceItemId;
            do
            {
                invoiceItemId = Guid.NewGuid();

            } while ((await _uow.InvoiceItemRepository.GetByIdAsync(invoiceItemId)) != null);
            var items = new InvoiceItem()
            {
                Id = invoiceItemId,
                InvoiceId = invoiceId,
                Quantity = days,
                UnitPrice = model.CostPerDay,
                Type = (int)InvoiceItemType.BaseRental,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                DeletedAt = null,
                Notes = InvoiceItemNote

            };
            await _uow.InvoiceItemRepository.AddRangeAsync([items]);

            //Deposit
            Guid depositId;
            do
            {
                depositId = Guid.NewGuid();

            } while (await _uow.DepositRepository.GetByIdAsync(depositId) != null);
            var deposit = new Deposit
            {
                Id = depositId,
                InvoiceId = invoiceId,
                Amount = model.DepositFee,
                Status = (int)DepositStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                DeletedAt = null,
                Description = $"Refundable deposit for vehicle model {model.Name}"
            };
            
            await _uow.DepositRepository.AddAsync(deposit);
            invoice.Subtotal = items.Quantity * items.UnitPrice + deposit.Amount;


            await _uow.SaveChangesAsync();
            var rentalContractViewRespone = _mapper.Map<RentalContractViewRes>(contract);
            return rentalContractViewRespone;
        }

        public async Task<IEnumerable<RentalContractForStaffViewRes>> GetByStatus(int status)
        {
            var RcList = await _uow.RentalContractRepository.GetByStatus(status);
            var RcForStaffViewRes = _mapper.Map<IEnumerable<RentalContractForStaffViewRes>>(RcList);
            return RcForStaffViewRes;
        }

        public async Task UpdateStatus(RentalContract rentalContract, int status)
        {
            
            rentalContract.Status = status;
            await _uow.RentalContractRepository.UpdateAsync(rentalContract);
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
                await UpdateStatus(rentalContract, (int)RentalContractStatus.PaymentPending);
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
                    await UpdateStatus(rentalContract, (int)RentalContractStatus.Cancelled);
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
