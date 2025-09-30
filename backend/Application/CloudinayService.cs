using Application.Abstractions;
using Application.AppSettingConfigurations;
using Application.Constants;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application
{
    public class CloudinayService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _settings;
        private readonly ILogger<CloudinayService> _logger;

        // allowed type
        private static readonly string[] _allowedType = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };

        public CloudinayService(
            Cloudinary cloudinary,
            IOptions<CloudinarySettings> options,
            ILogger<CloudinayService> logger)
        {
            _cloudinary = cloudinary;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<bool> DeletePhotoAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return false;
            var delParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(delParams);

            var ok = string.Equals(result.Result, "ok", StringComparison.OrdinalIgnoreCase)
                 || string.Equals(result.Result, "not_found", StringComparison.OrdinalIgnoreCase);

            if (!ok)
                _logger.LogWarning($"Cloudinary delete returned: {result.Result} for {publicId}");

            return ok;
        }

        public async Task<PhotoUploadResult> UploadPhotoAsync(IFormFile file, string? folder = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException(Message.Cloudinary.NotFoundObjectInFile);

            if (!_allowedType.Contains(file.ContentType))
                throw new ArgumentException(Message.Cloudinary.InvalidFileType);

            folder ??= _settings.ApiFolder;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation().Crop("limit").FetchFormat("auto").Quality("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new InvalidOperationException(result.Error.Message);

            return new PhotoUploadResult
            {
                Url = result.Url?.ToString() ?? "",
                PublicID = result.PublicId
            };
        }
    }
}
