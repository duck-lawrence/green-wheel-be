using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.VehicleModel.Request;
using Application.Dtos.VehicleModel.Respone;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly IVehicleModelRepository _vehicleModelRepository;
        private readonly IMapper _mapper;
        private readonly IMediaUow _uow;
        private readonly IPhotoService _photoService;

        public VehicleModelService(IVehicleModelRepository vehicleModelRepository, IMapper mapper, IMediaUow uow, IPhotoService photoService)
        {
            _vehicleModelRepository = vehicleModelRepository;
            _mapper = mapper;
            _uow = uow;
            _photoService = photoService;
        }

        public async Task<Guid> CreateVehicleModelAsync(CreateVehicleModelReq createVehicleModelReq)
        {
            Guid id;
            do
            {
                id = new Guid();
            } while (await _vehicleModelRepository.GetByIdAsync(id) != null);
            var vehicleModel = _mapper.Map<VehicleModel>(createVehicleModelReq);
            vehicleModel.CreatedAt = vehicleModel.UpdatedAt = DateTimeOffset.UtcNow;
            vehicleModel.DeletedAt = null;
            return await _vehicleModelRepository.AddAsync(vehicleModel);
        }

        public async Task<bool> DeleteVehicleModleAsync(Guid id)
        {
            return await _vehicleModelRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<VehicleModelViewRes>> GetAllVehicleModels(VehicleFilterReq vehicleFilterReq)
        {
            return await _vehicleModelRepository.FilterVehicleModelsAsync(vehicleFilterReq.StationId, vehicleFilterReq.StartDate, vehicleFilterReq.EndDate, vehicleFilterReq.SegmentId);
        }

        public async Task<VehicleModelViewRes> GetByIdAsync(Guid id, Guid stationId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var vehicleModelViewRes = await _vehicleModelRepository.GetByIdAsync(id, stationId, startDate, endDate);
            if (vehicleModelViewRes == null)
            {
                throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);
            }
            return vehicleModelViewRes;
        }

        public async Task<int> UpdateVehicleModelAsync(Guid Id, UpdateVehicleModelReq req)
        {
            var model = await _vehicleModelRepository.GetByIdAsync(Id) ?? throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);

            _mapper.Map(req, model);
            model.UpdatedAt = DateTimeOffset.UtcNow;

            return await _vehicleModelRepository.UpdateAsync(model);
        }

        public async Task<string> UploadMainImageAsync(Guid modelId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException(Message.CloudinaryMessage.NotFoundObjectInFile);

            var model = await _uow.VehicleModels.GetByIdAsync(modelId)
                ?? throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);

            var oldPublicId = model.ImagePublicId;

            var uploadReq = new UploadImageReq { File = file };
            var uploaded = await _photoService.UploadPhotoAsync(uploadReq, $"models/{modelId}/main");

            await using var trx = await _uow.BeginTransactionAsync();
            try
            {
                model.ImageUrl = uploaded.Url;
                model.ImagePublicId = uploaded.PublicID;
                model.UpdatedAt = DateTimeOffset.UtcNow;

                await _uow.VehicleModels.UpdateAsync(model);
                await _uow.SaveChangesAsync();
                await trx.CommitAsync();
            }
            catch
            {
                await trx.RollbackAsync();
                try { await _photoService.DeletePhotoAsync(uploaded.PublicID); } catch { }
                throw;
            }

            if (!string.IsNullOrEmpty(oldPublicId))
            {
                try { await _photoService.DeletePhotoAsync(oldPublicId); } catch { }
            }

            return model.ImageUrl!;
        }

        public async Task DeleteMainImageAsync(Guid modelId)
        {
            var model = await _uow.VehicleModels.GetByIdAsync(modelId)
                ?? throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);

            if (string.IsNullOrEmpty(model.ImagePublicId))
                throw new BadRequestException(Message.VehicleModelImageMessage.NoMainImage);

            await _photoService.DeletePhotoAsync(model.ImagePublicId);

            model.ImageUrl = null;
            model.ImagePublicId = null;
            model.UpdatedAt = DateTimeOffset.UtcNow;

            await _uow.VehicleModels.UpdateAsync(model);
            await _uow.SaveChangesAsync();
        }
    }
}