using Application.Dtos.Vehicle.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Dispatch.Response
{
    public class DispatchRequestVehicleRes
    {
        public DateTimeOffset CreatedAt { get; init; }
        public VehicleViewRes Vehicle { get; init; } = default!;
    }
}
