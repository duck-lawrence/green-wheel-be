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
    /// <summary>
    /// Manages user profile operations such as retrieving, updating, 
    /// and linking Google account information.
    /// </summary>
    [Route("api/me")]
    [ApiController]
    public class UserProfileController(IUserProfileSerivce service) : ControllerBase
    {
        private readonly IUserProfileSerivce _userProfileService = service;

        /// <summary>
        /// Retrieves the profile information of the currently authenticated user.
        /// </summary>
        /// <returns>User profile details of the logged-in user.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User profile not found.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userClaims = HttpContext.User;
            var userProfileViewRes = await _userProfileService.GetMeAsync(userClaims);
            return Ok(userProfileViewRes);
        }

        /// <summary>
        /// Updates the profile information of the currently authenticated user.
        /// </summary>
        /// <param name="userUpdateReq">Request containing updated user details such as name, phone, or address.</param>
        /// <returns>Success message if the profile is updated successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid profile data.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User not found.</response>
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateReq userUpdateReq)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.UpdateAsync(userId, userUpdateReq);
            return Ok();
        }

        /// <summary>
        /// Updates the bank information of the currently authenticated user.
        /// </summary>
        /// <param name="req">Request containing updated bank information.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid profile data.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User not found.</response>
        [HttpPut("bank-account")]
        [Authorize]
        public async Task<IActionResult> UpdateMyBankAccount([FromBody] UpdateBankAccountReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.UpdateBankAccountAsync(userId, req);
            return Ok();
        }

        /// <summary>
        /// Delete the bank information of the currently authenticated user.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User not found.</response>
        [HttpDelete("bank-account")]
        [Authorize]
        public async Task<IActionResult> DeleteMyBankAccount()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.DeleteBankAccountAsync(userId);
            return Ok();
        }

        /// <summary>
        /// Uploads or updates the avatar image of the currently authenticated user.
        /// </summary>
        /// <param name="request">Request containing the avatar image file to upload.</param>
        /// <returns>The URL of the uploaded avatar image.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid file format or upload error.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User not found.</response>
        [HttpPut("avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadImageReq request)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var avatarUrl = await _userProfileService.UploadAvatarAsync(userId, request.File);

            return Ok(new { AvatarUrl = avatarUrl });
        }

        /// <summary>
        /// Deletes the avatar image of the currently authenticated user.
        /// </summary>
        /// <returns>Success message if the avatar is deleted successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User or avatar not found.</response>
        [HttpDelete("avatar")]
        [Authorize]
        public async Task<IActionResult> DeleteAvatar()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.DeleteAvatarAsync(userId);

            return Ok(new { Message = Message.CloudinaryMessage.DeleteSuccess });
        }

        /// <summary>
        /// Uploads a citizen identity image for the currently authenticated user.
        /// </summary>
        /// <param name="file">The image file of the citizen identity card to upload.</param>
        /// <returns>Uploaded citizen identity information.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid file format or upload error.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User not found.</response>
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

        /// <summary>
        /// Uploads a driver license image for the currently authenticated user.
        /// </summary>
        /// <param name="file">The image file of the driver license to upload.</param>
        /// <returns>Uploaded driver license information.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid file format or upload error.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">User not found.</response>
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

        /// <summary>
        /// Retrieves the citizen identity information of the currently authenticated user.
        /// </summary>
        /// <returns>Citizen identity details of the logged-in user.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">Citizen identity record not found.</response>
        [HttpGet("citizen-identity")]
        [Authorize]
        public async Task<IActionResult> GetMyCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.GetMyCitizenIdentityAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the driver license information of the currently authenticated user.
        /// </summary>
        /// <returns>Driver license details of the logged-in user.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">Driver license record not found.</response>
        [HttpGet("driver-license")]
        [Authorize]
        public async Task<IActionResult> GetMyDriverLicense()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.GetMyDriverLicenseAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the citizen identity information of the currently authenticated user.
        /// </summary>
        /// <param name="req">Request containing updated citizen identity details.</param>
        /// <returns>Updated citizen identity information.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid citizen identity data.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">Citizen identity record not found.</response>
        [Authorize]
        [HttpPatch("citizen-identity")]
        public async Task<IActionResult> UpdateCitizenIdentity([FromBody] UpdateCitizenIdentityReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.UpdateCitizenIdentityAsync(userId, req);
            return Ok(result);
        }

        /// <summary>
        /// Updates the driver license information of the currently authenticated user.
        /// </summary>
        /// <param name="req">Request containing updated driver license details.</param>
        /// <returns>Updated driver license information.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid driver license data.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">Driver license record not found.</response>
        [Authorize]
        [HttpPatch("driver-license")]
        public async Task<IActionResult> UpdateDriverLicense([FromBody] UpdateDriverLicenseReq req)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userProfileService.UpdateDriverLicenseAsync(userId, req);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the citizen identity information of the currently authenticated user.
        /// </summary>
        /// <returns>Success message if the citizen identity information is deleted successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">Citizen identity record not found.</response>
        [Authorize]
        [HttpDelete("citizen-identity")]
        public async Task<IActionResult> DeleteCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userProfileService.DeleteCitizenIdentityAsync(userId);
            return Ok();
        }

        /// <summary>
        /// Deletes the driver license information of the currently authenticated user.
        /// </summary>
        /// <returns>Success message if the driver license information is deleted successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">Unauthorized — user is not authenticated.</response>
        /// <response code="404">Driver license record not found.</response>
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