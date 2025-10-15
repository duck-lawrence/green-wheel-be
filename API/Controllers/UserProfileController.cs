using Application.Abstractions;
using Application.Constants;
using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.Common.Request;
using Application.Dtos.DriverLicense.Request;
using Application.Dtos.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [Route("api/me")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileSerivce _userProfileService;

        public UserProfileController(IUserProfileSerivce service, IGoogleCredentialService googleCredentialService)
        {
            _userProfileService = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userClaims = HttpContext.User;
            var userProfileViewRes = await _userProfileService.GetMeAsync(userClaims);
            return Ok(userProfileViewRes);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateReq userUpdateReq)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.UpdateAsync(userId, userUpdateReq);
            return Ok();
        }

        [HttpPut("avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadImageReq request)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var avatarUrl = await _userProfileService.UploadAvatarAsync(userId, request.File);

            return Ok(new { AvatarUrl = avatarUrl });
        }

        [HttpDelete("avatar")]
        [Authorize]
        public async Task<IActionResult> DeleteAvatar()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.DeleteAvatarAsync(userId);

            return Ok(new { Message = Message.CloudinaryMessage.DeleteSuccess });
        }
        [HttpPut("citizen-identity")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCitizenId([FromForm] IFormFile file)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.UploadCitizenIdAsync(userId, file);
            return Ok(result);
        }

        [HttpPut("driver-license")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicense([FromForm] IFormFile file)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.UploadDriverLicenseAsync(userId, file);
            return Ok(result);
        }

        [HttpGet("citizen-identity")]
        [Authorize]
        public async Task<IActionResult> GetMyCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.GetMyCitizenIdentityAsync(userId);
            return Ok(result);
        }

        [HttpGet("driver-license")]
        [Authorize]
        public async Task<IActionResult> GetMyDriverLicense()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.GetMyDriverLicenseAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("citizen-identity")]
        public async Task<IActionResult> UpdateCitizenIdentity([FromBody] UpdateCitizenIdentityReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.UpdateCitizenIdentityAsync(userId, req);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("driver-license")]
        public async Task<IActionResult> UpdateDriverLicense([FromBody] UpdateDriverLicenseReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.UpdateDriverLicenseAsync(userId, req);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("citizen-identity")]
        public async Task<IActionResult> DeleteCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.DeleteCitizenIdentityAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpDelete("driver-license")]
        public async Task<IActionResult> DeleteDriverLicense()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.DeleteDriverLicenseAsync(userId);
            return Ok();
        }
    }
}
