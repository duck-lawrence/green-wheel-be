using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        protected readonly IGreenWheelDbContext _dbContext;

        public StaffRepository(GreenWheelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountStaffsInStationAsync(Guid[] staffIds, Guid stationId)
        {
            return await _dbContext.Staffs.CountAsync(s => staffIds.Contains(s.UserId) && s.StationId == stationId);
        }

        public async Task<Staff?> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Staffs.FirstOrDefaultAsync(s => s.UserId == userId && s.DeletedAt == null);
        }

        public async Task UpdateStationForDispatchAsync(Guid dispatchId, Guid stationId)
        {
            var staffId = await _dbContext.DispatchRequestStaffs.Where(x => x.DispatchRequestId == dispatchId).Select(x => x.StaffId).ToListAsync();

            if (staffId != null || staffId.Count == 0) { return; }
            var staffs = await _dbContext.Staffs.Where(s => staffId.Contains(s.StationId)).ToListAsync();
            foreach (var x in staffs)
            {
                x.StationId = stationId;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}