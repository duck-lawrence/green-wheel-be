﻿using Application.Constants;
using Application.Dtos.RentalContract.Respone;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Abstractions
{
    public interface IUserService
    {
        Task<string> RegisterAsync(string token, UserRegisterReq userRegisterReq);

        Task<string?> Login(UserLoginReq user);

        Task<int> Logout(string refreshToken);

        //Task<User> DeleteUserAsync(Guid id);
        //Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, object>>? include = null);
        //Task<int> UpdateUserAsync(User user);

        Task<User?> GetUserByIdAsync(Guid id);

        //Task<User> GetUserByEmail(string email);
        string GenerateAccessToken(Guid userId);

        Task<string> GenerateRefreshToken(Guid userId, ClaimsPrincipal? oldClaims);

        Task<string> RefreshToken(string refreshToken, bool getRevoked);

        Task<string> VerifyOTP(VerifyOTPReq verifyOTPDto, TokenType type, string cookiesKey);

        Task SendOTP(string email);

        Task ChangePassword(ClaimsPrincipal userClaims, UserChangePasswordReq userChangePasswordDto);

        Task ResetPassword(string forgotPasswordToken, string password);

        Task<Dictionary<string, string>> LoginWithGoogle(string email);

        //Task<string> SetPassword(string setPasswordToken,string password, string firstName, string lastName);
        //Task<UserProfileViewRes> GetMe(ClaimsPrincipal userClaims);
        //Task UpdateMe(ClaimsPrincipal userClaims, UserUpdateReq userUpdateReq);

        Task<string> SetPasswordAsync(string setPasswordToken, GoogleSetPasswordReq req);

        Task<UserProfileViewRes> GetMeAsync(ClaimsPrincipal userClaims);

        Task UpdateMeAsync(ClaimsPrincipal userClaims, UserUpdateReq userUpdateReq);

        Task<string> UploadAvatarAsync(Guid userId, IFormFile file);

        Task DeleteAvatarAsync(Guid pulicId);

        Task CheckDupEmailAsync(string email);

        Task<object> UploadCitizenIdAsync(Guid userId, IFormFile file);

        Task<object> UploadDriverLicenseAsync(Guid userId, IFormFile file);

        Task<object?> GetMyCitizenIdentityAsync(Guid userId);

        Task<object?> GetMyDriverLicenseAsync(Guid userId);
        Task<Guid> CreateAnounymousAccount(CreateUserReq req);
        Task<UserProfileViewRes> GetUserByPhoneAsync(string phone);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<UserProfileViewRes> GetByCitizenIdentityAsync(string idNumber);
        Task<UserProfileViewRes> GetByDriverLicenseAsync(string number);
        
    }
}