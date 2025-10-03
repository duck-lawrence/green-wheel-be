
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
        public async Task<RentalContractViewRes> CreateRentalContractAsync(ClaimsPrincipal userClaims, CreateRentalContractReq createReq)
        {
            var userID = Guid.Parse(userClaims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString());
            //ktra xem có cccd hay chưa

            //---------

            //ktra có đơn đặt xe chưa
            if(await _uow.RentalContracts.HasActiveContractAsync(userID))
            {
                throw new BusinessException(Message.RentalContract.UserAlreadyHaveContract);
            }
            var station = await _uow.Stations.GetByIdAsync(createReq.StationId) ??
                                                        throw new NotFoundException(Message.Station.StationNotFound);
            var vehicle = await _uow.Vehicles.GetVehicle(createReq.StationId,
                                                        createReq.ModelId,
                                                        createReq.StartDate,
                                                        createReq.EndDate) ??
                                                        throw new NotFoundException(Message.Vehicle.VehicleNotFound);

            var model = await _uow.VehicleModels.GetByIdAsync(createReq.ModelId);

            var days = (int)Math.Ceiling((createReq.EndDate - createReq.StartDate).TotalDays);
            Guid contractId;
            do
            {
                contractId = Guid.NewGuid();

            } while (await _uow.RentalContracts.GetByIdAsync(contractId) != null);
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
            await _uow.RentalContracts.AddAsync(contract);

            //Update vehicle
            await _uow.Vehicles.UpdateStatusAsync(vehicle.Id, (int)VehicleStatus.Unavaible);

            //Create invoice
            Guid invoiceId;
            do
            {
                invoiceId = Guid.NewGuid();

            } while (await _uow.Invoices.GetByIdOptionAsync(invoiceId) != null);
            var invoice = new Invoice()
            {
                Id = invoiceId,
                ContractId = contractId,
                Status = (int)InvoiceStatus.Pending,
                Tax = 0.1m, //10% dạng decimal
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                DeletedAt = null,
                Notes = "System generated invoice for rental contract"
            };
            await _uow.Invoices.AddAsync(invoice);

            //Create invoice Item
            var InvoiceItemNote = AutoGenerateHelper.GenerateInvoiceItemNotes(InvoiceItemType.BaseRental, days);
            Guid invoiceItemId;
            do
            {
                invoiceItemId = Guid.NewGuid();

            } while ((await _uow.InvoiceItems.GetByIdAsync(invoiceItemId)) != null);
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
            await _uow.InvoiceItems.AddRangeAsync([items]);

            //Deposit
            Guid depositId;
            do
            {
                depositId = Guid.NewGuid();

            } while (await _uow.Deposits.GetByIdAsync(depositId) != null);
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
            
            await _uow.Deposits.AddAsync(deposit);
            invoice.Subtotal = items.Quantity * items.UnitPrice + deposit.Amount;


            await _uow.SaveChangesAsync();
            var rentalContractViewRespone = _mapper.Map<RentalContractViewRes>(contract);
            return rentalContractViewRespone;
        }

        public async Task<IEnumerable<RentalContractForStaffViewRes>> GetByStatus(int status)
        {
            var RcList = await _uow.RentalContracts.GetByStatus(status);
            var RcForStaffViewRes = _mapper.Map<IEnumerable<RentalContractForStaffViewRes>>(RcList);
            return RcForStaffViewRes;
        }

        public async Task UpdateStatus(RentalContract rentalContract, int status)
        {
            
            rentalContract.Status = status;
            await _uow.RentalContracts.UpdateAsync(rentalContract);
        }

        /*
         Dear [CustomerName],

        Thank you for booking a vehicle with GreenWheel.  
        To complete your reservation, please proceed with the payment:

        - Booking ID: {rentalContract.Id}  
        - Rental period: [rentalContract.StartDate] – [rentalContract.EndDate]  
        - Amount due: [TotalAmount]

        You can pay directly in the GreenWheel app or click the link below:  
        [LinkSet]

        Note: Your reservation will be automatically cancelled if payment is not completed before [HUHU].

        Best regards,  
        The HYCAT Team
         */
        public async Task VerifyRentalContract(Guid id, int status)
        {
            var rentalContract = await _uow.RentalContracts.GetByIdAsync(id);
            if (rentalContract == null)
            {
                throw new NotFoundException(Message.RentalContract.RentalContractNotFound);
            }
            await UpdateStatus(rentalContract, status);
            //Lấy customer
            var customer = (await _uow.RentalContracts.GetAllAsync(new Expression<Func<RentalContract, object>>[]
            {
                rc => rc.Customer
            })).Where(rc => rc.Id == id)
            .Select(rc => rc.Customer).FirstOrDefault();
            
            //Lấy invoice
            var invoice = (await _uow.RentalContracts.GetAllAsync(new Expression<Func<RentalContract, object>>[]
            {
                rc => rc.Invoices
            })).Where(rc => rc.Id == id)
            .Select(rc => rc.Invoices).FirstOrDefault();

            string subject = "Hello đây là mail đến từ nhà vệ sinh";
            string templatePath = Path.Combine("../Application", "Templates", "PaymentEmailTemplate.txt");
            string body = System.IO.File.ReadAllText(templatePath);

            body = body.Replace("{CustomerName}", customer.LastName + " " + customer.FirstName)
                       .Replace("{BookingId}", rentalContract.Id.ToString())
                       .Replace("{StartDate}", rentalContract.StartDate.ToString("dd/MM/yyyy"))
                       .Replace("{EndDate}", rentalContract.EndDate.ToString("dd/MM/yyyy"))
                       .Replace("{AmountDue}", (invoice.First().Tax * invoice.First().Subtotal).ToString("C"))
                       .Replace("{PaymentLink}", "https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=RDdQw4w9WgXcQ&start_radio=1")
                       .Replace("{Deadline}", "NGU");


            await EmailHelper.SendEmailAsync(_emailSettings, customer.Email, subject, body);

        }
    }
}
