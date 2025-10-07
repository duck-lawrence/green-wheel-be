using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.DriverLicense.Request;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IGeminiService
    {
        Task<CitizenIdentityDto?> ExtractCitizenIdAsync(string imageUrl);

        Task<DriverLicenseDto?> ExtractDriverLicenseAsync(string imageUrl);
    }
}