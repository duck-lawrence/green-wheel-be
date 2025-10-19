using Application.Dtos.VehicleChecklistItem.Respone;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions
{
    public interface IChecklistItemImageService
    {
        Task<string> UploadChecklistItemImageAsync(Guid itemId, IFormFile file);

        Task<ChecklistItemImageRes> DeleteChecklistItemImageAsync(Guid itemId);
    }
}