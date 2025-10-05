using Application.Dtos.Common.Request;

namespace Application.Repositories
{
    public interface ICloudinaryRepository
    {
        Task<PhotoUploadResult> UploadAsync(UploadImageReq file, string folder);

        Task<bool> DeleteAsync(string publicId);
    }
}