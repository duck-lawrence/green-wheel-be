using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.Common.Request;
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
        private readonly IGoogleCredentialService _googleService;

        public UserController(IUserService service
            , IGoogleCredentialService googleCredentialService
            )
        {
            _userService = service;
            _googleService = googleCredentialService;
        }

        /*
         Status code:
         200: Login successfully
         401: Invalid email or password
         400: Incorrect form of email, email is empty, password < 6 character, password empty
         */

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReq user)
        {
            var accessToken = await _userService.Login(user);
            return Ok(new
            {
                AccessToken = accessToken
            });
        }

        /*
         Status code
         200: logout successfully
         401: Invalid refresh token
         */

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue(CookieKeys.RefreshToken, out var refreshToken))
            {
                await _userService.Logout(refreshToken);
                return Ok();
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /*
         Status code:
         200: send email successfully
         400: incorrect form of email
         429: send to much request per minutes
         */

        [HttpPost("register")]
        public async Task<IActionResult> RegisterSendOtp([FromBody] SendEmailReq email)
        {
            await _userService.CheckDupEmailAsync(email.Email);
            await _userService.SendOTP(email.Email);
            return Ok();
        }

        /*
         Status code:
         200: verify email successfully
         401: incorrect OTP or this email not have otp, or send to much request
         400: inccorect from of email or otp is not digit
         */

        [HttpPost("register/verify-otp")]
        public async Task<IActionResult> RegisterVerifyOtp([FromBody] VerifyOTPReq verifyOTPDto)
        {
            string registerToken = await _userService.VerifyOTP(verifyOTPDto, TokenType.RegisterToken, CookieKeys.RegisterToken);
            return Ok();
        }

        /*
         Status code:
         400: incorrect form of user info
         401: invalid token
         409: email is exists
         200: register successfully
         */

        [HttpPost("register/complete")]
        public async Task<IActionResult> Register([FromBody] UserRegisterReq registerUserDto)
        {
            if (Request.Cookies.TryGetValue(CookieKeys.RegisterToken, out var registerToken))
            {
                string accessToken = await _userService.RegisterAsync(registerToken, registerUserDto);
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /*
         status code:
         400: pass is too short, empty password/oldpassword/confirmpassword, confirm password not match
         401: invalid old password
         200: change password successfully
         */

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordReq userChangePasswordDto)
        {
            //if (userChangePasswordDto.OldPassword == null)
            //{
            //    return BadRequest(Message.UserMessage.OldPasswordIsRequired);
            //}
            var user = HttpContext.User;
            await _userService.ChangePassword(user, userChangePasswordDto);
            return Ok();
        }

        /*
         Status code:
         200: send email successfully
         400: incorrect form of email
         429: send to much request per minute
         */

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] SendEmailReq sendEmailRequestDto)
        {
            await _userService.SendOTP(sendEmailRequestDto.Email);
            return Ok();
        }

        /*
         Status code:
         200: verify email successfully
         401: incorrect OTP or this email not have otp, or send to much request
         400: incorect from of email or otp is not digit
         */

        [HttpPost("forgot-password/verify-otp")]
        public async Task<IActionResult> ForgotPasswordVerifyOTP([FromBody] VerifyOTPReq verifyOTPDto)
        {
            await _userService.VerifyOTP(verifyOTPDto, TokenType.ForgotPasswordToken, CookieKeys.ForgotPasswordToken);
            return Ok();
        }

        /*
         status code:
         400: password is too short / confirm password does not match
         200: reset password successfully
         401: invalid token
         */

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserChangePasswordReq userChangePasswordDto)
        {
            if (Request.Cookies.TryGetValue(CookieKeys.ForgotPasswordToken, out var forgotPasswordToken))
            {
                await _userService.ResetPassword(forgotPasswordToken, userChangePasswordDto.Password);
                return Ok();
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /*
         status code:
         200: refresh token successfully
         401: invalid token
         */

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (Request.Cookies.TryGetValue(CookieKeys.RefreshToken, out var refreshToken))
            {
                string accessToken = await _userService.RefreshToken(refreshToken, false);
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleReq loginGoogleReqDto)
        {
            var payload = await _googleService.VerifyCredential(loginGoogleReqDto.Credential);

            Dictionary<string, string> tokens = await _userService.LoginWithGoogle(payload);
            bool needSetPassword = true;
            if (tokens.TryGetValue(TokenType.AccessToken.ToString(), out var token))
            {
                needSetPassword = false;
            }
            return Ok(new LoginGoogleRes
            {
                NeedSetPassword = needSetPassword,
                AccessToken = token,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userClaims = HttpContext.User;
            var userProfileViewRes = await _userService.GetMeAsync(userClaims);
            return Ok(userProfileViewRes);
        }

        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateReq userUpdateReq)
        {
            var userClaims = HttpContext.User;
            await _userService.UpdateMeAsync(userClaims, userUpdateReq);
            return Ok();
        }

        [HttpPut("avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadImageReq request)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var avatarUrl = await _userService.UploadAvatarAsync(userId, request.File);

            return Ok(new { AvatarUrl = avatarUrl });
        }

        [HttpDelete("avatar")]
        [Authorize]
        public async Task<IActionResult> DeleteAvatar()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            await _userService.DeleteAvatarAsync(userId);

            return Ok(new { Message = Message.CloudinaryMessage.DeleteSuccess });
        }

        [HttpPost("citizen-identity")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCitizenId([FromForm] IFormFile file)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.UploadCitizenIdAsync(userId, file);
            return Ok(new
            {
                citizen_identity = result
            });
        }

        [HttpGet("citizen-identity")]
        [Authorize]
        public async Task<IActionResult> GetMyCitizenIdentity()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.GetMyCitizenIdentityAsync(userId);

            if (result == null)
                return NotFound(new { Message = Message.LicensesMessage.LicenseNotFound });

            return Ok(result);
        }

        [HttpPost("driver-license")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicense([FromForm] IFormFile file)
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.UploadDriverLicenseAsync(userId, file);
            return Ok(new
            {
                driver_license = result
            });
        }

        // Lấy bằng user trong token
        [HttpGet("driver-license")]
        [Authorize]
        public async Task<IActionResult> GetMyDriverLicense()
        {
            var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sid)!.Value);
            var result = await _userService.GetMyDriverLicenseAsync(userId);

            if (result == null)
                return NotFound(new { Message = Message.LicensesMessage.LicenseNotFound });

            return Ok(result);
        }

        //Create anonymouse account
        [RoleAuthorize("Staff")]
        [HttpPost("anonymous")]
        public async Task<IActionResult> CreateAnonymouseAccount([FromForm] CreateUserReq req)
        {
            var userId = await _userService.CreateAnounymousAccount(req);
            return Ok(userId);
        }

        //upload citizenId for Anonymous
        [HttpPost("{id}/citizen-identity")]
        [RoleAuthorize("Staff")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCitizenIdForAnonymous(Guid id, [FromForm] IFormFile file)
        {
            var citizenIdentity = await _userService.UploadCitizenIdAsync(id, file);
            return Ok(citizenIdentity);
        }

        [HttpPost("{id}/driver-license")]
        [RoleAuthorize("Staff")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicenseForAnonymous(Guid id, [FromForm] IFormFile file)
        {
            var driverLisence = await _userService.UploadDriverLicenseAsync(id, file);
            return Ok(driverLisence);
        }

        /*
         * Status code
         * 200 success
         * 404 not found
         */

        [HttpGet("phone/{phone}")]
        [RoleAuthorize("Staff", "Admin")]
        public async Task<IActionResult> GetUserByPhone(string phone)
        {
            var user = await _userService.GetUserByPhoneAsync(phone);
            return Ok(user);
        }

        /*
         * Status code
         * 200 success
         */

        [HttpGet]
        [RoleAuthorize(RoleName.Staff, RoleName.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("citizen-identity/{idNumber}")]
        [RoleAuthorize("Staff", "Admin")]
        public async Task<IActionResult> getUserByCitizenIdNumber(string idNumber)
        {
            var userView = await _userService.GetByCitizenIdentityAsync(idNumber);
            return Ok(userView);
        }

        [HttpGet("driver-license/{number}")]
        [RoleAuthorize("Staff", "Admin")]
        public async Task<IActionResult> getUserByDriverLisence(string number)
        {
            var userView = await _userService.GetByDriverLicenseAsync(number);
            return Ok(userView);
        }
    }
}