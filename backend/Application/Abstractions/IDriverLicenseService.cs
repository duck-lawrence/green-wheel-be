using Domain.Entities;

namespace Application.Abstractions
{
    public interface IDriverLicenseService
    {
        Task<DriverLicense?> ProcessDriverLicenseAsync(Guid userId, string imageUrl);

        Task<DriverLicense?> AddAsync(DriverLicense license);

        Task<DriverLicense?> UpdateAsync(DriverLicense license);

        Task<bool> DeleteAsync(Guid userId);

        Task<DriverLicense?> GetByUserIdAsync(Guid userId);

        Task<DriverLicense?> GetAsync(Guid id);

        Task<DriverLicense?> GetByLicenseNumberAsync(string licenseNumber);
    }
}