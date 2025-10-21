using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.Common.Request;
using Application.Dtos.DriverLicense.Request;
using Application.Dtos.Staff.Request;
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
        /// <summary>
        /// Retrieves all users with optional filters for phone number, citizen ID number, or driver license number.
        /// </summary>
        /// <param name="phone">Optional filter for the user's phone number.</param>
        /// <param name="citizenIdNumber">Optional filter for the user's citizen ID number.</param>
        /// <param name="driverLicenseNumber">Optional filter for the user's driver license number.</param>
        /// <returns>List of users matching the specified filters.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">No users found matching the given filters.</response>
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

        /// <summary>
        /// Retrieves all staff users with optional filters for name and station.
        /// </summary>
        /// <param name="name">Optional filter for the staff member's name.</param>
        /// <param name="stationId">Optional filter for the station the staff is assigned to.</param>
        /// <returns>List of staff members matching the specified filters.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">No staff members found matching the given filters.</response>
        [HttpGet("staffs")]
        [RoleAuthorize(RoleName.Admin)]
        public async Task<IActionResult> GetAllStaff(
           [FromQuery] string? name,
           [FromQuery] Guid? stationId
           )
        {
            var users = await _userService.GetAllStaffAsync(name, stationId);
            return Ok(users);
        }

        /// <summary>
        /// Creates a new user with the specified information.
        /// </summary>
        /// <param name="req">Request containing user details such as name, email, role, and station assignment.</param>
        /// <returns>The unique identifier of the created user.</returns>
        /// <response code="200">Success — user created.</response>
        /// <response code="400">Invalid user data.</response>
        /// <response code="409">User with the same email already exists.</response>
        [HttpPost]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> Create([FromBody] CreateUserReq req)
        {
            var userId = await _userService.CreateAsync(req);
            return Ok(new { userId });
        }
        /// <summary>
        /// Updates an existing user's information by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="req">Request containing updated user details such as name, role, or contact information.</param>
        /// <returns>Success message if the user information is updated successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid user data.</response>
        /// <response code="404">User not found.</response>
        [HttpPatch("{id}")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateById(Guid id, [FromBody] UserUpdateReq req)
        {
            await _userProfileService.UpdateAsync(id, req);
            return Ok();
        }

        /// <summary>
        /// Uploads a citizen identity image for a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="file">The image file of the citizen identity card to upload.</param>
        /// <returns>Uploaded citizen identity information.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid file format or upload error.</response>
        /// <response code="404">User not found.</response>
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
        /// <summary>
        /// Uploads a driver license image for a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="file">The image file of the driver license to upload.</param>
        /// <returns>Uploaded driver license information.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid file format or upload error.</response>
        /// <response code="404">User not found.</response>
        [HttpPut("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicenseById(Guid id, [FromForm] IFormFile file)
        {
            var driverLisence = await _userProfileService.UploadDriverLicenseAsync(id, file);
            return Ok(driverLisence);
        }
        /// <summary>
        /// Updates the citizen identity information of a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="req">Request containing updated citizen identity information.</param>
        /// <returns>Updated citizen identity details.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid citizen identity data.</response>
        /// <response code="404">User or citizen identity record not found.</response>

        [HttpPatch("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateCitizenIdentityById(Guid id, [FromBody] UpdateCitizenIdentityReq req)
        {
            var result = await _userProfileService.UpdateCitizenIdentityAsync(id, req);
            return Ok(result);
        }
        /// <summary>
        /// Updates the driver license information of a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="req">Request containing updated driver license information.</param>
        /// <returns>Updated driver license details.</returns>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid driver license data.</response>
        /// <response code="404">User or driver license record not found.</response>
        [HttpPatch("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> UpdateDriverLicenseById(Guid id, [FromBody] UpdateDriverLicenseReq req)
        {
            var result = await _userProfileService.UpdateDriverLicenseAsync(id, req);
            return Ok(result);
        }
        /// <summary>
        /// Deletes the citizen identity information of a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose citizen identity will be deleted.</param>
        /// <returns>Success message if the citizen identity is deleted successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">User or citizen identity record not found.</response>
        [HttpDelete("{id}/citizen-identity")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> DeleteCitizenIdentityById(Guid id)
        {
            await _userProfileService.DeleteCitizenIdentityAsync(id);
            return Ok();
        }
        /// <summary>
        /// Deletes the driver license information of a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose driver license will be deleted.</param>
        /// <returns>Success message if the driver license is deleted successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">User or driver license record not found.</response>
        [HttpDelete("{id}/driver-license")]
        [RoleAuthorize([RoleName.Admin, RoleName.Staff])]
        public async Task<IActionResult> DeleteDriverLicenseById(Guid id)
        {
            await _userProfileService.DeleteDriverLicenseAsync(id);
            return Ok();
        }
        /// <summary>
        /// Deletes a user by their unique identifier (admin only).
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>Success message if the user is deleted successfully.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">User not found.</response>
        /// <response code="403">Forbidden — only admins can perform this action.</response>
        [HttpDelete("{id}")]
        [RoleAuthorize(RoleName.Admin)]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            await _userService.DeleteCustomer(id);
            return Ok();
        }
        /// <summary>
        /// Creates a new staff account (admin only).
        /// </summary>
        /// <param name="req">Request containing staff information such as name, email, station assignment, and role.</param>
        /// <returns>The unique identifier of the created staff member.</returns>
        /// <response code="200">Success — staff created.</response>
        /// <response code="400">Invalid staff data.</response>
        /// <response code="409">A staff account with the same email already exists.</response>
        [HttpPost("create-staff")]
        [RoleAuthorize(RoleName.Admin)]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffReq req)
        {
            var staffId = await _userService.CreateStaffAsync(req);
            return Ok(new { staffId });
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