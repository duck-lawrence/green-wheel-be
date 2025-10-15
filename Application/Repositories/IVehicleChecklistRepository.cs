using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklist.Respone;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IVehicleCheckListRepository : IGenericRepository<VehicleChecklist>
    {
        Task<IEnumerable<VehicleChecklist>?> GetAll(Guid? contractId, int? type);
    }
}
