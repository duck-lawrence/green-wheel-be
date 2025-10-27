using Application.Abstractions;
using Application.Constants;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handles user authentication and Google OAuth login.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IGoogleCredentialService googleCredentialService, IUserProfileSerivce userProfileService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IGoogleCredentialService _googleService = googleCredentialService;
        private readonly IUserProfileSerivce _userProfileService = userProfileService;

        /// <summary>
        /// Authenticates a user and returns an access token if the credentials are valid.
        /// </summary>
        /// <param name="user">User login request containing email and password.</param>
        /// <returns>Access token if login is successful.</returns>
        /// <response code="200">Login successfully.</response>
        /// <response code="400">Invalid input format (email/password).</response>
        /// <response code="401">Invalid email or password.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReq user)
        {
            var accessToken = await _authService.Login(user);
            return Ok(new
            {
                AccessToken = accessToken
            });
        }

        /// <summary>
        /// Logs out the user by invalidating the refresh token.
        /// </summary>
        /// <returns>Logout success message if operation is successful.</returns>
        /// <response code="200">Logout successfully.</response>
        /// <response code="401">Invalid or expired refresh token.</response>

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue(CookieKeys.RefreshToken, out var refreshToken))
            {
                await _authService.Logout(refreshToken);
                return Ok();
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /// <summary>
        /// Sends a verification email to the specified address.
        /// </summary>
        /// <param name="email">Email request containing recipient address and other required data.</param>
        /// <returns>Success message if the email is sent successfully.</returns>
        /// <response code="200">Send email successfully.</response>
        /// <response code="400">Incorrect form of email.</response>
        /// <response code="429">Too many requests per minute.</response>

        [HttpPost("register")]
        public async Task<IActionResult> RegisterSendOtp([FromBody] SendEmailReq email)
        {
            await _userProfileService.CheckDupEmailAsync(email.Email);
            await _authService.SendOTP(email.Email);
            return Ok();
        }

        /// <summary>
        /// Verifies the OTP code sent to the user's email address.
        /// </summary>
        /// <param name="verifyOTPDto">Verification request containing email and OTP code.</param>
        /// <returns>Success message if the email is verified successfully.</returns>
        /// <response code="200">Verify email successfully.</response>
        /// <response code="400">Incorrect form of email or OTP is not a valid digit.</response>
        /// <response code="401">Incorrect OTP, email has no OTP, or too many requests.</response>

        [HttpPost("register/verify-otp")]
        public async Task<IActionResult> RegisterVerifyOtp([FromBody] VerifyOTPReq verifyOTPDto)
        {
            string registerToken = await _authService.VerifyOTP(verifyOTPDto, TokenType.RegisterToken, CookieKeys.RegisterToken);
            return Ok();
        }

        /// <summary>
        /// Registers a new user with provided information.
        /// </summary>
        /// <param name="registerUserDto">User registration request containing email, password, and other user details.</param>
        /// <returns>Success message if the user is registered successfully.</returns>
        /// <response code="200">Register successfully.</response>
        /// <response code="400">Incorrect form of user information.</response>
        /// <response code="401">Invalid or missing token.</response>
        /// <response code="409">Email already exists.</response>

        [HttpPost("register/complete")]
        public async Task<IActionResult> Register([FromBody] UserRegisterReq registerUserDto)
        {
            if (Request.Cookies.TryGetValue(CookieKeys.RegisterToken, out var registerToken))
            {
                string accessToken = await _authService.RegisterAsync(registerToken, registerUserDto);
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /// <summary>
        /// Changes the user's password after validating the old password.
        /// </summary>
        /// <param name="userChangePasswordDto">Request containing old password, new password, and confirm password.</param>
        /// <returns>Success message if the password is changed successfully.</returns>
        /// <response code="200">Change password successfully.</response>
        /// <response code="400">Password too short, empty fields, or confirm password does not match.</response>
        /// <response code="401">Invalid old password.</response>

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordReq userChangePasswordDto)
        {
            //if (userChangePasswordDto.OldPassword == null)
            //{
            //    return BadRequest(Message.UserMessage.OldPasswordIsRequired);
            //}
            var user = HttpContext.User;
            await _authService.ChangePassword(user, userChangePasswordDto);
            return Ok();
        }

        /// <summary>
        /// Sends a verification or notification email to the specified address.
        /// </summary>
        /// <param name="sendEmailRequestDto">Email request containing recipient address and required information.</param>
        /// <returns>Success message if the email is sent successfully.</returns>
        /// <response code="200">Send email successfully.</response>
        /// <response code="400">Incorrect form of email.</response>
        /// <response code="429">Send too many requests per minute.</response>

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] SendEmailReq sendEmailRequestDto)
        {
            await _authService.SendOTP(sendEmailRequestDto.Email);
            return Ok();
        }

        /// <summary>
        /// Verifies the OTP code sent to the user's email address.
        /// </summary>
        /// <param name="verifyOTPDto">Verification request containing email and OTP code.</param>
        /// <returns>Success message if the email is verified successfully.</returns>
        /// <response code="200">Verify email successfully.</response>
        /// <response code="400">Incorrect form of email or OTP is not a valid digit.</response>
        /// <response code="401">Incorrect OTP, email has no OTP, or too many requests.</response>

        [HttpPost("forgot-password/verify-otp")]
        public async Task<IActionResult> ForgotPasswordVerifyOTP([FromBody] VerifyOTPReq verifyOTPDto)
        {
            await _authService.VerifyOTP(verifyOTPDto, TokenType.ForgotPasswordToken, CookieKeys.ForgotPasswordToken);
            return Ok();
        }

        /// <summary>
        /// Resets the user's password using the token stored in cookies.
        /// </summary>
        /// <param name="userChangePasswordDto">Request containing the new password and confirm password.</param>
        /// <returns>Success message if the password is reset successfully.</returns>
        /// <response code="200">Reset password successfully.</response>
        /// <response code="400">Password is too short or confirm password does not match.</response>
        /// <response code="401">Invalid or missing reset token.</response>

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserChangePasswordReq userChangePasswordDto)
        {
            if (Request.Cookies.TryGetValue(CookieKeys.ForgotPasswordToken, out var forgotPasswordToken))
            {
                await _authService.ResetPassword(forgotPasswordToken, userChangePasswordDto.Password);
                return Ok();
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /// <summary>
        /// Generates a new access token using a valid refresh token from cookies.
        /// </summary>
        /// <returns>New access token if the refresh token is valid.</returns>
        /// <response code="200">Refresh token successfully.</response>
        /// <response code="401">Invalid or missing refresh token.</response>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (Request.Cookies.TryGetValue(CookieKeys.RefreshToken, out var refreshToken))
            {
                string accessToken = await _authService.RefreshToken(refreshToken, false);
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /// <summary>
        /// Logs in or registers a user using Google account credentials.
        /// </summary>
        /// <param name="loginGoogleReqDto">Request containing Google credential token.</param>
        /// <returns>Access token and user information if login is successful.</returns>
        /// <response code="200">Login with Google successfully.</response>
        /// <response code="401">Invalid Google credential.</response>
        [HttpPost("google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleReq loginGoogleReqDto)
        {
            var payload = await _googleService.VerifyCredential(loginGoogleReqDto.Credential);

            Dictionary<string, string> tokens = await _authService.LoginWithGoogle(payload);
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

    }
}
