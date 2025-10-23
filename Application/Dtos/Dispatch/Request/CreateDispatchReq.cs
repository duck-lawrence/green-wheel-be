using Application.Constants;

namespace Application.Dtos.Dispatch.Request
{
    public sealed class CreateDispatchReq
    {
        public string? Description { get; set; }
        public Guid FromStationId { get; set; }
        public Guid[]? StaffIds { get; set; }
        public Guid[]? VehicleIds { get; set; }
    }
} 