using Application.Dtos.Common.Request;
using Application.Dtos.UserSupport.Request;
using Application.Dtos.UserSupport.Response;

namespace Application.Abstractions
{
    public interface ISupportRequestService
    {
        Task<Guid> CreateAsync(Guid customerId, CreateSupportReq req);

        Task<PageResult<SupportRes>> GetAllAsync(PaginationParams pagination);

        Task<IEnumerable<SupportRes>> GetByCustomerAsync(Guid customerId);

        Task UpdateAsync(Guid id, UpdateSupportReq req, Guid staffId);
    }
}