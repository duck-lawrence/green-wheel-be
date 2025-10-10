using Application.Dtos.VehicleModel.Respone;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IVehicleModelRepository : IGenericRepository<VehicleModel>
    {
        Task<IEnumerable<VehicleModelViewRes>> FilterVehicleModelsAsync(Guid stationId,
                                                            DateTimeOffset startDate,
                                                            DateTimeOffset endDate,
                                                            Guid? segmentId = null);
        Task<VehicleModelViewRes> GetByIdAsync(Guid vehicleId, Guid stationId, DateTimeOffset startDate,
        DateTimeOffset endDate);
    }
}
