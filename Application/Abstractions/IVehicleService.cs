using Application.Dtos.Vehicle.Request;
using Application.Dtos.Vehicle.Respone;
using Domain.Entities;

namespace Application.Abstractions
{
    public interface IVehicleService
    {
        Task<Guid> CreateVehicleAsync(CreateVehicleReq createVehicleReq);

        Task<int> UpdateVehicleAsync(Guid Id, UpdateVehicleReq updateVehicleReq);

        Task<bool> DeleteVehicle(Guid id);

        Task<IEnumerable<VehicleViewRes>> GetAllVehicle();

        Task<VehicleViewRes> GetVehicleById(Guid id);

        //Task<Vehicle> GetVehicle(Guid stationId, Guid modelId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}