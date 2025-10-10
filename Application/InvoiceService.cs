using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Invoice.Response;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Momo.Request;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceUow _uow;
        private readonly IMapper _mapper;
        private readonly IMomoService _momoService;

        public InvoiceService(IInvoiceUow uow, IMapper mapper, IMomoService momoService)
        {
            _uow = uow;
            _mapper = mapper;
            _momoService = momoService;
        }

        public async Task CashPayment(Invoice invoice)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId);
            if (contract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            if (contract.Status != (int)RentalContractStatus.PaymentPending)
            {
                throw new BadRequestException(Message.RentalContractMessage.ThisRentalContractAlreadyProcess);
            }
            contract.Status = (int)RentalContractStatus.Active;
            invoice.PaymentMethod = (int)PaymentMethod.Cash;
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.InvoiceRepository.UpdateAsync(invoice);
            await _uow.SaveChangesAsync();
        }

        public async Task<InvoiceViewRes> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false)
        {
            var invoice = await _uow.InvoiceRepository.GetByIdOptionAsync(id, includeItems, includeDeposit);
            if (invoice == null)
            {
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
            }
            var invoiceViewRes = _mapper.Map<InvoiceViewRes>(invoice);
            return invoiceViewRes;
        }

        public async Task<string?> ProcessPayment(Guid id, int paymentMethod)
        {
            var invoice = await _uow.InvoiceRepository.GetByIdOptionAsync(id, false, true);
            if (paymentMethod == (int)PaymentMethod.Cash)
            {
                await CashPayment(invoice);
            }
            else if (paymentMethod == (int)PaymentMethod.MomoWallet)
            {
                var amount = invoice.Subtotal + invoice.Deposit.Amount + invoice.Subtotal * invoice.Tax;
                var link = await _momoService.CreatePaymentAsync(amount, id, invoice.Notes);
                return link;
            }
            return null;
        }

        public async Task ProcessUpdateInvoice(MomoIpnReq momoIpnReq, Guid invoiceId)
        {
            if (momoIpnReq.ResultCode == (int)MomoPaymentStatus.Success)
            {
                var invoice = await _uow.InvoiceRepository.GetByIdAsync(invoiceId);
                if (invoice == null)
                {
                    throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
                }
                var rentalContract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId);
                if (rentalContract == null)
                {
                    throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
                }
                rentalContract.Status = (int)RentalContractStatus.Active;
                invoice.Status = (int)InvoiceStatus.Paid;
                invoice.PaymentMethod = (int)PaymentMethod.MomoWallet;
                invoice.PaidAmount = momoIpnReq.Amount;
                invoice.PaidAt = DateTimeOffset.FromUnixTimeMilliseconds(momoIpnReq.ResponseTime);
                await _uow.InvoiceRepository.UpdateAsync(invoice);
                await _uow.RentalContractRepository.UpdateAsync(rentalContract);
                await _uow.MomoPaymentLinkRepository.RemovePaymentLinkAsync(invoiceId.ToString());
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination)
        {
            var invoices = await _uow.InvoiceRepository.GetAllInvoicesAsync(pagination);

            if (invoices == null || !invoices.Items.Any())
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);

            return invoices;
        }
    }
}