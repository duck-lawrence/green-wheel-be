using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IVehicleImageService
    {
        Task<List<VehicleImage>> UploadVehicleImagesAsync(Guid vehicleId, List<IFormFile> files);

        Task DeleteVehicleImagesAsync(Guid vehicleId, List<Guid> imageIds);
    }
}