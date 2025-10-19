using Application.Dtos.Invoice.Response;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Momo.Request;
using Domain.Entities;
using Application.Dtos.InvoiceItem.Request;
using Application.Dtos.Invoice.Request;

namespace Application.Abstractions
{
    public interface IInvoiceService
    {
        Task UpdateInvoiceMomoPayment(MomoIpnReq momoIpnReq, Guid invoiceId);

        Task<InvoiceViewRes> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false);

        Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination);

        Task<IEnumerable<InvoiceViewRes>?> GetByContractIdAndStatus(Guid? contractId, int? status);

        Task PayHandoverInvoiceManual(Invoice invoice, decimal amount);
        Task PayReservationInvoiceManual(Invoice invoice, decimal amount);
        Task PayReturnInvoiceManual(Invoice invoice, decimal amount);
        Task<string> PayReservationInvoiceOnline(Invoice invoice, string fallbackUrl);

        Task<string> PayHandoverInvoiceOnline(Invoice invoice, string fallbackUrl);

        Task<string> PayReturnInvoiceOnline(Invoice invoice, string fallbackUrl);
        Task<string> PayRefundInvoiceOnline(Invoice invoice, string fallbackUrl);

        Task<Invoice> GetRawInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false);
        Task CreateAsync(CreateInvoiceReq req);
        Task UpdateAsync(Guid invoiceId, UpdateInvoiceReq req);
        Task UpdateNoteAsync(Guid id, string notes);
    }
}