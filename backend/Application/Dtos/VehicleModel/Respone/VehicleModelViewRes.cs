using Domain.Entities;

namespace Application.Dtos.VehicleModel.Respone
{
    public class VehicleModelViewRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal CostPerDay { get; set; }

        public decimal DepositFee { get; set; }

        public int SeatingCapacity { get; set; }

        public int NumberOfAirbags { get; set; }

        public decimal MotorPower { get; set; }

        public decimal BatteryCapacity { get; set; }

        public decimal EcoRangeKm { get; set; }

        public decimal SportRangeKm { get; set; }

        public Brand Brand { get; set; } = null!;
        public IEnumerable<string>? ImageUrls { get; set; } 
        public Domain.Entities.VehicleSegment Segment { get; set; } = null!;
        public int AvailableVehicleCount { get; set; }
    }
}
