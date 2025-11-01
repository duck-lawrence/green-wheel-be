using Domain.Entities;

namespace Application.Dtos.VehicleModel.Request
{
    public class CreateVehicleModelReq
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; } = null!;

        public decimal CostPerDay { get; set; }

        public decimal DepositFee { get; set; }

        public int SeatingCapacity { get; set; }

        public int NumberOfAirbags { get; set; }

        public decimal MotorPower { get; set; }

        public decimal BatteryCapacity { get; set; }

        public decimal EcoRangeKm { get; set; }

        public decimal SportRangeKm { get; set; }
        public Guid BrandId { get; set; }
        public Guid SegmentId { get; set; }
        public IEnumerable<Guid> ComponentIds { get; set; } = [];
    }
}
