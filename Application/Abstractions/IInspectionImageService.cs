using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IChecklistItemImageService
    {
        Task<object> UploadChecklistItemImageAsync(Guid itemId, IFormFile file);

        Task<object> DeleteChecklistItemImageAsync(Guid itemId);
    }
}