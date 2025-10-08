using Application.Dtos.Common.Request;
using Domain.Entities;

namespace Application.Repositories
{
    public interface ISupportRequestRepository : IGenericRepository<SupportRequest>
    {
        Task<PageResult<SupportRequest>> GetAllAsync(PaginationParams pagination);

        Task<IEnumerable<SupportRequest>> GetByCustomerAsync(Guid customerId);
    }
}