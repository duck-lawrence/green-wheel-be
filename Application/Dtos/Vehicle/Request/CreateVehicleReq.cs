namespace Application.Dtos.Vehicle.Request
{
    public class CreateVehicleReq
    {
        public string LicensePlate { get; set; } = null!;

        public Guid ModelId { get; set; }

        public Guid StationId { get; set; }
    }
}