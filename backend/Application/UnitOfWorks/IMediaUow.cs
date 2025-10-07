using Application.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.UnitOfWorks
{
    public interface IMediaUow : IDisposable
    {
        IUserRepository Users { get; }
        ICitizenIdentityRepository CitizenIdentities { get; }
        IDriverLicenseRepository DriverLicenses { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    }
}