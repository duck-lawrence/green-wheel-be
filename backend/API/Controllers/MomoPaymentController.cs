using Application.Abstractions;
using Application.Dtos.Momo.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/momo-payment")]
    [ApiController]
    public class MomoPaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;

        
        public MomoPaymentController(IMomoService momoService)
        {
            _momoService = momoService;
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
        [HttpPost]
        public async Task<IActionResult> CreateMomoPayment(CreateMomoPaymentReq req)
        {
            var res = await _momoService.CreatePaymentAsync(req.Amount, req.InvoiceId, req.Description);
            return Ok(new { res });
        }


        
    }

}
