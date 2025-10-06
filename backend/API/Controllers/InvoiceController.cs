using Application;
using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.Dtos.Momo.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IInvoiceService _invoiceItemService;

        public InvoiceController(IMomoService momoService, IInvoiceService invoiceItemService)
        {
            _momoService = momoService;
            _invoiceItemService = invoiceItemService;
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
            await _invoiceItemService.ProcessUpdateInvoice(req, Guid.Parse(req.OrderId));
            return Ok(new { resultCode = 0, message = "Received" });
        }

        /*
         * status code
         * 200 success
         * 400 invoice not found
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            var invoice = await _invoiceItemService.GetInvoiceById(id);
            return Ok(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] PaginationParams pagination)
        {
            var result = await _invoiceItemService.GetAllInvoicesAsync(pagination);
            return Ok(result);
        }
    }
}