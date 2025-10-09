using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWorks
{
    public class ModelImageUow : IModelImageUow
    {
        private readonly IGreenWheelDbContext _context;

        public IModelImageRepository ModelImageRepository { get; }
        public IVehicleModelRepository VehicleModelRepository { get; }

        public ModelImageUow(
            IGreenWheelDbContext context,
            IModelImageRepository modelImageRepository,
            IVehicleModelRepository vehicleModelRepository)
        {
            _context = context;
            ModelImageRepository = modelImageRepository;
            VehicleModelRepository = vehicleModelRepository;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            var db = (DbContext)_context;
            return await db.Database.BeginTransactionAsync(ct);
        }

        public void Dispose()
        {
            if (_context is IDisposable disposable)
                disposable.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}