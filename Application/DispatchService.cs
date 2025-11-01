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

        public DispatchService(
            IDispatchRepository repository,
            IMapper mapper,
            IVehicleRepository vehicleRepository,
            IStaffRepository staffRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _vehicleRepository = vehicleRepository;
            _staffRepository = staffRepository;
        }

        // ========== CREATE ==========
        public async Task<Guid> CreateAsync(Guid adminId, CreateDispatchReq req)
        {
            if (req is null)
                throw new BadRequestException(Message.DispatchMessage.InvalidStatus);

            // admin A (trạm gửi) tạo
            var adminStaff = await _staffRepository.GetByUserIdAsync(adminId)
                ?? throw new ForbidenException(Message.UserMessage.DoNotHavePermission);

            var toStationId = req.FromStationId;         // Đây mới là trạm NHẬN (B)
            var fromStationId = adminStaff.StationId;
            // bạn flow: admin gửi -> admin B approve -> ... 
            // Ở đây ta hiểu: admin A đang ngồi ở FROM station và gửi sang B
            // => toStationId phải là req.ToStationId? nhưng bạn đang để FromStationId trong req
            // nên ta giữ nguyên logic bạn đang chạy: admin A chọn "fromStationId" để gửi sang "trạm của mình"
            // muốn clear thì đổi dto sau.

            // 2 trạm phải khác
            DispatchValidationHelper.EnsureDifferentStations(fromStationId, toStationId);

            // validate staff
            if (req.NumberOfStaff is > 0)
            {
                var availableStaffCount =
                    await _staffRepository.CountAvailableStaffInStationAsync(fromStationId);
                if (req.NumberOfStaff > availableStaffCount)
                    throw new BadRequestException(Message.DispatchMessage.StaffNotInFromStation);
            }

            // validate vehicle
            if (req.Vehicles is { Length: > 0 })
            {
                foreach (var v in req.Vehicles)
                {
                    var availableVehicles = await _vehicleRepository
                        .CountAvailableVehiclesByModelAsync(fromStationId, v.ModelId);

                    if (availableVehicles < v.NumberOfVehicle)
                        throw new BadRequestException(
                            $"{Message.DispatchMessage.VehicleNotInFromStation} - model {v.ModelId} only {availableVehicles} available at this station.");
                }
            }

            var entity = new DispatchRequest
            {
                Id = Guid.NewGuid(),
                RequestAdminId = adminId,
                FromStationId = fromStationId,
                ToStationId = toStationId,
                Status = (int)DispatchRequestStatus.Pending,
            };

            await _repository.AddAsync(entity);
            return entity.Id;
        }

        // ========== GET ==========
        public async Task<IEnumerable<DispatchRes>> GetAllAsync(
            Guid? fromStationId,
            Guid? toStationId,
            DispatchRequestStatus? status)
        {
            var data = await _repository.GetAllExpandedAsync(
                fromStationId,
                toStationId,
                status.HasValue ? (int)status.Value : null);

            return _mapper.Map<IEnumerable<DispatchRes>>(data);
        }

        public async Task<DispatchRes?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdWithFullInfoAsync(id);
            return entity == null ? null : _mapper.Map<DispatchRes>(entity);
        }

        // ========== UPDATE STATUS ==========
        public async Task UpdateStatusAsync(
            Guid currentAdminId,
            Guid currentAdminStationId,
            Guid id,
            UpdateApproveDispatchReq req)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.DispatchMessage.NotFound);

            var currentStatus = (DispatchRequestStatus)entity.Status;
            var newStatus = (DispatchRequestStatus)req.Status;

            switch (newStatus)
            {
                // --------------------------------------------------
                // 1) ADMIN B (TRẠM NHẬN) DUYỆT  -> Approved
                // --------------------------------------------------
                case DispatchRequestStatus.Approved:
                    // chỉ TO station mới được duyệt
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.ToStationId,
                        currentStatus,
                        DispatchRequestStatus.Pending,
                        Message.UserMessage.DoNotHavePermission,
                        Message.DispatchMessage.OnlyPendingCanApproveReject);

                    if (req.StaffIds == null || req.VehicleIds == null)
                        throw new BadRequestException(Message.DispatchMessage.IdNull);

                    // staff + vehicle phải đang ở FROM station (vì FROM station gửi đồ sang B)
                    await DispatchValidationHelper.ValidateStaffsInStationAsync(
                        _staffRepository, req.StaffIds, entity.FromStationId);

                    await DispatchValidationHelper.ValidateVehiclesInStationAsync(
                        _vehicleRepository, req.VehicleIds, entity.FromStationId);

                    // clear cũ
                    await _repository.ClearDispatchRelationsAsync(entity.Id);

                    // add mới
                    var newStaffs = req.StaffIds.Select(staffId => new DispatchRequestStaff
                    {
                        Id = Guid.NewGuid(),
                        DispatchRequestId = entity.Id,
                        StaffId = staffId
                    }).ToList();

                    var newVehicles = req.VehicleIds.Select(vehicleId => new DispatchRequestVehicle
                    {
                        Id = Guid.NewGuid(),
                        DispatchRequestId = entity.Id,
                        VehicleId = vehicleId
                    }).ToList();

                    await _repository.AddDispatchRelationsAsync(newStaffs, newVehicles);

                    entity.ApprovedAdminId = currentAdminId;
                    entity.Status = (int)DispatchRequestStatus.Approved;
                    break;

                // --------------------------------------------------
                // 2) ADMIN A (TRẠM GỬI) XÁC NHẬN -> ConfirmApproved
                // --------------------------------------------------
                case DispatchRequestStatus.ConfirmApproved:
                    // chỉ FROM station mới được confirm
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.FromStationId,
                        currentStatus,
                        DispatchRequestStatus.Approved,
                        Message.UserMessage.DoNotHavePermission,
                        "Only approved request can be confirmed.");

                    entity.Status = (int)DispatchRequestStatus.ConfirmApproved;
                    break;

                // --------------------------------------------------
                // 3) ADMIN A (TRẠM GỬI) ĐÁNH DẤU ĐÃ GIAO XONG -> Received
                // --------------------------------------------------
                case DispatchRequestStatus.Received:
                    // vẫn là FROM station (vì bên gửi đóng request)
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.FromStationId,
                        currentStatus,
                        DispatchRequestStatus.ConfirmApproved,
                        Message.UserMessage.DoNotHavePermission,
                        "Only confirmed request can be received.");

                    entity.Status = (int)DispatchRequestStatus.Received;

                    // lúc này thực sự chuyển staff + vehicle sang TRẠM NHẬN
                    await _staffRepository.UpdateStationForDispatchAsync(entity.Id, entity.ToStationId);
                    await _vehicleRepository.UpdateStationForDispatchAsync(entity.Id, entity.ToStationId);
                    break;

                // --------------------------------------------------
                // 4) ADMIN B (TRẠM NHẬN) không nhận -> Cancelled
                // --------------------------------------------------
                case DispatchRequestStatus.Cancelled:
                    // chỉ TO station được huỷ, và chỉ khi đang Approved
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.ToStationId,
                        currentStatus,
                        DispatchRequestStatus.Approved,
                        Message.UserMessage.DoNotHavePermission,
                        "Only approved request can be cancelled.");

                    entity.Status = (int)DispatchRequestStatus.Cancelled;
                    break;

                // --------------------------------------------------
                // 5) ADMIN B (TRẠM NHẬN) từ chối ngay từ Pending -> Rejected
                // --------------------------------------------------
                case DispatchRequestStatus.Rejected:
                    DispatchValidationHelper.EnsureCanUpdate(
                        currentAdminStationId,
                        entity.ToStationId,
                        currentStatus,
                        DispatchRequestStatus.Pending,
                        Message.UserMessage.DoNotHavePermission,
                        Message.DispatchMessage.OnlyPendingCanApproveReject);

                    entity.Status = (int)DispatchRequestStatus.Rejected;
                    entity.ApprovedAdminId = null;
                    entity.Description = DispatchValidationHelper.AppendDescription(
                        entity.Description, req.Description);
                    break;

                default:
                    throw new BadRequestException(Message.DispatchMessage.InvalidStatus);
            }

            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await _repository.UpdateAsync(entity);
        }
    }
}
