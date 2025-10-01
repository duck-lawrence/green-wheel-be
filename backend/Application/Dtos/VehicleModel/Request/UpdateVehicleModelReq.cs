using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleModel.Request
{
    public class UpdateVehicleModelReq
    {
        public string? Name { get; set; }

        public string? Description { get; set; } 

        public decimal? CostPerDay { get; set; }

        public decimal? DepositFee { get; set; }

        public int? SeatingCapacity { get; set; }

        public int? NumberOfAirbags { get; set; }

        public decimal? MotorPower { get; set; }

        public decimal? BatteryCapacity { get; set; }

        public decimal? EcoRangeKm { get; set; }

        public decimal? SportRangeKm { get; set; }

        public Guid? BrandId { get; set; }

        public Guid? SegmentId { get; set; }
    }
}
