using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWorks
{
    public class MediaUow : IMediaUow
    {
        private readonly IGreenWheelDbContext _context;

        public IUserRepository Users { get; }
        public ICitizenIdentityRepository CitizenIdentities { get; }
        public IDriverLicenseRepository DriverLicenses { get; }

        public MediaUow(
            IGreenWheelDbContext context,
            IUserRepository users,
            ICitizenIdentityRepository citizenIdentities,
            IDriverLicenseRepository driverLicenses)
        {
            _context = context;
            Users = users;
            CitizenIdentities = citizenIdentities;
            DriverLicenses = driverLicenses;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            // IGreenWheelDbContext kế thừa DbContext nên cast an toàn
            var db = (DbContext)_context;
            return await db.Database.BeginTransactionAsync(ct);
        }

        public void Dispose()
        {
            if (_context is IDisposable d)
                d.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}