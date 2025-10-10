using Application.Abstractions;
using Application.Constants;
using Application.AppExceptions;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Repositories;

namespace Application
{
    public class CloudinaryService : IPhotoService
    {
        private readonly ICloudinaryRepository _cloudRepo;

        public CloudinaryService(ICloudinaryRepository cloudRepo)
        {
            _cloudRepo = cloudRepo;
        }

        public async Task<PhotoUploadResult> UploadPhotoAsync(UploadImageReq file, string? folder = null)
        {
            if (file.File == null || file.File.Length == 0)
                throw new BadRequestException(Message.CloudinaryMessage.NotFoundObjectInFile);

            folder ??= "uploads";

            var result = await _cloudRepo.UploadAsync(file, folder);

            if (result == null || string.IsNullOrEmpty(result.Url))
                throw new BusinessException(Message.CloudinaryMessage.UploadFailed);
            return result;
        }

        public async Task<bool> DeletePhotoAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new BadRequestException(Message.CloudinaryMessage.NotFoundObjectInFile);

            var deleted = await _cloudRepo.DeleteAsync(publicId);

            return deleted;
        }
    }
}