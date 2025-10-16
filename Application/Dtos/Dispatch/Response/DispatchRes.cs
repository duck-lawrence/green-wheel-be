using Application.Constants;

namespace Application.Dtos.Dispatch.Response
{
    public class DispatchRes
    {
        public Guid Id { get; init; }
        public string? Description { get; init; }
        public Guid FromStationId { get; init; }
        public Guid ToStationId { get; init; }
        public string FromStationName { get; init; } = default!;
        public string ToStationName { get; init; } = default!;
        public DispatchRequestStatus Status { get; init; }
        public Guid RequestAdminId { get; init; }
        public string RequestAdminName { get; init; } = default!;
        public DateTimeOffset CreatedAt { get; init; }
    }
}