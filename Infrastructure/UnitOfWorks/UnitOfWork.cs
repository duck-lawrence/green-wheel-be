using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfwork : IUnitOfwork
    {
        protected readonly IGreenWheelDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfwork(IGreenWheelDbContext context)
        {
            _context = context;
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction ??= await ((GreenWheelDbContext)_context)
                        .Database.BeginTransactionAsync(cancellationToken);

        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
    }
}
