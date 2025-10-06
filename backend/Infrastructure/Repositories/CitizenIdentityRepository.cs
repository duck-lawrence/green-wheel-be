using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CitizenIdentityRepository : GenericRepository<CitizenIdentity>, ICitizenIdentityRepository
    {
        private readonly IGreenWheelDbContext _dbContext;

        public CitizenIdentityRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CitizenIdentity> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.CitizenIdentities.FirstOrDefaultAsync(x => x.UserId == userId && x.DeletedAt == null);
        }

        public async Task<CitizenIdentity> GetIdNumberAsync(string idNumber)
        {
            return await _dbContext.CitizenIdentities
                        .Include(u => u.User).FirstOrDefaultAsync(x => x.Number == idNumber && x.DeletedAt == null);
        }
    }
}