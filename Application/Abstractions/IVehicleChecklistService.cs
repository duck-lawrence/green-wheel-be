using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklist.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IVehicleChecklistService
    {
        Task<Guid> Create(ClaimsPrincipal userclaims, CreateVehicleChecklistReq req);
        Task<VehicleChecklistViewRes> GetByIdAsync(Guid id, ClaimsPrincipal userClaims);
        Task<IEnumerable<VehicleChecklistViewRes>>GetAll(Guid? contractId, int? type, ClaimsPrincipal userClaims);
        Task UpdateAsync(UpdateVehicleChecklistReq req, Guid id);
        Task UpdateItemsAsync(Guid id, int status, string? notes);
        Task CustomerSignVehicleChecklistAsync(Guid id, ClaimsPrincipal user);
    }
}
