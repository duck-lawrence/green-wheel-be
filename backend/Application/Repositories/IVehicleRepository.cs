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
        Task<Vehicle> GetVehicle(Guid stationId, Guid modelId,
            DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
