using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IModelComponentRepository : IGenericRepository<ModelComponent>
    {
        Task DeleteRangeAsync(IEnumerable<ModelComponent> items);
        Task<IEnumerable<ModelComponent>> GetByModelIdAsync(Guid ModelId);
    }
}
