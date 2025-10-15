using Domain.Entities;

namespace Application.Repositories
{
    public interface IDriverLicenseRepository : IGenericRepository<DriverLicense>
    {
        Task<DriverLicense?> GetByUserIdAsync(Guid userId);

        Task<DriverLicense?> GetByLicenseNumber(string licenseNumber);
    }
}