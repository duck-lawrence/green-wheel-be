using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.Dtos.UserSupport.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/support-requests")]
    public class SupportRequestController : ControllerBase
    {
        private readonly ISupportRequestService _service;

        public SupportRequestController(ISupportRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize("Customer")]
        public async Task<IActionResult> Create([FromBody] CreateSupportReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var id = await _service.CreateAsync(userId, req);
            return Ok(new { Id = id });
        }

        [HttpGet("me")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var data = await _service.GetByCustomerAsync(userId);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var data = await _service.GetAllAsync(pagination);
            return Ok(data);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupportReq req)
        {
            var staffId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _service.UpdateAsync(id, req, staffId);
            return NoContent();
        }
    }
}