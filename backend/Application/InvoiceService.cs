using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Momo.Request;
using Application.Repositories;
using Application.UnitOfWorks;
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
        public InvoiceService(IInvoiceUow uow)
        {
            _uow = uow;

        }

        public async Task CashPayment(Invoice invoice)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId);
            if(contract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            if(contract.Status != (int)RentalContractStatus.PaymentPending)
            {
                throw new BadRequestException(Message.RentalContractMessage.ThisRentalContractAlreadyProcess);
            }
            contract.Status = (int)RentalContractStatus.Active;
            invoice.PaymentMethod = (int)PaymentMethod.Cash;
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.InvoiceRepository.UpdateAsync(invoice);
            await _uow.SaveChangesAsync();
        }

        public async Task<Invoice> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false)
        {
            var invoice = await _uow.InvoiceRepository.GetByIdOptionAsync(id, includeItems, includeDeposit);
            if(invoice == null)
            {
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
            }
            return invoice;
        }

        

        public async Task ProcessUpdateInvoice(MomoIpnReq momoIpnReq, Guid invoiceId)
        {
            if(momoIpnReq.ResultCode == (int)MomoPaymentStatus.Success)
            {
                var invoice = await _uow.InvoiceRepository.GetByIdAsync(invoiceId);
                if(invoice == null)
                {
                    throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
                }
                var rentalContract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId);
                if(rentalContract == null)
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


    }
}
