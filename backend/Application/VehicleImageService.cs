using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.UnitOfWorks;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class VehicleImageService : IVehicleImageService
    {
        private readonly IRentalContractUow _uow;
        private readonly IPhotoService _photoService;
        private readonly ILogger<VehicleImageService> _logger;

        public VehicleImageService(
            IRentalContractUow uow,
            IPhotoService photoService,
            ILogger<VehicleImageService> logger)
        {
            _uow = uow;
            _photoService = photoService;
            _logger = logger;
        }

        public async Task<List<VehicleImage>> UploadVehicleImagesAsync(Guid vehicleId, List<IFormFile> files)
        {
            if (files == null || !files.Any())
                throw new ArgumentException("No file chosen.");

            var uploadedImages = new List<VehicleImage>();
            _logger.LogInformation(">>> Uploading images for vehicleId: {VehicleId}", vehicleId);
            foreach (var file in files)
            {
                try
                {
                    var uploadReq = new UploadImageReq { File = file };
                    var result = await _photoService.UploadPhotoAsync(uploadReq, $"vehicles/{vehicleId}");

                    if (string.IsNullOrEmpty(result.Url))
                        throw new Exception("Upload fail.");

                    var image = new VehicleImage
                    {
                        Id = Guid.NewGuid(),
                        VehicleId = vehicleId,
                        Url = result.Url,
                        PublicId = result.PublicID,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _uow.VehicleImages.AddAsync(image);
                    uploadedImages.Add(image);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "fail upload {File} for car {VehicleId}", file.FileName, vehicleId);
                    throw;
                }
            }

            await _uow.SaveChangesAsync();
            _logger.LogInformation("Đã upload {Count} ảnh cho xe {VehicleId}", uploadedImages.Count, vehicleId);
            return uploadedImages;
        }

        public async Task DeleteVehicleImagesAsync(Guid vehicleId, List<Guid> imageIds)
        {
            if (imageIds == null || !imageIds.Any())
                throw new ArgumentException("no id found to delete");

            var images = await _uow.VehicleImages.FindAsync(x => imageIds.Contains(x.Id) && x.VehicleId == vehicleId);

            foreach (var image in images)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(image.PublicId);
                    _uow.VehicleImages.Remove(image);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "can not delete {PublicId} on Cloudinary", image.PublicId);
                }
            }

            await _uow.SaveChangesAsync();
            _logger.LogInformation("Deleted {Count} picture(s) of {VehicleId}", images.Count(), vehicleId);
        }
    }
}