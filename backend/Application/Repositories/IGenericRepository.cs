using Domain.Commons;
using System.Linq.Expressions;

namespace Application.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<Guid> AddAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[]? includes = null);
        Task<int> UpdateAsync(T entity);

        Task<T?> GetByIdAsync(Guid id);

    }
}
