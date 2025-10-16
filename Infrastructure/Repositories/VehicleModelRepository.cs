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
        public async Task<IEnumerable<VehicleModel>> GetAllAsync(string? name, Guid? segmentId)
        {
            var models = _dbContext.VehicleModels
                            .Include(vm => vm.Brand)
                            .Include(vm => vm.Segment)
                            .Include(vm => vm.ModelImages)
                            .Include(vm => vm.Vehicles)
                            .AsQueryable();
            if (!string.IsNullOrEmpty(name)) models = models.Where(vm => vm.Name.ToLower().Contains(name.ToLower()));
            if (segmentId != null) models = models.Where(vm => vm.SegmentId == segmentId);
            return await models.ToListAsync();
        }

        public async Task<IEnumerable<VehicleModel>> FilterVehicleModelsAsync(
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
                .Include(vm => vm.Vehicles)
                    .ThenInclude(v => v.Station)
                .Include(vm => vm.Brand)
                .Include(vm => vm.Segment)
                .AsNoTracking()
                .AsQueryable();
            if (segmentId.HasValue)
                query = query.Where(vm => vm.SegmentId == segmentId.Value);
            var models = await query.ToListAsync();
            foreach (var model in models)
            {
                 var vehicles = model.Vehicles.Where(v =>
                    v.StationId == stationId && (
                        v.Status == (int)VehicleStatus.Available ||
                        (
                            (v.Status == (int)VehicleStatus.Unavaible || v.Status == (int)VehicleStatus.Rented) &&
                            !v.RentalContracts.Any(rc =>
                                rc.Status == (int)RentalContractStatus.Active &&
                                startBuffer <= rc.EndDate &&
                                endBuffer >= rc.StartDate
                            )
                        )
                    ));
                model.Vehicles = vehicles.ToList();
                
            }
            return models;
        }

        public async Task<VehicleModel?> GetByIdAsync(
            Guid id,
            Guid stationId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            var startBuffer = startDate.AddDays(-10);
            var endBuffer = endDate.AddDays(10);

            // Load model + ảnh + brand + segment
            var model = await _dbContext.VehicleModels
                .Include(vm => vm.ModelImages)
                .Include(vm => vm.Vehicles)
                    .ThenInclude(v => v.RentalContracts)
                .Include(vm => vm.Brand)
                .Include(vm => vm.Segment)
                .FirstOrDefaultAsync(vm => vm.Id == id && vm.DeletedAt == null);

            if (model == null) return null;

            var vehicles = model.Vehicles.Where(v => v.StationId == stationId && (
                                                v.Status == (int)VehicleStatus.Available ||
                                                (
                                                    (v.Status == (int)VehicleStatus.Unavaible || v.Status == (int)VehicleStatus.Rented) &&
                                                    !v.RentalContracts.Any(rc =>
                                                        rc.Status == (int)RentalContractStatus.Active &&
                                                        startBuffer <= rc.EndDate &&
                                                        endBuffer >= rc.StartDate
                                                    )
                                                )
                                            ));
            model.Vehicles = model.Vehicles = vehicles.ToList();

            return model;
        }
    }
}