using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Application.Helpers;

namespace Infrastructure.Repositories
{
    public class SupportRequestRepository : GenericRepository<Ticket>, ISupportRequestRepository
    {
        public SupportRequestRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PageResult<Ticket>> GetAllAsync(PaginationParams pagination)
        {
            var query = _dbContext.Tickets
                .Include(s => s.Requester)
                .Include(s => s.Assignee)
                .OrderByDescending(s => s.CreatedAt);

            var total = await query.CountAsync();
            var items = await query.ApplyPagination(pagination).ToListAsync();

            return new PageResult<Ticket>(items, pagination.PageNumber, pagination.PageSize, total);
        }

        public async Task<IEnumerable<Ticket>> GetByCustomerAsync(Guid customerId)
        {
            return await _dbContext.Tickets
                .Where(x => x.RequesterId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}