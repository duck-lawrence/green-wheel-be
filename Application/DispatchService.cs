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
            var entity = await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Dispatch not found");
            var currentStatus = (DispatchRequestStatus)entity.Status;
            var newStatus = req.Status;
            if (newStatus == DispatchRequestStatus.Approved || newStatus == DispatchRequestStatus.Rejected)
            {
                if (entity.ToStationId != currentAdminStationId || currentStatus != DispatchRequestStatus.Pending)
                    throw new ForbidenException("Invalid operation.");

                entity.ApprovedAdminId = currentAdminId;
                entity.Status = (int)newStatus;
            }
            else if (newStatus == DispatchRequestStatus.Received)
            {
                if (entity.FromStationId != currentAdminStationId || currentStatus != DispatchRequestStatus.Approved) throw new ForbidenException("Invalid operation.");
                await _staffRepository.UpdateStationForDispatchAsync(entity.Id, entity.ToStationId);
                await _vehicleRepository.UpdateStationForDispatchAsync(entity.Id, entity.ToStationId);
                entity.Status = (int)DispatchRequestStatus.Received;
            }
            else if (newStatus == DispatchRequestStatus.Cancel)
            {
                if (entity.FromStationId != currentAdminStationId || currentStatus != DispatchRequestStatus.Pending) throw new ForbidenException("Invalid operation.");
                entity.Status = (int)DispatchRequestStatus.Cancel;
            }
            else throw new BadRequestException("Invalid status");
            await _repository.UpdateAsync(entity);
        }
    }
}