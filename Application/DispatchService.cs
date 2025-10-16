using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Dispatch.Request;
using Application.Dtos.Dispatch.Response;
using Application.Helpers;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class DispatchService : IDispatchRequestService
    {
        private readonly IDispatchRepository _repository;
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IStaffRepository _staffRepository;

        public DispatchService(IDispatchRepository repository, IMapper mapper, IVehicleRepository vehicleRepository, IStaffRepository staffRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _vehicleRepository = vehicleRepository;
            _staffRepository = staffRepository;
        }

        public async Task<Guid> CreateAsync(Guid adminId, Guid stationId, CreateDispatchReq req)
        {
            DispatchValidationHelper.EnsureDifferentStations(stationId, req.ToStationId);
            await DispatchValidationHelper.ValidateStaffsInStationAsync(_staffRepository, req.staffIds, stationId);
            await DispatchValidationHelper.ValidateVehiclesInStationAsync(_vehicleRepository, req.vehicleIds, stationId);
            var entity = _mapper.Map<DispatchRequest>(req);
            entity.Id = Guid.NewGuid();
            entity.FromStationId = stationId;
            entity.RequestAdminId = adminId;
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = entity.CreatedAt;
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task<IEnumerable<DispatchRes>> GetAllAsync(Guid? fromStationId, Guid? toStationId, DispatchRequestStatus? status)
        {
            var data = await _repository.GetAllExpandedAsync(fromStationId, toStationId, status.HasValue ? (int)status.Value : null);
            return _mapper.Map<IEnumerable<DispatchRes>>(data);
        }

        public async Task<DispatchRes?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdWithFullInfoAsync(id);
            return entity == null ? null : _mapper.Map<DispatchRes>(entity);
        }

        public async Task UpdateStatusAsync(Guid currentAdminId, Guid currentAdminStationId, Guid id, UpdateDispatchReq req)
        {
            var entity = await _repository.GetByIdAsync(id) ?? throw new NotFoundException(Message.DispatchMessage.NotFound);
            var currentStatus = (DispatchRequestStatus)entity.Status;
            var newStatus = req.status;
            if (newStatus == (int)DispatchRequestStatus.Approved || newStatus == (int)DispatchRequestStatus.Rejected)
            {
                if (entity.ToStationId != currentAdminStationId)
                    throw new ForbidenException(Message.UserMessage.DoNotHavePermission);
                if (currentStatus != DispatchRequestStatus.Pending)
                    throw new BadRequestException(Message.DispatchMessage.OnlyApprovedCanReceive);

                entity.ApprovedAdminId = currentAdminId;
                entity.Status = newStatus;
            }
            else if (newStatus == (int)DispatchRequestStatus.Received)
            {
                if (entity.FromStationId != currentAdminStationId
                    || currentStatus != DispatchRequestStatus.Approved)
                    throw new ForbidenException(Message.UserMessage.DoNotHavePermission);

                await _staffRepository.UpdateStationForDispatchAsync(entity.Id, entity.ToStationId);
                await _vehicleRepository.UpdateStationForDispatchAsync(entity.Id, entity.ToStationId);
                entity.Status = (int)DispatchRequestStatus.Received;
            }
            else if (newStatus == (int)DispatchRequestStatus.Cancel)
            {
                if (entity.FromStationId != currentAdminStationId
                    || currentStatus != DispatchRequestStatus.Pending)
                    throw new ForbidenException(Message.UserMessage.DoNotHavePermission);

                entity.Status = (int)DispatchRequestStatus.Cancel;
            }
            else
            {
                throw new BadRequestException(Message.DispatchMessage.InvalidStatus);
            }

            await _repository.UpdateAsync(entity);
        }
    }
}