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
        public async Task<IActionResult> ProcessUpdateInvoice([FromBody] MomoIpnReq req)
        {
            await _momoService.VerifyMomoIpnReq(req);
            await _invoiceService.ProcessUpdateInvoice(req, Guid.Parse(req.OrderId));
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
            var invoice = await _invoiceService.GetInvoiceById(id, true, true);
            return Ok(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] PaginationParams pagination)
        {
            var result = await _invoiceService.GetAllInvoicesAsync(pagination);
            return Ok(result);
        }

        [HttpPut("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(Guid id, [FromBody] PaymentReq paymentReq)
        {
            var invoice = await _invoiceService.GetInvoiceById(id);
            if (paymentReq.PaymentMethod == (int)PaymentMethod.Cash)
            {
                await _invoiceService.CashPayment(invoice);
            }
            else if (paymentReq.PaymentMethod == (int)PaymentMethod.MomoWallet)
            {
                var amount = invoice.Subtotal + invoice.Subtotal * invoice.Tax;
                var link = await _momoService.CreatePaymentAsync(amount, id, invoice.Notes);
                return Ok(link);
            }
            return Ok();
        }
    }
}