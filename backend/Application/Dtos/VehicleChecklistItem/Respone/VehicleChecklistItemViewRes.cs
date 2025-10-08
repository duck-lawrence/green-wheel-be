using Application.Dtos.VehicleComponent.Respone;

namespace Application.Dtos.VehicleChecklistItem.Respone
{
    public class VehicleChecklistItemViewRes
    {
        public Guid Id { get; set; }
        public string? Notes { get; set; }

        public int Status { get; set; }
        public string? ImageUrl { get; set; }
        public VehicleComponentViewRes Component { get; set; }
    }
}
