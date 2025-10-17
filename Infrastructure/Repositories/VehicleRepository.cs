using Application.AppExceptions;
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
            return await _dbContext.Vehicles
                .Include(v => v.Model)
                .FirstOrDefaultAsync(x => x.LicensePlate == licensePlate);
        }

       public async Task<IEnumerable<Vehicle>> GetAllAsync(string? name, Guid? stationId, int? status, string? licensePlate)
        {
            var vehicles = _dbContext.Vehicles
                            .Include(v => v.Model)
                            .AsQueryable();
            if (!string.IsNullOrEmpty(name)) vehicles = vehicles.Where(v => v.Model.Name.ToLower().Contains(name.ToLower()));
            if (stationId != null) vehicles = vehicles.Where(v => v.StationId == stationId);
            if (status != null) vehicles = vehicles.Where(v => v.Status == status);
            if (!string.IsNullOrEmpty(licensePlate)) vehicles = vehicles.Where(v => v.LicensePlate.ToLower().Contains(licensePlate.ToLower()));
            return await vehicles.ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>?> GetVehicles (Guid stationId, Guid modelId)
        {
            // Query lọc trực tiếp từ DB (không ToList trước)
            var vehicles = await _dbContext.Vehicles
                .Include(v => v.Model)
                .Include(v => v.RentalContracts) //join bảng rentalContracts để lấy xe có hợp đồng
                .Where
                (
                    v => v.StationId == stationId
                        && v.ModelId == modelId
                        && v.Status != (int)VehicleStatus.Maintenance
                ).AsNoTracking().ToListAsync();

            foreach (var v in vehicles)
            {
                v.RentalContracts = v.RentalContracts.Where(rc => rc.Status != (int)RentalContractStatus.Cancelled &&
                                              rc.Status != (int)RentalContractStatus.Completed).ToList();
            }
            return vehicles;
        }

        public async Task<Vehicle?> GetByIdOptionAsync(Guid id, bool includeModel = false)
        {
            IQueryable<Vehicle> query = _dbContext.Vehicles.AsQueryable();
            if (includeModel)
            {
                query = query.Include(i => i.Model);
            }
            return await query.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> CountVehiclesInStationAsync(Guid[] vehicleIds, Guid stationId)
        {
            return await _dbContext.Vehicles.CountAsync(x => vehicleIds.Contains(x.Id) && x.StationId == stationId);
        }

        public async Task UpdateStationForDispatchAsync(Guid dispatchId, Guid toStationId)
        {
            var vehicleIds = await _dbContext.DispatchRequestVehicles
                .Where(x => x.DispatchRequestId == dispatchId)
                .Select(x => x.VehicleId)
                .ToListAsync();

            if (vehicleIds.Count == 0)
                return;

            var vehicles = await _dbContext.Vehicles
                .Where(v => vehicleIds.Contains(v.Id))
                .ToListAsync();

            foreach (var v in vehicles)
            {
                v.StationId = toStationId;
                v.UpdatedAt = DateTimeOffset.UtcNow;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}