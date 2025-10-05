using Application.Dtos.VehicleSegment.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IVehicleSegmentService
    {
        Task<IEnumerable<VehicleSegmentViewRes>> GetAllVehicleSegment();
    }
}
