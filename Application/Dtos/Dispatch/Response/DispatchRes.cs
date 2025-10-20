using Application.Constants;

namespace Application.Dtos.Dispatch.Response
{
    public class DispatchRes
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public string FromStationName { get; init; }
        public string ToStationName { get; init; }
        public DispatchRequestStatus Status { get; init; }
        public string RequestAdminName { get; init; }
        public string? ApprovedAdminName { get; init; }
        public IEnumerable<string> StaffNames { get; init; } = [];
        public IEnumerable<string> VehiclePlates { get; init; } = [];
    }
}