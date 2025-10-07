using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IModelImageService
    {
        Task<List<ModelImage>> UploadModelImagesAsync(Guid modelId, List<IFormFile> files);

        Task DeleteModelImagesAsync(Guid modelId, List<Guid> imageIds);
    }
}