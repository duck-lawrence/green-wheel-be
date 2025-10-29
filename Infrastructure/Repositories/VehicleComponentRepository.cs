using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VehicleComponentRepository : GenericRepository<VehicleComponent>, IVehicleComponentRepository
    {
        public VehicleComponentRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<VehicleComponent>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var components = await _dbContext.Vehicles
                .Where(v => v.Id == vehicleId)
                .SelectMany(v => v.Model.ModelComponents.Select(mc => mc.Component))
                .ToListAsync();
            //lấy ra list linh kiện của xe
            return components;
        }
        public async Task<IEnumerable<VehicleComponent>> GetAllAsync(Guid? modelId)
        {
            var components = await _dbContext.VehicleComponents
                                    .Include(vc => vc.ModelComponents)
                                    .ToListAsync();
            if (modelId != null)
            {
                components = components
                                .Where(vc => vc.ModelComponents
                                    .Any(mc => mc.ModelId == modelId))
                                .ToList();
            }
            return components;
        }
    }
}
