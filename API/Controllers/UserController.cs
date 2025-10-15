using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.Common.Request;
using Application.Dtos.DriverLicense.Request;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserProfileSerivce _userProfileService;

        public UserController(IUserService service, IGoogleCredentialService googleCredentialService, IUserProfileSerivce userProfileSerivce)
        {
            _userService = service;
            _userProfileService = userProfileSerivce;
        }
        [HttpGet]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? phone,
            [FromQuery] string? citizenIdNumber,
            [FromQuery] string? driverLicenseNumber)
        {
            var users = await _userService.GetAllAsync(phone, citizenIdNumber, driverLicenseNumber);
            return Ok(users);
        }

        //Create anonymous account
        [HttpPost]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> Create([FromBody] CreateUserReq req)
        {
            var userId = await _userService.CreateAsync(req);
            return Ok(new { userId });
        }

        [HttpPatch("{id}")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateById(Guid id, [FromBody] UserUpdateReq req)
        {
            await _userProfileService.UpdateAsync(id, req);
            return Ok();
        }

        //upload citizenId for Anonymous
        [HttpPut("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCitizenIdById(Guid id, [FromForm] IFormFile file)
        {
            var citizenIdentity = await _userProfileService.UploadCitizenIdAsync(id, file);
            return Ok(citizenIdentity);
        }

        [HttpPut("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicenseById(Guid id, [FromForm] IFormFile file)
        {
            var driverLisence = await _userProfileService.UploadDriverLicenseAsync(id, file);
            return Ok(driverLisence);
        }

        [HttpPatch("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateCitizenIdentityById(Guid id, [FromBody] UpdateCitizenIdentityReq req)
        {
            var result = await _userProfileService.UpdateCitizenIdentityAsync(id, req);
            return Ok(result);
        }

        [HttpPatch("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateDriverLicenseById(Guid id, [FromBody] UpdateDriverLicenseReq req)
        {
            var result = await _userProfileService.UpdateDriverLicenseAsync(id, req);
            return Ok(result);
        }

        [HttpDelete("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> DeleteCitizenIdentityById(Guid id)
        {
            await _userProfileService.DeleteCitizenIdentityAsync(id);
            return Ok();
        }

        [HttpDelete("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> DeleteDriverLicenseById(Guid id)
        {
            await _userProfileService.DeleteDriverLicenseAsync(id);
            return Ok();
        }

        /*
         * Status code
         * 200 success
         * 404 not found
         */

        //[HttpGet("phone/{phone}")]
        //[RoleAuthorize("Staff", "Admin")]
        //public async Task<IActionResult> GetUserByPhone(string phone)
        //{
        //    var user = await _userService.GetByPhoneAsync(phone);
        //    return Ok(user);
        //}

        ///*
        // * Status code
        // * 200 success
        // */

        //[HttpGet]
        //[RoleAuthorize(RoleName.Staff, RoleName.Admin)]
        //public async Task<IActionResult> GetAll()
        //{
        //    var users = await _userService.GetAllUsersAsync();
        //    return Ok(users);
        //}

        //[HttpGet("citizen-identity/{idNumber}")]
        //[RoleAuthorize("Staff", "Admin")]
        //public async Task<IActionResult> getUserByCitizenIdNumber(string idNumber)
        //{
        //    var userView = await _userService.GetByCitizenIdentityAsync(idNumber);
        //    return Ok(userView);
        //}

        //[HttpGet("driver-license/{number}")]
        //[RoleAuthorize("Staff", "Admin")]
        //public async Task<IActionResult> getUserByDriverLisence(string number)
        //{
        //    var userView = await _userService.GetByDriverLicenseAsync(number);
        //    return Ok(userView);
        //}
    }
}