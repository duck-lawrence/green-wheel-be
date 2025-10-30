using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Helpers;
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
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            //lấy ra list linh kiện của xe
            return components;
        }
        public async Task<PageResult<VehicleComponent>> GetAllAsync(Guid? modelId, PaginationParams pagination)
        {
            var query = _dbContext.VehicleComponents
                                    .Include(vc => vc.ModelComponents)
                                    .AsQueryable();
                                    //.ToListAsync();

            if (modelId != null)
            {
                query = query
                        .Where(vc => vc.ModelComponents
                            .Any(mc => mc.ModelId == modelId))
                        .OrderBy(c => c.CreatedAt);
            }
            var totalCount = await query.CountAsync();
            var components = await query
                            .ApplyPagination(pagination)
                            .ToListAsync();
            return new PageResult<VehicleComponent>(components, pagination.PageNumber, pagination.PageSize, totalCount);
        }
    }
}
