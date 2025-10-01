using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IPhotoService
    {
        //upload image 
        Task<PhotoUploadResult> UploadPhotoAsync(IFormFile file, string folder = null);

        //delete image
        Task<bool> DeletePhotoAsync(string publicId);
    }
}
