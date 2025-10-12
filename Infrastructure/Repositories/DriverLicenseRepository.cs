using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DriverLicenseRepository : GenericRepository<DriverLicense>, IDriverLicenseRepository
    {
        private readonly GreenWheelDbContext _context;

        public DriverLicenseRepository(GreenWheelDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DriverLicense?> GetByUserIdAsync(Guid userId)
        {
            return await _context.DriverLicenses
                .FirstOrDefaultAsync(x => x.UserId == userId && x.DeletedAt == null);
        }

        public async Task<DriverLicense?> GetByLicenseNumber(string licenseNumber)
        {
            return await _context.DriverLicenses
                .Include(u => u.User)
                .FirstOrDefaultAsync(x => x.Number == licenseNumber && x.DeletedAt == null);
        }
    }
}