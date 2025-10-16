using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DispatchRepository : GenericRepository<DispatchRequest>, IDispatchRepository
    {
        private readonly IGreenWheelDbContext _ctx;

        public DispatchRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
            _ctx = dbContext;
        }

        public async Task<IEnumerable<DispatchRequest>> GetAllExpandedAsync(Guid? fromStationId, Guid? toStationId, int? status)
        {
            var query = _ctx.DispatchRequests
                .Include(x => x.FromStationId)
                .Include(x => x.ToStationId)
                .Include(x => x.RequestAdmin).ThenInclude(s => s.User)
                .AsQueryable();

            if (typeof(Domain.Commons.SorfDeletedEntity).IsAssignableFrom(typeof(DispatchRequest)))
                query = query.Where(x => x.DeletedAt == null);

            if (fromStationId.HasValue && toStationId.HasValue)
                query = query.Where(x => x.FromStationId == fromStationId.Value || x.ToStationId == toStationId.Value);
            else if (fromStationId.HasValue)
                query = query.Where(x => x.FromStationId == fromStationId.Value);
            else if (toStationId.HasValue)
                query = query.Where(x => x.ToStationId == toStationId.Value);
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);
            return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<DispatchRequest?> GetByIdWithFullInfoAsync(Guid id)
        {
            return await _dbContext.DispatchRequests
                .Include(x => x.FromStation)
                .Include(x => x.ToStation)
                .Include(x => x.RequestAdmin).ThenInclude(r => r.User)
                .Include(x => x.ApprovedAdmin).ThenInclude(a => a.User)
                .Include(x => x.DispatchRequestStaffs).ThenInclude(rs => rs.Staff).ThenInclude(s => s.User)
                .Include(x => x.DispatchRequestVehicles).ThenInclude(rv => rv.Vehicle)
                .FirstOrDefaultAsync(x => x.Id == id && (x.DeletedAt == null));
        }
    }
}