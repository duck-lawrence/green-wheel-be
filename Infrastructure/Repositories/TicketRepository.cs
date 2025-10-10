using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Application.Helpers;

namespace Infrastructure.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PageResult<Ticket>> GetAllAsync(PaginationParams pagination)
        {
            var query = _dbContext.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Assignee)
                .OrderByDescending(t => t.CreatedAt);

            var total = await query.CountAsync();
            var items = await query.ApplyPagination(pagination).ToListAsync();

            return new PageResult<Ticket>(items, pagination.PageNumber, pagination.PageSize, total);
        }

        public async Task<IEnumerable<Ticket>> GetByCustomerAsync(Guid customerId)
        {
            return await _dbContext.Tickets
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Assignee)
                .ToListAsync();
        }
    }
}