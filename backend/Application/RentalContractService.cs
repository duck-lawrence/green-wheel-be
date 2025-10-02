
using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.RentalContract.Request;
using Application.Dtos.RentalContract.Respone;
using Application.Helpers;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractUow _uow;
        private readonly IMapper _mapper;
        public RentalContractService(IRentalContractUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
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
    }
}
