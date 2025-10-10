using Domain.Entities;

namespace Application.Repositories
{
    public interface IDriverLicenseRepository : IGenericRepository<DriverLicense>
    {
        Task<DriverLicense?> GetByUserId(Guid userId);

        Task<DriverLicense?> GetByLicenseNumber(string licenseNumber);
    }
}