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
        private readonly IVehicleModelUow _vehicleModelUow;
        private readonly IVehicleComponentService _vehicleComponentService;

        public VehicleModelService(IVehicleModelRepository vehicleModelRepository, IMapper mapper, IMediaUow uow,
            IPhotoService photoService, IVehicleModelUow vehicleModelUow, IVehicleComponentService vehicleComponentService)
        {
            _vehicleModelRepository = vehicleModelRepository;
            _mapper = mapper;
            _uow = uow;
            _photoService = photoService;
            _vehicleModelUow = vehicleModelUow;
            _vehicleComponentService = vehicleComponentService;
        }

        public async Task<Guid> CreateVehicleModelAsync(CreateVehicleModelReq createVehicleModelReq)
        {
            Guid id;
            do
            {
                id = new Guid();
            } while (await _vehicleModelRepository.GetByIdAsync(id) != null);
            var vehicleModel = _mapper.Map<VehicleModel>(createVehicleModelReq);
            return await _vehicleModelRepository.AddAsync(vehicleModel);
        }

        public async Task<bool> DeleteVehicleModleAsync(Guid id)
        {
            return await _vehicleModelRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<VehicleModelViewRes>> SearchVehicleModel(VehicleFilterReq vehicleFilterReq)
        {
            var vehicleModels = await _vehicleModelRepository.FilterVehicleModelsAsync(vehicleFilterReq.StationId, vehicleFilterReq.StartDate, vehicleFilterReq.EndDate, vehicleFilterReq.SegmentId);
            return _mapper.Map<IEnumerable<VehicleModelViewRes>>(vehicleModels)  ?? [];
        }

        public async Task<VehicleModelViewRes> GetByIdAsync(Guid id, Guid stationId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var vehicleModelViewRes = await _vehicleModelRepository.GetByIdAsync(id, stationId, startDate, endDate)
            ?? throw new NotFoundException(Message.VehicleModelMessage.NotFound);
            return _mapper.Map<VehicleModelViewRes>(vehicleModelViewRes);
        }

        public async Task<IEnumerable<VehicleModelViewRes>> GetAllAsync(string? name, Guid? segmentId)
        {
            var models = await _uow.VehicleModels.GetAllAsync(name, segmentId);
            return _mapper.Map<IEnumerable<VehicleModelViewRes>>(models) ?? [];
        }
        public async Task<int> UpdateVehicleModelAsync(Guid Id, UpdateVehicleModelReq req)
        {
            var model = await _vehicleModelRepository.GetByIdAsync(Id) ?? throw new NotFoundException(Message.VehicleModelMessage.NotFound);

            _mapper.Map(req, model);
            model.UpdatedAt = DateTimeOffset.UtcNow;

            return await _vehicleModelRepository.UpdateAsync(model);
        }

        public async Task<string> UploadMainImageAsync(Guid modelId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException(Message.CloudinaryMessage.NotFoundObjectInFile);

            var model = await _uow.VehicleModels.GetByIdAsync(modelId)
                ?? throw new NotFoundException(Message.VehicleModelMessage.NotFound);

            var oldPublicId = model.ImagePublicId;

            var uploadReq = new UploadImageReq { File = file };
            var uploaded = await _photoService.UploadPhotoAsync(uploadReq, $"models/{modelId}/main");

            await using var trx = await _uow.BeginTransactionAsync();
            try
            {
                model.ImageUrl = uploaded.Url;
                model.ImagePublicId = uploaded.PublicID;

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
                ?? throw new NotFoundException(Message.VehicleModelMessage.NotFound);

            if (string.IsNullOrEmpty(model.ImagePublicId))
                throw new BadRequestException(Message.VehicleModelImageMessage.NotFound);

            await _photoService.DeletePhotoAsync(model.ImagePublicId);

            model.ImageUrl = null;
            model.ImagePublicId = null;
            model.UpdatedAt = DateTimeOffset.UtcNow;

            await _uow.VehicleModels.UpdateAsync(model);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateVehicleModelComponentsAsync(Guid id, UpdateModelComponentsReq req)
        {
            IEnumerable<ModelComponent> modelComponents = await _vehicleModelUow.ModelComponentRepository.GetByModelIdAsync(id);
            IEnumerable<Guid> needAddItem = req.ComponentIds.Where(id => !modelComponents.Any(m => m.ComponentId == id));
            IEnumerable<ModelComponent> needDeleteItem = modelComponents.Where(item => !req.ComponentIds.Any(id => id == item.ComponentId));
            await _vehicleModelUow.BeginTransactionAsync();
            try
            {
                IEnumerable<ModelComponent> addItems = needAddItem.Select(componentId => new ModelComponent
                {
                    Id = Guid.NewGuid(),
                    ModelId = id,
                    ComponentId = componentId,
                });
                await _vehicleModelUow.ModelComponentRepository.DeleteRangeAsync(needDeleteItem);
                await _vehicleModelUow.ModelComponentRepository.AddRangeAsync(addItems);
                await _vehicleModelUow.SaveChangesAsync();
                await _vehicleModelUow.CommitAsync();
            }
            catch (Exception)
            {
                await _vehicleModelUow.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllModelMainImage()
        {
            var vehicleModels = await _vehicleModelRepository.GetAllAsync(null, null);
            IEnumerable<string> imageUrls = [];
            if (vehicleModels != null || vehicleModels.Any())
            {
                imageUrls = vehicleModels
                    .Where(vm => !string.IsNullOrEmpty(vm.ImageUrl))
                    .Select(vm => vm.ImageUrl!);
            }
            return imageUrls;
        }
    }
}