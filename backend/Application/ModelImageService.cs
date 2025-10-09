using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.UnitOfWorks;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class ModelImageService : IModelImageService
    {
        private readonly IModelImageUow _uow;
        private readonly IPhotoService _photoService;

        public ModelImageService(IModelImageUow uow, IPhotoService photoService)
        {
            _uow = uow;
            _photoService = photoService;
        }

        public async Task<List<ModelImage>> UploadModelImagesAsync(Guid modelId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                throw new BadRequestException(Message.CloudinaryMessage.NotFoundObjectInFile);

            var model = await _uow.VehicleModelRepository.GetByIdAsync(modelId);
            if (model == null)
                throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);

            var uploadedImages = new List<ModelImage>();

            foreach (var file in files)
            {
                var uploadReq = new UploadImageReq { File = file };
                var result = await _photoService.UploadPhotoAsync(uploadReq, $"models/{modelId}");

                if (string.IsNullOrEmpty(result.Url))
                    throw new BusinessException(Message.CloudinaryMessage.UploadFailed);

                var image = new ModelImage
                {
                    Id = Guid.NewGuid(),
                    ModelId = modelId,
                    Url = result.Url,
                    PublicId = result.PublicID,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                await _uow.ModelImageRepository.AddAsync(image);
                uploadedImages.Add(image);
            }

            await _uow.SaveChangesAsync();
            return uploadedImages;
        }

        public async Task DeleteModelImagesAsync(Guid modelId, List<Guid> imageIds)
        {
            if (imageIds == null || imageIds.Count == 0)
                throw new BadRequestException(Message.CommonMessage.UnexpectedError);

            var images = await _uow.ModelImageRepository.FindAsync(x =>
                imageIds.Contains(x.Id) && x.ModelId == modelId);

            if (!images.Any())
                throw new NotFoundException(Message.CloudinaryMessage.NotFoundObjectInFile);

            foreach (var image in images)
            {
                // Không throw nếu Cloudinary không tìm thấy file
                await _photoService.DeletePhotoAsync(image.PublicId);

                _uow.ModelImageRepository.Remove(image);
            }

            await _uow.SaveChangesAsync();
        }

        public async Task<(string mainImageUrl, List<ModelImage> galleyImages)> UploadAllModelImagesAsync(Guid modelId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0) throw new BadHttpRequestException(Message.CommonMessage.UnexpectedError);
            var model = await _uow.VehicleModelRepository.GetByIdAsync(modelId) ?? throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);
            var mainFile = files.FirstOrDefault();
            var mainUpload = await _photoService.UploadPhotoAsync(new UploadImageReq { File = mainFile });
            await using var trx = await _uow.BeginTransactionAsync();
            try
            {
                model.ImageUrl = mainUpload.Url;
                model.ImagePublicId = mainUpload.PublicID;
                model.UpdatedAt = DateTimeOffset.UtcNow;
                await _uow.VehicleModelRepository.UpdateAsync(model);
                var galleyImages = await UploadModelImagesAsync(modelId, files.Skip(1).ToList());
                await trx.CommitAsync();
                return (mainUpload.Url, galleyImages);
            }
            catch
            {
                await trx.RollbackAsync();
                // rollback ảnh main
                try { await _photoService.DeletePhotoAsync(mainUpload.PublicID); } catch { }

                // rollback ảnh phụ nếu có
                foreach (var file in files.Skip(1))
                {
                    try
                    {
                        var uploadPath = $"models/{modelId}/gallery";
                        await _photoService.DeletePhotoAsync(uploadPath);
                    }
                    catch { }
                }
                throw;
            }
        }
    }
}