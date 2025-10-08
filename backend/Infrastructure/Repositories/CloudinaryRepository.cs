using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _settings;
        private readonly ILogger<CloudinaryRepository> _logger;

        private static readonly string[] _allowedTypes =
            { "image/jpeg", "image/jpg", "image/png", "image/webp" };

        public CloudinaryRepository(
            Cloudinary cloudinary,
            IOptions<CloudinarySettings> options,
            ILogger<CloudinaryRepository> logger)
        {
            _cloudinary = cloudinary;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<PhotoUploadResult> UploadAsync(UploadImageReq file, string folder)
        {
            if (file == null || file.File == null)
                throw new ArgumentException(Message.CloudinaryMessage.NotFoundObjectInFile);

            if (!_allowedTypes.Contains(file.File.ContentType))
                throw new ArgumentException(Message.CloudinaryMessage.InvalidFileType);

            using var stream = file.File.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.File.FileName, stream),
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

        public async Task<bool> DeleteAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return false;

            var delParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(delParams);

            var ok = string.Equals(result.Result, "ok", StringComparison.OrdinalIgnoreCase)
                  || string.Equals(result.Result, "not_found", StringComparison.OrdinalIgnoreCase);

            if (!ok)
                _logger.LogWarning("Cloudinary delete returned: {Result} for {PublicId}", result.Result, publicId);

            return ok;
        }
    }
}