using Application;
using Application.Abstractions;
using Application.Dtos.RentalContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/rental-contracts")]
    [ApiController]
    public class RentalContractController : ControllerBase
    {
        private readonly IRentalContractService _rentalContractService;
        public RentalContractController(IRentalContractService rentalContractService)
        {
            _rentalContractService = rentalContractService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRentalContract(CreateRentalContractReq createReq)
        {
            var userClaims = HttpContext.User;
            var rentalContractViewRes = await _rentalContractService.CreateRentalContractAsync(userClaims, createReq);
            return Ok(new
            {
                rentalContractViewRes
            });
        }   
    }
}
