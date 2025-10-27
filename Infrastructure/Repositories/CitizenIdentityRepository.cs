using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CitizenIdentityRepository : GenericRepository<CitizenIdentity>, ICitizenIdentityRepository
    {

        public CitizenIdentityRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CitizenIdentity?> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.CitizenIdentities.FirstOrDefaultAsync(x => x.UserId == userId && x.DeletedAt == null);
        }

        public async Task<CitizenIdentity?> GetByIdNumberAsync(string idNumber)
        {
            return await _dbContext.CitizenIdentities
                        .Include(u => u.User).FirstOrDefaultAsync(x => x.Number == idNumber && x.DeletedAt == null);
        }
    }
}