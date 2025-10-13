using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.RentalContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
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
        [RoleAuthorize(RoleName.Customer)]
        public async Task<IActionResult> CreateRentalContract(CreateRentalContractReq createReq)
        {
            var userClaims = HttpContext.User;
            var userID = Guid.Parse(userClaims.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString());
            await _rentalContractService.CreateRentalContractAsync(userID, createReq);
            return Created(
                // rentalContractViewRes
            );
        }
        /*
         status code
         404 rental contract not found
         200 succes
         */
        [HttpPut("{id}/accept")]
        [RoleAuthorize(RoleName.Staff)]
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
        [RoleAuthorize(RoleName.Staff)]
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
        [RoleAuthorize(RoleName.Staff)]
        [HttpPost("offline")]
        public async Task<IActionResult> CreateRentalContractOffline(CreateRentalContractReq req)
        {
            var userId = req.CustomerId;
            if(userId == null)
            {
                return BadRequest(Message.UserMessage.UserIdIsRequired);
            } 
            await _rentalContractService.CreateRentalContractAsync((Guid)userId, req);
            return Created();
        }

        /*
        * status code
        * 404: rentalContract not found
        * 200: success
        */
        [RoleAuthorize(RoleName.Staff)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllRentalContactReq req)
        {
            var contractViews = await _rentalContractService.GetAll(req);
            return Ok(contractViews);
        }

        /*
         * Status code
         * 
         */
        [RoleAuthorize(RoleName.Staff)]
        [HttpPut("{id}/handover")]
        public async Task<IActionResult> HandoverRentalContract(Guid id, HandoverContractReq req)
        {
            var staff = HttpContext.User;
            await _rentalContractService.HandoverProcessRentalContractAsync(staff, id, req);
            return Ok();

        }

        /*
         * Status code
         * 404 contract not found
         * 200 success
         */
        [RoleAuthorize(RoleName.Staff)]
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnRentalContract(Guid id)
        {
            var staff = HttpContext.User;
            var invoiceView = await _rentalContractService.ReturnProcessRentalContractAsync(staff, id);
            return invoiceView == null ? Ok() : Ok(invoiceView);
        }

        [RoleAuthorize(RoleName.Customer)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyContracts(int status)
        {
            var user = HttpContext.User;
            var rentalViews = await _rentalContractService.GetMyContracts(user, status);
            return Ok(rentalViews);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateRentalContractStatus(Guid id)
        {
            await _rentalContractService.UpdateStatusAsync(id);
            return Ok();
        }

        public async Task<IActionResult> GetById(Guid id)
        {
            var contractView = await _rentalContractService.GetByIdAsync(id);
            return Ok(contractView);
        }
        
    }
}
