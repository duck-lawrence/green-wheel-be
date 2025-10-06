using Application.Dtos.Momo.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IInvoiceService
    {
        Task ProcessUpdateInvoice(MomoIpnReq momoIpnReq, Guid invoiceId);
        Task<Invoice> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false);
        Task CashPayment(Invoice invoice);
        //Task<IEnumerable<Invoice>> GetByContractId(Guid contractId);
    }
}
