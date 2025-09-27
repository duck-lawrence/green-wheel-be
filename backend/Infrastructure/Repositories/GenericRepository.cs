using Application.Repositories;
using Domain.Commons;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : class,IEntity
    {
        protected readonly IGreenWheelDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(IGreenWheelDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }


        public async Task<Guid> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entityFromDb = await GetByIdAsync(id)
        ?? throw new Exception($"{typeof(T).Name} is not found");

            if (entityFromDb is SorfDeletedEntity softEntity && softEntity.DeletedAt == null)
            {
                // Gán DeletedAt thông qua softEntity (compiler hiểu type rồi)
                softEntity.DeletedAt = DateTime.UtcNow;
                //chỉ cần chỉnh hoai, savechange tự update
            }
            else
            {
                _dbSet.Remove(entityFromDb);
            }
            await _dbContext.SaveChangesAsync();
            return entityFromDb != null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, object>>? include = null)
        {
            var query = _dbSet.AsQueryable();
            if(include != null)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);   
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var entityFromDb = await GetByIdAsync(entity.Id);
            if(entityFromDb == null)
            {
                throw new Exception($"{typeof(T).Name} is not found");
            }
            _dbContext.Entry(entityFromDb).CurrentValues.SetValues(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
