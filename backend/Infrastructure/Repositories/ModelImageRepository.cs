using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ModelImageRepository : GenericRepository<ModelImage>, IModelImageRepository
    {
        public ModelImageRepository(IGreenWheelDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ModelImage>> GetByModelAndIdsAsync(Guid modelId, IEnumerable<Guid> ids)
        {
            return await _dbContext.ModelImages
                .Where(x => x.ModelId == modelId && ids.Contains(x.Id) && x.DeletedAt == null)
                .ToListAsync();
        }
    }
}