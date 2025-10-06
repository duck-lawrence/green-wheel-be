using API.Filters;
using Application;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.RentalContract.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/rental-contracts")]
    [ApiController]
    public class RentalContractController : ControllerBase
    {

        private readonly IRentalContractService _rentalContractService;
        
        public RentalContractController(IRentalContractService rentalContractService
            )
        {
            _rentalContractService = rentalContractService;
            
        }
        /*
         status code
         404: vehicle, model not found
         422: business error (citizen id)
         200: success
         */
        [HttpPost]
        [Authorize]
        [RoleAuthorize("Customer")]
        public async Task<IActionResult> CreateRentalContract(CreateRentalContractReq createReq)
        {
            var userClaims = HttpContext.User;
            var userID = Guid.Parse(userClaims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString());
            var rentalContractViewRes = await _rentalContractService.CreateRentalContractAsync(userID, createReq);
            return Ok(
                rentalContractViewRes
            );
        }
        /*
         status code
         200 success
         */
        [HttpGet]
        [RoleAuthorize("Staff", "Admin")]
        public async Task<IActionResult> GetByStatus([FromQuery] int status)
        {
            var rcList = await _rentalContractService.GetByStatus(status);
            return Ok(rcList);
        }

        /*
         status code
         404 rental contract not found
         200 succes
         */
        [HttpPut("{id}/accept")]
        [RoleAuthorize("Staff")]
        public async Task<IActionResult> AcceptRentalContract(Guid id)
        {
            await _rentalContractService.VerifyRentalContract(id);
            return Ok();
        }

        /*
         status code
         404 rental contract not found
         200 succes
         */
        [HttpPut("{id}/reject")]
        [RoleAuthorize("Staff")]
        public async Task<IActionResult> RejectRentalContract(Guid id, [FromBody] int vehicleStatus)
        {
            await _rentalContractService.VerifyRentalContract(id, false, vehicleStatus);
            return Ok();
        }

        /*
        * status code
        * 404: vehicle, model not found
        * 422: business error (citizen id)
        * 200: success
        */
        [HttpPost("offline")]
        public async Task<IActionResult> CreateRentalContractOffline(CreateRentalContractReq req)
        {
            var userId = req.customerId;
            if(userId == null)
            {
                return BadRequest(Message.UserMessage.UserIdIsRequired);
            } 
            var rentalContractViewRes = await _rentalContractService.CreateRentalContractAsync((Guid)userId, req);
            return Ok(rentalContractViewRes);
        }
    }
}
