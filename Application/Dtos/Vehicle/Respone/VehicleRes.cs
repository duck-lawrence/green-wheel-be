using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Vehicle.Respone
{
    public class VehicleRes
    {
        public Guid Id { get; init; }
        public string LicensePlate { get; init; } = default!;
        public VehicleModelRes Model { get; init; } = default!;
        public int Status { get; init; }
    }
}
