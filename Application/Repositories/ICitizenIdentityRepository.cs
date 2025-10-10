using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ICitizenIdentityRepository : IGenericRepository<CitizenIdentity>
    {
        Task<CitizenIdentity> GetByIdNumberAsync(string idNumber);

        Task<CitizenIdentity> GetByUserIdAsync(Guid userId);
    }
}