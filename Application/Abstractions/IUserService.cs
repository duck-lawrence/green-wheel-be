using Application.Constants;
using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.CitizenIdentity.Response;
using Application.Dtos.DriverLicense.Request;
using Application.Dtos.DriverLicense.Response;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Domain.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Abstractions
{
    public interface IUserService
    {
        // ===========================
        // Auth
        // ===========================
        #region auth
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

        #endregion

        // ===========================
        // Profile
        // ===========================
        #region profile
        Task<UserProfileViewRes> GetMeAsync(ClaimsPrincipal userClaims);

        Task UpdateAsync(Guid userId, UserUpdateReq req);

        Task<string> UploadAvatarAsync(Guid userId, IFormFile file);

        Task DeleteAvatarAsync(Guid pulicId);

        Task CheckDupEmailAsync(string email);

        #region document

        Task<CitizenIdentityRes> UploadCitizenIdAsync(Guid userId, IFormFile file);

        Task<DriverLicenseRes> UploadDriverLicenseAsync(Guid userId, IFormFile file);

        Task<CitizenIdentityRes?> GetMyCitizenIdentityAsync(Guid userId);

        Task<DriverLicenseRes?> GetMyDriverLicenseAsync(Guid userId);

        Task<CitizenIdentityRes> UpdateCitizenIdentityAsync(Guid userId, UpdateCitizenIdentityReq req);

        Task<DriverLicenseRes> UpdateDriverLicenseAsync(Guid userId, UpdateDriverLicenseReq req);

        Task DeleteDriverLicenseAsync(Guid userId);

        Task DeleteCitizenIdentityAsync(Guid userId);

        #endregion

        #endregion

        // ===========================
        // ===== User Management =====
        // ===========================
        #region user-management
        Task<Guid> CreateAsync(CreateUserReq req);

        Task<IEnumerable<UserProfileViewRes>> GetAllAsync(string? phone, string? citizenIdNumber, string? driverLicenseNumber);

        Task<User?> GetByIdAsync(Guid id);

        Task<UserProfileViewRes> GetByPhoneAsync(string phone);

        Task<UserProfileViewRes> GetByCitizenIdentityAsync(string idNumber);

        Task<UserProfileViewRes> GetByDriverLicenseAsync(string number);

        #endregion
    }
}