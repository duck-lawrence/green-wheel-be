using Application.Dtos.VehicleModel.Respone;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IVehicleModelRepository : IGenericRepository<Domain.Entities.VehicleModel>
    {
        Task<IEnumerable<Dtos.VehicleModel.Respone.VehicleModelViewRes>> FilterVehicleModelsAsync(Guid stationId,
                                                            DateTimeOffset startDate,
                                                            DateTimeOffset endDate,
                                                            Guid? segmentId = null);
        Task<Dtos.VehicleModel.Respone.VehicleModelViewRes> GetByIdAsync(Guid vehicleId, Guid stationId, DateTimeOffset startDate,
        DateTimeOffset endDate);
        Task<IEnumerable<Domain.Entities.VehicleModel>> GetAllAsync(string? name, Guid? segmentId);
    }
}
