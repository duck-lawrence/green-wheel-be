using Application.Dtos.VehicleModel.Respone;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IModelImageService
    {
        Task<List<VehicleModelImageRes>> UploadModelImagesAsync(Guid modelId, List<IFormFile> files);

        Task DeleteModelImagesAsync(Guid modelId, List<Guid> imageIds);

        Task<(VehicleModelImageRes mainImage, List<VehicleModelImageRes> galleryImages)>
            UploadAllModelImagesAsync(Guid modelId, List<IFormFile> files);
    }
}