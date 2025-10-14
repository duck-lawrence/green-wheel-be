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
            return await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.LicensePlate == licensePlate);
        }

        public async Task<IEnumerable<VehicleComponent>> GetVehicleComponentsAsync(Guid vehicleId)
        {
            var components = await _dbContext.Vehicles
                .Where(v => v.Id == vehicleId)
                .SelectMany(v => v.Model.ModelComponents.Select(mc => mc.Component))
                .ToListAsync();
            //lấy ra list linh kiện của xe
            return components;
        }

        public async Task<IEnumerable<Vehicle>?> GetVehicles(Guid stationId, Guid modelId)
        {
            // Query lọc trực tiếp từ DB (không ToList trước)
            var vehicles = await _dbContext.Vehicles
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

        
    }
}
