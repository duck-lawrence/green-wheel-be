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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IInvoiceService _invoiceService;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;

        public InvoiceController(IMomoService momoService,
            IInvoiceService invoiceItemService,
            IMemoryCache cache,
            IUserService userService
            )
        {
            _momoService = momoService;
            _invoiceService = invoiceItemService;
            _cache = cache;
            _userService = userService;
        }

        /*
         * status code
         * 400 invalid signature
         * 200 success
         * 404 not found
         */

        [HttpPost("payment-callback/momo")]
        public async Task<IActionResult> UpdateInvoiceMomoPayment([FromBody] MomoIpnReq req)
        {
            await _momoService.VerifyMomoIpnReq(req);
            await _invoiceService.UpdateInvoiceMomoPayment(req, Guid.Parse(req.OrderId.Substring(0, req.OrderId.Length - 6)));
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
        [RoleAuthorize(RoleName.Staff, RoleName.Customer)]
        public async Task<IActionResult> ProcessPayment(Guid id, [FromBody] PaymentReq paymentReq)
        {
            //kiểm tra phải hoá đơn của nó không
            var userId = Guid.Parse(HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString());
            var invoice = await _invoiceService.GetRawInvoiceById(id, true, true);
            var roles = _cache.Get<List<Role>>("AllRoles");
            var userInDB = await _userService.GetByIdAsync(userId);
            var userRole = roles.FirstOrDefault(r => r.Id == userInDB.RoleId).Name;
            if (userRole == RoleName.Customer)
            {
                if (invoice.Contract.CustomerId != userId)
                    throw new BusinessException(Message.InvoiceMessage.ForbiddenInvoiceAccess);
            }

            if (invoice.Status == (int)InvoiceStatus.Cancelled || invoice.Status == (int)InvoiceStatus.Paid) 
                throw new BusinessException(Message.InvoiceMessage.ThisInvoiceWasPaidOrCancel);
            if(paymentReq.PaymentMethod == (int)PaymentMethod.Cash)
            {
                if (paymentReq.Amount == null) throw new BadRequestException(Message.InvoiceMessage.AmountRequired);
                
                switch (invoice.Type)
                {
                    case (int)InvoiceType.Handover:
                        await _invoiceService.PayHandoverInvoiceManual(invoice, (decimal)paymentReq.Amount);
                        break;
                    case (int)InvoiceType.Return:
                        await _invoiceService.PayReturnInvoiceManual(invoice, (decimal)paymentReq.Amount);
                        break;
                    case (int)InvoiceType.Reservation:
                        await _invoiceService.PayReservationInvoiceManual(invoice, (decimal)paymentReq.Amount);
                        break;
                    default:
                        throw new Exception(Message.InvoiceMessage.InvalidInvoiceType);
                       
                }

                return Ok();
            }
            string link = invoice.Type switch
            {
                (int)InvoiceType.Handover => await _invoiceService.PayHandoverInvoiceOnline(invoice, paymentReq.FallbackUrl),
                (int)InvoiceType.Reservation => await _invoiceService.PayReservationInvoiceOnline(invoice, paymentReq.FallbackUrl),
                (int)InvoiceType.Return => await _invoiceService.PayReturnInvoiceOnline(invoice, paymentReq.FallbackUrl),
                _ => throw new Exception(Message.InvoiceMessage.InvalidInvoiceType),
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