using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRentalContractRepository : IGenericRepository<RentalContract>
    {
        Task<IEnumerable<RentalContract>> GetByCustomerAsync(Guid customerId, int? status = null);
        Task<bool> HasActiveContractAsync(Guid customerId);
        Task<IEnumerable<RentalContract>> GetAllAsync(int? status = null, string? phone = null,
            string? citizenIdentity = null, string? driverLicense = null);
        Task<RentalContract?> GetByCheckListIdAsync(Guid id);

    }
}
