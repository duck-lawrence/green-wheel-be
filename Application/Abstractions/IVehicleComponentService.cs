using Application.Dtos.VehicleComponent.Request;
using Application.Dtos.VehicleComponent.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IVehicleComponentService
    {
        Task DeleteAsync(Guid id);
        Task<Guid> AddAsync(CreateVehicleComponentReq req);
        Task UpdateAsync(Guid id, UpdateVehicleComponentReq req);
        Task<IEnumerable<VehicleComponentViewRes>> GetAllAsync(Guid? id);
        Task<VehicleComponentViewRes> GetByIdAsync(Guid id);
    }
}
