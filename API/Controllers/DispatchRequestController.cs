using API.Filters;
using Application;
using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Dispatch.Request;
using Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/dispatch-requests")]
    [RoleAuthorize(RoleName.Admin)]
    public class DispatchRequestController : ControllerBase
    {
        private readonly IDispatchRequestService _dispatchRequestService;
        private readonly IUserService _userService;
        private readonly IStaffRepository _staffRepository;

        public DispatchRequestController(IDispatchRequestService dispatchRequestService, IUserService userService, IStaffRepository staffRepository)
        {
            _userService = userService;
            _dispatchRequestService = dispatchRequestService;
            _staffRepository = staffRepository;
        }

        [HttpPost]
        [RoleAuthorize(RoleName.Admin)]
        public async Task<IActionResult> Create([FromBody] CreateDispatchReq req)
        {
            var adminId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);

            var dispatchId = await _dispatchRequestService.CreateAsync(adminId, req);
            return Ok(new { DispatchId = dispatchId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateDispatchReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var staff = await _staffRepository.GetByUserIdAsync(userId)
                ?? throw new ForbidenException(Message.UserMessage.DoNotHavePermission);
            await _dispatchRequestService.UpdateStatusAsync(userId, staff.StationId, id, req);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? fromStationId,
            [FromQuery] Guid? toStationId,
            [FromQuery] DispatchRequestStatus? status)
        {
            var result = await _dispatchRequestService.GetAllAsync(fromStationId, toStationId, status);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var res = await _dispatchRequestService.GetByIdAsync(id);
            if (res == null)
                return NotFound();

            return Ok(res);
        }
    }
}