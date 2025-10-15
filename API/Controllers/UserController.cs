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

        public UserController(IUserService service
            , IGoogleCredentialService googleCredentialService
            )
        {
            _userService = service;
        }

        // ===========================
        // Profile
        // ===========================

        #region profile

        

        #region document

        [HttpPut("citizen-identity")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCitizenId([FromForm] IFormFile file)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.UploadCitizenIdAsync(userId, file);
            return Ok(result);
        }

        [HttpPut("driver-license")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicense([FromForm] IFormFile file)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.UploadDriverLicenseAsync(userId, file);
            return Ok(result);
        }

        [HttpGet("citizen-identity")]
        [Authorize]
        public async Task<IActionResult> GetMyCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.GetMyCitizenIdentityAsync(userId);
            return Ok(result);
        }

        [HttpGet("driver-license")]
        [Authorize]
        public async Task<IActionResult> GetMyDriverLicense()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.GetMyDriverLicenseAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("citizen-identity")]
        public async Task<IActionResult> UpdateCitizenIdentity([FromBody] UpdateCitizenIdentityReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.UpdateCitizenIdentityAsync(userId, req);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("driver-license")]
        public async Task<IActionResult> UpdateDriverLicense([FromBody] UpdateDriverLicenseReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.UpdateDriverLicenseAsync(userId, req);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("citizen-identity")]
        public async Task<IActionResult> DeleteCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userService.DeleteCitizenIdentityAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpDelete("driver-license")]
        public async Task<IActionResult> DeleteDriverLicense()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userService.DeleteDriverLicenseAsync(userId);
            return Ok();
        }

        #endregion document

        #endregion profile

        // ===========================
        // ===== User Management =====
        // ===========================

        #region user-management

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
            await _userService.UpdateAsync(id, req);
            return Ok();
        }

        //upload citizenId for Anonymous
        [HttpPut("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCitizenIdById(Guid id, [FromForm] IFormFile file)
        {
            var citizenIdentity = await _userService.UploadCitizenIdAsync(id, file);
            return Ok(citizenIdentity);
        }

        [HttpPut("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicenseById(Guid id, [FromForm] IFormFile file)
        {
            var driverLisence = await _userService.UploadDriverLicenseAsync(id, file);
            return Ok(driverLisence);
        }

        [HttpPatch("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateCitizenIdentityById(Guid id, [FromBody] UpdateCitizenIdentityReq req)
        {
            var result = await _userService.UpdateCitizenIdentityAsync(id, req);
            return Ok(result);
        }

        [HttpPatch("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateDriverLicenseById(Guid id, [FromBody] UpdateDriverLicenseReq req)
        {
            var result = await _userService.UpdateDriverLicenseAsync(id, req);
            return Ok(result);
        }

        [HttpDelete("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> DeleteCitizenIdentityById(Guid id)
        {
            await _userService.DeleteCitizenIdentityAsync(id);
            return Ok();
        }

        [HttpDelete("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> DeleteDriverLicenseById(Guid id)
        {
            await _userService.DeleteDriverLicenseAsync(id);
            return Ok();
        }

        #endregion user-management

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