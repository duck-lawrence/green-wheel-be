using Application.Dtos.Common.Request;
using Application.Dtos.Momo.Request;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IInvoiceService
    {
        Task ProcessUpdateInvoice(MomoIpnReq momoIpnReq, Guid invoiceId);

        Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination);

        Task<Invoice> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false);

        Task CashPayment(Invoice invoice);

        //Task<IEnumerable<Invoice>> GetByContractId(Guid contractId);
    }
}