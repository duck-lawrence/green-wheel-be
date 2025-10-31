using Application.Dtos.VehicleSegment.Request;
using Application.Dtos.VehicleSegment.Respone;

namespace Application.Abstractions
{
    public interface IVehicleSegmentService
    {
        Task<IEnumerable<VehicleSegmentViewRes>> GetAllVehicleSegment();
        Task<Guid> CreateAsync(CreateSegmentReq req);
        Task UpdateAsync(Guid id, UpdateSegmentReq req);
        Task DeleteAsync(Guid id);
    }
}
