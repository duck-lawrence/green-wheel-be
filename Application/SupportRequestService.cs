using Application.Abstractions;
using Application.AppExceptions;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.UserSupport.Response;
using Application.Dtos.UserSupport.Request;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class SupportRequestService : ISupportRequestService
    {
        private readonly ISupportRequestRepository _repo;
        private readonly IMapper _mapper;

        public SupportRequestService(ISupportRequestRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(Guid customerId, CreateSupportReq req)
        {
            var entity = new Ticket
            {
                Id = Guid.NewGuid(),
                Title = req.Title,
                Description = req.Description,
                Type = req.Type,
                Status = 0,
                RequesterId = customerId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _repo.AddAsync(entity);
            return entity.Id;
        }

        public async Task<IEnumerable<SupportRes>> GetByCustomerAsync(Guid customerId)
        {
            var list = await _repo.GetByCustomerAsync(customerId);
            return _mapper.Map<IEnumerable<SupportRes>>(list);
        }

        public async Task<PageResult<SupportRes>> GetAllAsync(PaginationParams pagination)
        {
            var page = await _repo.GetAllAsync(pagination);
            var mapped = _mapper.Map<IEnumerable<SupportRes>>(page.Items);
            return new PageResult<SupportRes>(mapped, page.PageNumber, page.PageSize, page.TotalCount);
        }

        public async Task UpdateAsync(Guid id, UpdateSupportReq req, Guid staffId)
        {
            var entity = await _repo.GetByIdAsync(id)
                ?? throw new NotFoundException("Support request not found");

            entity.Reply = req.Reply ?? entity.Reply;
            entity.Status = req.Status ?? entity.Status;
            entity.AssigneeId = staffId;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _repo.UpdateAsync(entity);
        }
    }
}