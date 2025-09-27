using Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<Guid> AddAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? include = null);
        Task<int> UpdateAsync(T entity);

        Task<T?> GetByIdAsync(Guid id);

    }
}
