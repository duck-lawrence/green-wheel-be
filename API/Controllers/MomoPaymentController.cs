using API.Filters;
using Application;
using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Momo.Request;
using Application.Dtos.Payment.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handles all MoMo payment operations such as creating payment requests 
    /// and processing IPN callbacks.
    /// </summary>
    [Route("api/momo-payment")]
    [ApiController]
    public class MomoPaymentController(IMomoService momoService, IInvoiceService invoiceService) : ControllerBase
    {
        private readonly IMomoService _momoService = momoService;
        private readonly IInvoiceService _invoiceService = invoiceService;

        /// <summary>
        /// Creates a MoMo payment link for the specified invoice.
        /// </summary>
        /// <param name="req">Request containing invoice ID and fallback URL for MoMo payment.</param>
        /// <returns>Payment link if the creation is successful.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid signature.</response>
        /// <response code="401">Missing field when generating signature.</response>
        /// <response code="403">Do not have permission.</response>
        /// <response code="404">Invoice not found.</response>
        /// <response code="500">MoMo error or duplicate order ID.</response>
        [RoleAuthorize(RoleName.Staff)]
        [HttpPost]
        public async Task<IActionResult> CreateMomoPayment(CreateMomoPaymentReq req)
        {
            var invoice = await _invoiceService.GetRawInvoiceById(req.InvoiceId, true, true)
                ?? throw new NotFoundException(Message.InvoiceMessage.NotFound);

            string link = invoice.Type switch
            {
                (int)InvoiceType.Handover => await _invoiceService.PayHandoverInvoiceOnline(invoice, req.FallbackUrl),
                (int)InvoiceType.Reservation => await _invoiceService.PayReservationInvoiceOnline(invoice, req.FallbackUrl),
                (int)InvoiceType.Return => await _invoiceService.PayReturnInvoiceOnline(invoice, req.FallbackUrl),
                (int)InvoiceType.Refund => await _invoiceService.PayRefundInvoiceOnline(invoice, req.FallbackUrl),
                _ => throw new Exception(Message.InvoiceMessage.InvalidInvoiceType),
            };
            return Ok(new { link });
        }
    }

}
