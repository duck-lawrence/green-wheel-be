using Application.Constants;
using Application.Dtos.VehicleModel.Respone;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
        {
            return await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.LicensePlate == licensePlate);
        }

        public async Task<Vehicle> GetVehicle(Guid stationId, Guid modelId,
            DateTimeOffset startDate, DateTimeOffset endDate)
        {
            // Query lọc trực tiếp từ DB (không ToList trước)
            var vehicles = await _dbContext.Vehicles
                .Include(v => v.RentalContracts)
                .Where(v => v.StationId == stationId && v.ModelId == modelId)
                .ToListAsync();

            // Lọc tiếp ở memory (sau khi có dữ liệu)
            var result = vehicles.FirstOrDefault(v =>
                v.Status == (int)VehicleStatus.Available
                || (v.Status == (int)VehicleStatus.Unavaible
                    && v.RentalContracts.Any() // tránh lỗi Min() khi rỗng
                    && endDate < v.RentalContracts.Min(rc => rc.StartDate).AddDays(-10))
                || (v.Status == (int)VehicleStatus.Rented
                    && v.RentalContracts.Any() // tránh lỗi Max() khi rỗng
                    && startDate > v.RentalContracts.Max(rc => rc.EndDate).AddDays(10))
            );
            return result;
        }
    }
}
