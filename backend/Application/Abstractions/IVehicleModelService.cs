using Application.Dtos.VehicleModel.Request;
using Application.Dtos.VehicleModel.Respone;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IVehicleModelService
    {
        Task<Guid> CreateVehicleModelAsync(CreateVehicleModelReq createVehicleModelReq);
        Task<IEnumerable<VehicleModelViewRes>> GetAllVehicleModels(VehicleFilterReq vehicleFilterReq);
        Task<int> UpdateVehicleModelAsync(Guid Id, UpdateVehicleModelReq updateVehicleModelReq);
        Task<bool> DeleteVehicleModleAsync(Guid id);
        Task<VehicleModelViewRes> GetByIdAsync(Guid id, Guid stationId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
