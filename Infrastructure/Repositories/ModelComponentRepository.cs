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
    public class ModelComponentRepository : GenericRepository<ModelComponent>, IModelComponentRepository
    {
        public ModelComponentRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteRangeAsync(IEnumerable<ModelComponent> items)
        {
            _dbContext.ModelComponents.RemoveRange(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ModelComponent>> GetByModelIdAsync(Guid ModelId)
        {
            var modelComponents = await _dbContext.ModelComponents.ToListAsync();
            return modelComponents.Where(m => m.ModelId == ModelId) ?? [];
        }
    }
}
