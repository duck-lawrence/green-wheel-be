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

            entity.DispatchRequestStaffs = req.staffIds != null
                ? _mapper.Map<List<DispatchRequestStaff>>(req.staffIds)
                : new List<DispatchRequestStaff>();
            entity.DispatchRequestVehicles = req.vehicleIds != null
                ? _mapper.Map<List<DispatchRequestVehicle>>(req.vehicleIds)
                : new List<DispatchRequestVehicle>();

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
            switch (newStatus)
            {
                case DispatchRequestStatus.Approved:
                case DispatchRequestStatus.Rejected:
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.ToStationId,
                        currentStatus,
                        DispatchRequestStatus.Pending,
                        Message.UserMessage.DoNotHavePermission,
                        Message.DispatchMessage.OnlyPendingCanApproveReject);
                    entity.ApprovedAdminId = currentAdminId;
                    entity.Status = (int)newStatus;
                    break;

                case DispatchRequestStatus.Received:
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.ToStationId,
                        currentStatus,
                        DispatchRequestStatus.Approved,
                        Message.UserMessage.DoNotHavePermission,
                        Message.DispatchMessage.OnlyApprovedCanReceive);
                    break;

                case DispatchRequestStatus.Cancel:
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.ToStationId,
                        currentStatus,
                        DispatchRequestStatus.Pending,
                        Message.UserMessage.DoNotHavePermission,
                        Message.DispatchMessage.OnlyPendingCanCancel);
                    entity.Status = (int)DispatchRequestStatus.Cancel;
                    break;

                default:
                    throw new BadRequestException(Message.DispatchMessage.InvalidStatus);
            }
            await _repository.UpdateAsync(entity);
        }
    }
}