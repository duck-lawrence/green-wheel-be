using Application.Dtos.Common.Request;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Application.Helpers;

namespace Infrastructure.Repositories
{
    public class SupportRequestRepository : GenericRepository<SupportRequest>, ISupportRequestRepository
    {
        public SupportRequestRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PageResult<SupportRequest>> GetAllAsync(PaginationParams pagination)
        {
            var query = _dbContext.SupportRequests
                .Include(s => s.Customer)
                .Include(s => s.Staff)
                .OrderByDescending(s => s.CreatedAt);

            var total = await query.CountAsync();
            var items = await query.ApplyPagination(pagination).ToListAsync();

            return new PageResult<SupportRequest>(items, pagination.PageNumber, pagination.PageSize, total);
        }

        public async Task<IEnumerable<SupportRequest>> GetByCustomerAsync(Guid customerId)
        {
            return await _dbContext.SupportRequests
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}