namespace Application.Dtos.Vehicle.Request
{
    public class DeleteVehicleImagesReq
    {
        public List<Guid> ImageIds { get; set; } = new();
    }
}