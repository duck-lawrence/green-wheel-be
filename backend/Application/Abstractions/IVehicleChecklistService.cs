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
        Task<VehicleChecklistViewRes> CreateVehicleChecklistAsync(ClaimsPrincipal userclaims, CreateVehicleChecklistReq req);
    }
}
