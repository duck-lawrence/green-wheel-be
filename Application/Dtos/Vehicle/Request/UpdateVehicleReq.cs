using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Vehicle.Request
{
    public class UpdateVehicleReq
    {
        public string? LicensePlate { get; set; } = null!;

        public int? Status { get; set; }

        public Guid? ModelId { get; set; }

        public Guid? StationId { get; set; }
    }
}
