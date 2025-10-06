using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.UnitOfWorks;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class ModelImageService : IModelImageService
    {
        private readonly IRentalContractUow _uow;
        private readonly IPhotoService _photoService;
        private readonly ILogger<ModelImageService> _logger;

        public ModelImageService(
            IRentalContractUow uow,
            IPhotoService photoService,
            ILogger<ModelImageService> logger)
        {
            _uow = uow;
            _photoService = photoService;
            _logger = logger;
        }

        public async Task<List<ModelImage>> UploadModelImagesAsync(Guid modelId, List<IFormFile> files)
        {
            if (files == null || !files.Any())
                throw new ArgumentException("No file chosen.");

            var uploadedImages = new List<ModelImage>();
            _logger.LogInformation(">>> Uploading images for modelId: {ModelId}", modelId);

            foreach (var file in files)
            {
                try
                {
                    var uploadReq = new UploadImageReq { File = file };
                    var result = await _photoService.UploadPhotoAsync(uploadReq, $"models/{modelId}");

                    if (string.IsNullOrEmpty(result.Url))
                        throw new Exception("Upload failed.");

                    var image = new ModelImage
                    {
                        Id = Guid.NewGuid(),
                        ModelId = modelId,
                        Url = result.Url,
                        PublicId = result.PublicID,
                        CreatedAt = DateTimeOffset.UtcNow,
                        UpdatedAt = DateTimeOffset.UtcNow
                    };

                    await _uow.ModelImages.AddAsync(image);
                    uploadedImages.Add(image);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "fail upload {File} for model {ModelId}", file.FileName, modelId);
                    throw;
                }
            }

            await _uow.SaveChangesAsync();
            _logger.LogInformation("Uploaded {Count} image(s) for model {ModelId}", uploadedImages.Count, modelId);

            return uploadedImages;
        }

        public async Task DeleteModelImagesAsync(Guid modelId, List<Guid> imageIds)
        {
            if (imageIds == null || !imageIds.Any())
                throw new ArgumentException("No image IDs to delete.");

            var images = await _uow.ModelImages.FindAsync(x => imageIds.Contains(x.Id) && x.ModelId == modelId);

            foreach (var image in images)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(image.PublicId);
                    _uow.ModelImages.Remove(image);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Cannot delete {PublicId} on Cloudinary", image.PublicId);
                }
            }

            await _uow.SaveChangesAsync();
            _logger.LogInformation("Deleted {Count} image(s) of model {ModelId}", images.Count(), modelId);
        }
    }
}