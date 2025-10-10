using Application.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.UnitOfWorks
{
    public interface IModelImageUow : IDisposable
    {
        IModelImageRepository ModelImageRepository { get; }
        IVehicleModelRepository VehicleModelRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}