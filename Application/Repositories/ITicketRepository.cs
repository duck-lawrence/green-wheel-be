using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Ticket.Request;
using Domain.Entities;

namespace Application.Repositories
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<PageResult<Ticket>> GetAllAsync(TicketFilterParams filter, PaginationParams pagination);

        Task<PageResult<Ticket>> GetByCustomerAsync(Guid customerId, int? status, PaginationParams pagination);

        Task<PageResult<Ticket>> GetEscalatedAsync(PaginationParams pagination);
    }
}