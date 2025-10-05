using Application.Dtos.Common.Request;
using Domain.Entities;

namespace Application.Repositories
{
    public interface ICloudinaryRepository
    {
        Task<PhotoUploadResult> UploadAsync(UploadImageReq file, string folder);

        Task<bool> DeleteAsync(string publicId);
    }
}