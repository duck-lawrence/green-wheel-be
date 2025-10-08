using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWorks
{
    public class ModelImageUow : IModelImageUow
    {
        private readonly IGreenWheelDbContext _context;
        public IModelImageRepository ModelImageRepository { get; }
        public IVehicleModelRepository VehicleModelRepository { get; }

        public ModelImageUow(IGreenWheelDbContext context, IModelImageRepository modelImageRepository)
        {
            _context = context;
            ModelImageRepository = modelImageRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (_context is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}