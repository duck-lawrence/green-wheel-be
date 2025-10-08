using Application.Constants;
using Application.Dtos.Brand.Respone;
using Application.Dtos.VehicleModel.Respone;
using Application.Dtos.VehicleSegment.Respone;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VehicleModelRepository : GenericRepository<VehicleModel>, IVehicleModelRepository
    {
        private IMapper _mapper;
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

            var query = _dbContext.VehicleModels.Where(vm => vm.DeletedAt == null)
                .Include(vm => vm.Vehicles)
                    .ThenInclude(v => v.RentalContracts)
                .AsQueryable();

            // filter theo model
            if (segmentId.HasValue)
            {
                query = query.Where(vm => vm.SegmentId == segmentId.Value);
            }
           
            
            // lọc vehicles trong station
            var result = await query
                .Select(vm => new VehicleModelViewRes
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    Description = vm.Description,
                    CostPerDay = vm.CostPerDay,
                    DepositFee = vm.DepositFee,
                    SeatingCapacity = vm.SeatingCapacity,
                    NumberOfAirbags = vm.NumberOfAirbags,
                    MotorPower = vm.MotorPower,
                    BatteryCapacity = vm.BatteryCapacity,
                    EcoRangeKm = vm.EcoRangeKm,
                    SportRangeKm = vm.SportRangeKm,
                    Brand = _mapper.Map<BrandViewRes>(vm.Brand),
                    Segment = _mapper.Map<VehicleSegmentViewRes>(vm.Segment),
                    ImageUrl = vm.ImageUrl,
                    AvailableVehicleCount = vm.Vehicles.Count(v =>
                    v.StationId == stationId &&
                    (
                        v.Status == (int)VehicleStatus.Available ||
                        (
                            (v.Status == (int)VehicleStatus.Unavaible || v.Status == (int)VehicleStatus.Rented) &&
                            !v.RentalContracts.Any(rc =>
                                endDate.AddDays(10) > rc.StartDate && startDate.AddDays(-10) < rc.EndDate
                            )
                        )
                    )
                )
                })
                .ToListAsync();
            return result;
        }

        public async Task<VehicleModelViewRes> GetByIdAsync(Guid id, Guid stationId, DateTimeOffset startDate,
        DateTimeOffset endDate)
        {
            var query = _dbContext.VehicleModels.Where(vm => vm.Id == id)
                .Include(vm => vm.ModelImages)
                .Include(vm => vm.Segment)
                .Include(vm => vm.Vehicles)
                    .ThenInclude(v => v.RentalContracts)
                .AsQueryable();
            var result = await query
                .Select(vm => new VehicleModelViewRes
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    Description = vm.Description,
                    CostPerDay = vm.CostPerDay,
                    DepositFee = vm.DepositFee,
                    SeatingCapacity = vm.SeatingCapacity,
                    NumberOfAirbags = vm.NumberOfAirbags,
                    MotorPower = vm.MotorPower,
                    BatteryCapacity = vm.BatteryCapacity,
                    EcoRangeKm = vm.EcoRangeKm,
                    SportRangeKm = vm.SportRangeKm,
                    Brand = _mapper.Map<BrandViewRes>(vm.Brand),
                    Segment = _mapper.Map<VehicleSegmentViewRes>(vm.Segment),
                    ImageUrl = vm.ImageUrl,
                    ImageUrls = vm.ModelImages.Select(x => x.Url),
                    AvailableVehicleCount = vm.Vehicles.Count(v =>
                    v.StationId == stationId &&
                    (
                        v.Status == (int)VehicleStatus.Available ||
                        (
                            (v.Status == (int)VehicleStatus.Unavaible || v.Status == (int)VehicleStatus.Rented) &&
                            !v.RentalContracts.Any(rc =>
                                endDate.AddDays(10) > rc.StartDate && startDate.AddDays(-10) < rc.EndDate
                            )
                        )
                    )
                )
                })
                .FirstOrDefaultAsync();
            return result;

        }
    }
}
