using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class VehicleChecklistUow : IVehicleChecklistUow
    {
        private readonly IGreenWheelDbContext _context;
        public IVehicleChecklistItemRepository VehicleChecklistItemRepository { get; }
        public IVehicleCheckListRepository VehicleChecklistRepository { get; }
        public IVehicleRepository VehicleRepository { get; }

        public VehicleChecklistUow(IGreenWheelDbContext context,
            IVehicleChecklistItemRepository vehicleChecklistItemRepository,
            IVehicleCheckListRepository vehicleCheckListRepository,
            IVehicleRepository vehicleRepository)
        {
            _context = context;
            VehicleChecklistItemRepository = vehicleChecklistItemRepository;
            VehicleChecklistRepository = vehicleCheckListRepository;
            VehicleRepository = vehicleRepository;
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