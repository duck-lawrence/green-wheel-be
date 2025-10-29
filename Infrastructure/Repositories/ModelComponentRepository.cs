using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ModelComponentRepository : GenericRepository<ModelComponent>, IModelComponentRepository
    {
        public ModelComponentRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteRangeAsync(IEnumerable<VehicleComponent> components)
        {
            _dbContext.VehicleComponents.RemoveRange(components);
            await _dbContext.SaveChangesAsync();
        }
    }
}
