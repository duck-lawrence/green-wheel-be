using Domain.Entities;
using System.Numerics;

namespace Application.Repositories
{
    public interface IModelImageRepository : IGenericRepository<ModelImage>
    {
        Task<IEnumerable<ModelImage>> GetByModelAndIdsAsync(Guid modelId, IEnumerable<Guid> ids);
    }
}