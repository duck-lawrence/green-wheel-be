namespace Application.Dtos.VehicleModel.Request
{
    public class DeleteModelImagesReq
    {
        public List<Guid> ImageIds { get; set; } = new();
    }
}