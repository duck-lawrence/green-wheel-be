using API.Filters;
using Application;
using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.Constants;
using Application.Dtos.Momo.Request;
using Application.Dtos.Payment.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Application.AppExceptions;

namespace API.Controllers
{
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IMomoService momoService,
            IInvoiceService invoiceItemService
            )
        {
            _momoService = momoService;
            _invoiceService = invoiceItemService;
        }

        /*
         * status code
         * 400 invalid signature
         * 200 success
         * 404 not found
         */

        [HttpPost("process-update")]
        public async Task<IActionResult> UpdateInvoiceMomoPayment([FromBody] MomoIpnReq req)
        {
            await _momoService.VerifyMomoIpnReq(req);
            await _invoiceService.UpdateInvoiceMomoPayment(req, Guid.Parse(req.OrderId));
            return Ok(new { resultCode = 0, message = "Received" });
        }

        /*
         * status code
         * 200 success
         * 404 invoice not found
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            var invoiceView = await _invoiceService.GetInvoiceById(id, true, true);
            return Ok(invoiceView);
        }

        /*
         500 invoice type error
         404 not found
         200 success
         */
        [HttpPut("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(Guid id, [FromBody] PaymentReq paymentReq)
        {
            var invoice = await _invoiceService.GetRawInvoiceById(id, false, true); 
            if(paymentReq.PaymentMethod == (int)PaymentMethod.Cash)
            {
                await _invoiceService.CashPayment(invoice);
                return Ok();
            }
            string link = invoice.Type switch
            {
                (int)InvoiceType.Handover => await _invoiceService.ProcessHandoverInvoice(invoice),
                (int)InvoiceType.Reservation => await _invoiceService.ProcessReservationInvoice(invoice),
                _ => throw new Exception(),
            };
            return Ok(new { link });
        }

        /*
         200 success
         */
        [RoleAuthorize(RoleName.Staff)]
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] PaginationParams pagination)
        {
            var result = await _invoiceService.GetAllInvoicesAsync(pagination);
            return Ok(result);
        }
    }
    
}