using Application.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<Vehicle> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Vehicle>?> GetVehicles(Guid stationId, Guid modelId);
        Task<IEnumerable<VehicleComponent>> GetVehicleComponentsAsync(Guid vehicleId);
        Task<Vehicle?> GetByIdOptionAsync(Guid id, bool includeModel = false);
    }
}
