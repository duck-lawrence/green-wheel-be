using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Momo.Request;
using Application.Repositories;
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
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMomoPaymentLinkRepository _momoPaymentLinkRepositorys;

        public InvoiceService(IInvoiceRepository invoiceRepositor, IMomoPaymentLinkRepository momoPaymentLinkRepositorys)
        {
            _invoiceRepository = invoiceRepositor;
            _momoPaymentLinkRepositorys = momoPaymentLinkRepositorys;
        }

        public async Task<Invoice> GetInvoiceById(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if(invoice == null)
            {
                throw new NotFoundException(Message.Invoice.InvoiceNotFound);
            }
            return invoice;
        }

        public async Task ProcessUpdateInvoice(MomoIpnReq momoIpnReq, Guid invoiceId)
        {
            if(momoIpnReq.ResultCode == (int)MomoPaymentStatus.Success)
            {
                var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
                if(invoice == null)
                {
                    throw new NotFoundException(Message.Invoice.InvoiceNotFound);
                }
                invoice.Status = (int)InvoiceStatus.Paid;
                invoice.PaymentMethod = (int)PaymentMethod.MomoWallet;
                invoice.PaidAmount = momoIpnReq.Amount;
                invoice.PaidAt = DateTimeOffset.FromUnixTimeMilliseconds(momoIpnReq.ResponseTime);
                await _invoiceRepository.UpdateAsync(invoice);
                await _momoPaymentLinkRepositorys.RemovePaymentLinkAsync(invoiceId.ToString());
            }
        }
    }
}
