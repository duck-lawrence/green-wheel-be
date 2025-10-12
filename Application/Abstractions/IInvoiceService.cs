using Application.Dtos.Invoice.Response;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Momo.Request;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IInvoiceService
    {
        Task UpdateInvoiceMomoPayment(MomoIpnReq momoIpnReq, Guid invoiceId);

        Task<InvoiceViewRes> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false);

        Task CashPayment(Invoice invoice);

        Task<string?> ProcessPayment(Guid id, int paymentMethod);

        Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination);

        Task<IEnumerable<InvoiceViewRes>?> GetByContractIdAndStatus(Guid? contractId, int? status);

    }
}