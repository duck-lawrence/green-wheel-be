using Application.Dtos.Common.Request;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IPhotoService
    {
        //upload image 
        Task<PhotoUploadResult> UploadPhotoAsync(UploadImageReq file, string folder = null);

        //delete image
        Task<bool> DeletePhotoAsync(string publicId);
    }
}
