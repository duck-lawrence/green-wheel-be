using Application.Constants;
using Application.Dtos.User.Request;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string token, UserRegisterReq userRegisterReq);

        Task<string?> Login(UserLoginReq user);

        Task<int> Logout(string refreshToken);

        //Task<User> GetUserByEmail(string email);
        string GenerateAccessToken(Guid userId);

        Task<string> GenerateRefreshToken(Guid userId, ClaimsPrincipal? oldClaims);

        Task<string> RefreshToken(string refreshToken, bool getRevoked);

        Task<string> VerifyOTP(VerifyOTPReq verifyOTPDto, TokenType type, string cookiesKey);

        Task SendOTP(string email);

        Task ChangePassword(ClaimsPrincipal userClaims, UserChangePasswordReq userChangePasswordDto);

        Task ResetPassword(string forgotPasswordToken, string password);

        Task<Dictionary<string, string>> LoginWithGoogle(GoogleJsonWebSignature.Payload payload);
    }
}
