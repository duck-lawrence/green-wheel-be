using Application.Constants;
using Application.Dtos.VehicleModel.Respone;
using Application.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VehicleModelRepository : GenericRepository<VehicleModel>, IVehicleModelRepository
    {
        private readonly IMapper _mapper;

        public VehicleModelRepository(IGreenWheelDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleModelViewRes>> FilterVehicleModelsAsync(
            Guid stationId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            Guid? segmentId = null)
        {
            if ((endDate - startDate).TotalHours < 24)
                throw new ArgumentException(Message.VehicleModelMessage.RentTimeIsNotAvailable);

            var startBuffer = startDate.AddDays(-10);
            var endBuffer = endDate.AddDays(10);

            // Query cơ bản
            var query = _dbContext.VehicleModels
                .Include(vm => vm.Vehicles)
                    .ThenInclude(v => v.RentalContracts)
                .Include(vm => vm.Brand)
                .Include(vm => vm.Segment)
                .Where(vm => vm.DeletedAt == null);

            if (segmentId.HasValue)
                query = query.Where(vm => vm.SegmentId == segmentId.Value);

            // Dùng ProjectTo để EF tự select đúng cột
            var models = await query
                .ProjectTo<VehicleModelViewRes>(_mapper.ConfigurationProvider)
                .ToListAsync();

            // Bổ sung logic AvailableVehicleCount (AutoMapper không xử lý được)
            foreach (var model in models)
            {
                var entity = await _dbContext.VehicleModels
                    .Include(vm => vm.Vehicles)
                        .ThenInclude(v => v.RentalContracts)
                    .FirstAsync(vm => vm.Id == model.Id);

                model.AvailableVehicleCount = entity.Vehicles.Count(v =>
                    v.StationId == stationId &&
                    (
                        v.Status == (int)VehicleStatus.Available ||
                        (
                            (v.Status == (int)VehicleStatus.Unavaible || v.Status == (int)VehicleStatus.Rented) &&
                            !v.RentalContracts
                                .Where(rc => rc.Status == (int)RentalContractStatus.Active)
                                .Any(rc =>
                                    startBuffer < rc.EndDate &&
                                    endBuffer > rc.StartDate
                                )
                        )
                    ));
            }

            return models;
        }

        public async Task<VehicleModelViewRes?> GetByIdAsync(
            Guid id,
            Guid stationId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            var startBuffer = startDate.AddDays(-10);
            var endBuffer = endDate.AddDays(10);

            // Load model + ảnh + brand + segment
            var entity = await _dbContext.VehicleModels
                .Include(vm => vm.ModelImages)
                .Include(vm => vm.Vehicles)
                    .ThenInclude(v => v.RentalContracts)
                .Include(vm => vm.Brand)
                .Include(vm => vm.Segment)
                .FirstOrDefaultAsync(vm => vm.Id == id && vm.DeletedAt == null);

            if (entity == null) return null;

            var model = _mapper.Map<VehicleModelViewRes>(entity);

            model.AvailableVehicleCount = entity.Vehicles.Count(v =>
                v.StationId == stationId &&
                (
                    v.Status == (int)VehicleStatus.Available ||
                    (
                        (v.Status == (int)VehicleStatus.Unavaible || v.Status == (int)VehicleStatus.Rented) &&
                        !v.RentalContracts
                            .Where(rc => rc.Status == (int)RentalContractStatus.Active)
                            .Any(rc =>
                                startBuffer < rc.EndDate &&
                                endBuffer > rc.StartDate
                            )
                    )
                ));

            return model;
        }
    }
}