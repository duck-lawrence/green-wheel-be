using Application.Dtos.Common.Request;
using Application.Dtos.Momo.Request;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IInvoiceService
    {
        Task ProcessUpdateInvoice(MomoIpnReq momoIpnReq, Guid invoiceId);

        Task<Invoice> GetInvoiceById(Guid id);

        Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination);
    }
}