using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Ticket.Request;
using Application.Helpers;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PageResult<Ticket>> GetAllAsync(TicketFilterParams filter, PaginationParams pagination)
        {
            var query = _dbContext.Tickets
                .Include(t => t.Requester) // include User
                .Include(t => t.Assignee)  // include Staff
                .OrderByDescending(t => t.CreatedAt)
                .AsQueryable();

            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status);
            }
            if (filter.Type.HasValue)
            {
                query = query.Where(t => t.Type == filter.Type);
            }

            var total = await query.CountAsync();
            var items = await query.ApplyPagination(pagination).ToListAsync();

            return new PageResult<Ticket>(items, pagination.PageNumber, pagination.PageSize, total);
        }

        public async Task<PageResult<Ticket>> GetByCustomerAsync(Guid customerId, int? status, PaginationParams pagination)
        {
            var query = _dbContext.Tickets
                .Where(x => x.RequesterId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Assignee)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status);
            }

            var total = await query.CountAsync();
            var items = await query.ApplyPagination(pagination).ToListAsync();

            return new PageResult<Ticket>(items, pagination.PageNumber, pagination.PageSize, total);
        }

        public async Task<PageResult<Ticket>> GetEscalatedAsync(PaginationParams pagination)
        {
            var query = _dbContext.Tickets
                .Where(t => t.Status == (int)TicketStatus.EscalatedToAdmin)
                .Include(t => t.Requester)
                .Include(t => t.Assignee)
                .OrderByDescending(x => x.CreatedAt);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PageResult<Ticket>(items, pagination.PageNumber, pagination.PageSize, total);
        }
    }
}