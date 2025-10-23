using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.StationFeedback.Request;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/station-feedbacks")]
    public class StationFeedbackController : ControllerBase
    {
        private readonly IStationFeedbackService _service;

        public StationFeedbackController(IStationFeedbackService service)
        {
            _service = service;
        }

        [HttpPost]
        [RoleAuthorize(RoleName.Customer)]
        public async Task<IActionResult> Create([FromBody] StationFeedbackCreateReq req)
        {
            var customerId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _service.CreateAsync(req, customerId);
            return Ok(result);
        }

        [HttpGet("station/{stationId}")]
        public async Task<IActionResult> GetByStationId(Guid stationId)
        {
            var data = await _service.GetByStationIdAsync(stationId);
            return Ok(data);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyFeedbacks()
        {
            var customerId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var data = await _service.GetByCustomerIdAsync(customerId);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customerId = Guid.Parse(User.FindFirst("sid")!.Value);
            await _service.DeleteAsync(id, customerId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }
    }
}