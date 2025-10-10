using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class ChecklistItemImageService : IChecklistItemImageService
    {
        private readonly IVehicleChecklistItemRepository _itemRepository;
        private readonly IPhotoService _photoService;

        public ChecklistItemImageService(
            IVehicleChecklistItemRepository itemRepository,
            IPhotoService photoService)
        {
            _itemRepository = itemRepository;
            _photoService = photoService;
        }

        public async Task<object> UploadChecklistItemImageAsync(Guid itemId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException(Message.CloudinaryMessage.NotFoundObjectInFile);

            var item = await _itemRepository.GetByIdAsync(itemId)
                ?? throw new NotFoundException(Message.VehicleChecklistMessage.VehicleChecklistNotFound);

            // Upload ảnh lên Cloudinary
            var uploadResult = await _photoService.UploadPhotoAsync(
                new UploadImageReq { File = file },
                $"checklists/{item.ChecklistId}/items");

            if (string.IsNullOrEmpty(uploadResult.Url))
                throw new BusinessException(Message.CloudinaryMessage.UploadFailed);

            // Cập nhật thông tin ảnh vào DB
            item.ImageUrl = uploadResult.Url;
            item.ImagePublicId = uploadResult.PublicID;
            item.UpdatedAt = DateTimeOffset.UtcNow;

            await _itemRepository.UpdateAsync(item);

            return new
            {
                data = new
                {
                    itemId,
                    imageUrl = uploadResult.Url,
                    publicId = uploadResult.PublicID
                },
                message = Message.CloudinaryMessage.UploadSuccess
            };
        }

        public async Task<object> DeleteChecklistItemImageAsync(Guid itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId)
                ?? throw new NotFoundException(Message.VehicleChecklistMessage.VehicleChecklistNotFound);

            if (string.IsNullOrEmpty(item.ImagePublicId)) throw new BadRequestException(Message.CloudinaryMessage.NotFoundObjectInFile);

            try
            {
                await _photoService.DeletePhotoAsync(item.ImagePublicId);
            }
            catch
            {
                throw new BusinessException(Message.CloudinaryMessage.DeleteFailed);
            }

            item.ImageUrl = null;
            item.ImagePublicId = null;
            item.UpdatedAt = DateTimeOffset.UtcNow;

            await _itemRepository.UpdateAsync(item);

            return new { message = Message.CloudinaryMessage.DeleteSuccess };
        }
    }
}