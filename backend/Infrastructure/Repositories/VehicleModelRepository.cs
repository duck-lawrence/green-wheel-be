using Application.Constants;
using Application.Dtos.VehicleModel.Respone;
using Application.Repositories;
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
        public VehicleModelRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {

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
                .Include(vm => vm.ModelImages)
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
                    Brand = vm.Brand,
                    Segment = vm.Segment,
                    ImageUrl = vm.ImageUrl,
                    AvailableVehicleCount = vm.Vehicles.Count(v =>
                        v.StationId == stationId &&
                        (
                            v.Status == (int)VehicleStatus.Available // Available
                            ||
                            (v.Status == (int)VehicleStatus.Unavaible && endDate < v.RentalContracts.Min(rc => rc.StartDate).AddDays(-10)) // Unavailable
                                                                                                                                           //nếu có người thuê rồi thì 
                                                                                                                                           //ngày kết thúc phải sớm hơn 10 ngày
                            ||
                            (v.Status == (int)VehicleStatus.Rented && startDate > v.RentalContracts.Max(rc => rc.EndDate).AddDays(10)) // Rented
                                                                                                                                       //nếu có người đang thuê thì 
                                                                                                                                       //ngày bắt đầu phải muộn hơn ngày kết thúc của người trước 10 ngày
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
                    Brand = vm.Brand,
                    Segment = vm.Segment,
                    ImageUrl = vm.ImageUrl,
                    ImageUrls = vm.ModelImages.Select(x => x.Url),
                    AvailableVehicleCount = vm.Vehicles.Count(v =>
                        v.StationId == stationId &&
                        (
                            v.Status == (int)VehicleStatus.Available // Available
                            ||
                            (v.Status == (int)VehicleStatus.Unavaible && endDate < v.RentalContracts.Min(rc => rc.StartDate).AddDays(-10)) // Unavailable
                                                                                                                                           //nếu có người thuê rồi thì 
                                                                                                                                           //ngày kết thúc phải sớm hơn 10 ngày
                            ||
                            (v.Status == (int)VehicleStatus.Rented && startDate > v.RentalContracts.Max(rc => rc.EndDate).AddDays(10)) // Rented
                                                                                                                                       //nếu có người đang thuê thì 
                                                                                                                                       //ngày bắt đầu phải muộn hơn ngày kết thúc của người trước 10 ngày
                        )
                    )
                })
                .FirstOrDefaultAsync();
            return result;

        }
    }
}
