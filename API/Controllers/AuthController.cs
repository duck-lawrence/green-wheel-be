using Application.Abstractions;
using Application.Constants;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleCredentialService _googleService;
        private readonly IUserProfileSerivce _userProfileService;


        public AuthController(IAuthService authService, IGoogleCredentialService googleCredentialService, IUserProfileSerivce userProfileService)
        {
            _authService = authService;
            _googleService = googleCredentialService;
            _userProfileService = userProfileService;
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
            var accessToken = await _authService.Login(user);
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
                await _authService.Logout(refreshToken);
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
            await _userProfileService.CheckDupEmailAsync(email.Email);
            await _authService.SendOTP(email.Email);
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
            string registerToken = await _authService.VerifyOTP(verifyOTPDto, TokenType.RegisterToken, CookieKeys.RegisterToken);
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
                string accessToken = await _authService.RegisterAsync(registerToken, registerUserDto);
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
            await _authService.ChangePassword(user, userChangePasswordDto);
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
            await _authService.SendOTP(sendEmailRequestDto.Email);
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
            await _authService.VerifyOTP(verifyOTPDto, TokenType.ForgotPasswordToken, CookieKeys.ForgotPasswordToken);
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
                await _authService.ResetPassword(forgotPasswordToken, userChangePasswordDto.Password);
                return Ok();
            }
            throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
        }

        /*
         status code:
         200: refresh token successfully
         401: invalid token
         */

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

        /*
         * Status code
         * 
         */
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
