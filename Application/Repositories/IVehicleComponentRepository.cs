using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IVehicleComponentRepository : IGenericRepository<VehicleComponent>
    {
        Task<IEnumerable<VehicleComponent>> GetByVehicleIdAsync(Guid vehicleId); 
        Task<IEnumerable<VehicleComponent>> GetAllAsync(Guid? modelId);
    }
}
