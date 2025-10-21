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
    [Route("api/momo-payment")]
    [ApiController]
    public class MomoPaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IInvoiceService _invoiceService;

        
        public MomoPaymentController(IMomoService momoService, IInvoiceService invoiceService)
        {
            _momoService = momoService;
            _invoiceService = invoiceService;
        }

        /*
         *status code
         *400: ivalid signature
         *401: missing field when generate signature
         *403: do not have permission
         *404: not found
         *500: momo error / duplicate order id
         *200: success
         */
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
