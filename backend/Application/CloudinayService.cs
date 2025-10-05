using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CloudinaryService : IPhotoService
    {
        private readonly ICloudinaryRepository _cloudRepo;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(ICloudinaryRepository cloudRepo, ILogger<CloudinaryService> logger)
        {
            _cloudRepo = cloudRepo;
            _logger = logger;
        }

        public async Task<PhotoUploadResult> UploadPhotoAsync(UploadImageReq file, string? folder = null)
        {
            try
            {
                folder ??= "uploads";
                var result = await _cloudRepo.UploadAsync(file, folder);
                _logger.LogInformation("Image uploaded successfully: {Url}", result.Url);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload failed for file {File}", file.File.FileName);
                throw;
            }
        }

        public async Task<bool> DeletePhotoAsync(string publicId)
        {
            try
            {
                var deleted = await _cloudRepo.DeleteAsync(publicId);
                if (deleted)
                    _logger.LogInformation("Deleted image {PublicId} successfully", publicId);
                else
                    _logger.LogWarning("Failed to delete image {PublicId}", publicId);

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {PublicId}", publicId);
                return false;
            }
        }
    }
}