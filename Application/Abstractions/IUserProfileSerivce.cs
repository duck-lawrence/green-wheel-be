using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.CitizenIdentity.Response;
using Application.Dtos.Common.Request;
using Application.Dtos.DriverLicense.Request;
using Application.Dtos.DriverLicense.Response;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IUserProfileSerivce
    {
        Task<UserProfileViewRes> GetMeAsync(ClaimsPrincipal userClaims);

        Task UpdateAsync(Guid userId, UserUpdateReq req);

        Task UpdateBankAccountAsync(Guid userId, UpdateBankAccountReq req);

        Task DeleteBankAccountAsync(Guid userId);

        Task<string> UploadAvatarAsync(Guid userId, IFormFile file);

        Task DeleteAvatarAsync(Guid pulicId);

        Task CheckDupEmailAsync(string email);

        Task<CitizenIdentityRes> UploadCitizenIdAsync(Guid userId, UploadImagesReq req);

        Task<DriverLicenseRes> UploadDriverLicenseAsync(Guid userId, IFormFile file);

        Task<CitizenIdentityRes?> GetMyCitizenIdentityAsync(Guid userId);

        Task<DriverLicenseRes?> GetMyDriverLicenseAsync(Guid userId);

        Task<CitizenIdentityRes> UpdateCitizenIdentityAsync(Guid userId, UpdateCitizenIdentityReq req);

        Task<DriverLicenseRes> UpdateDriverLicenseAsync(Guid userId, UpdateDriverLicenseReq req);

        Task DeleteDriverLicenseAsync(Guid userId);

        Task DeleteCitizenIdentityAsync(Guid userId);
    }
}