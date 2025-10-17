using Application.Constants;

namespace Application.Dtos.Dispatch.Request
{
    public sealed class CreateDispatchReq
    {
        public string? Description { get; set; }
        public Guid ToStationId { get; set; }
        public Guid[]? staffIds { get; set; }
        public Guid[]? vehicleIds { get; set; }
    }

    public sealed class UpdateDispatchReq
    {
        public int status { get; set; }
    }
}