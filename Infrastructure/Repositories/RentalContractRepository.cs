using Application.Constants;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RentalContractRepository : GenericRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<RentalContract>> GetByCustomerAsync(Guid customerId, int? status)
        {
            var contracts = _dbContext.RentalContracts.Where(r => r.CustomerId == customerId)
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(x => x.Station)
                // .Include(x => x.Invoices)
                //     .ThenInclude(i => i.InvoiceItems)
                // .Include(x => x.Invoices)
                //     .ThenInclude(i => i.Deposit)
                .AsQueryable();
            if (status != null)
            {
                contracts = contracts.Where(c => c.Status == status);
            }
            return await contracts.ToListAsync();
        }

        public async Task<IEnumerable<RentalContract>> GetAllAsync(int? status = null, string? phone = null,
            string? citizenIdentityNumber = null, string? driverLicenseNumber = null, Guid? checklistId = null)
        {
            var rentalContracts = _dbContext.RentalContracts
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(x => x.Station)
                .Include(x => x.HandoverStaff)
                    .ThenInclude(h => h.User)
                .Include(x => x.ReturnStaff)
                    .ThenInclude(h => h.User)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.CitizenIdentity)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.DriverLicense)
                .AsQueryable();
            if (!string.IsNullOrEmpty(phone))
            {
                rentalContracts = rentalContracts.Where(rc => rc.Customer.Phone == phone);
            }
            if (status != null)
            {
                rentalContracts = rentalContracts.Where(rc => rc.Status == status);
            }
            if (!string.IsNullOrEmpty(citizenIdentityNumber))
            {
                rentalContracts = rentalContracts.Where(rc => rc.Customer.CitizenIdentity.Number == citizenIdentityNumber);
            }
            if (!string.IsNullOrEmpty(driverLicenseNumber))
            {
                rentalContracts = rentalContracts.Where(rc => rc.Customer.DriverLicense.Number == driverLicenseNumber);
            }
            if(checklistId != null)
            {
                rentalContracts = rentalContracts.Where(rc => rc.VehicleChecklists.Any(vc => vc.Id  == checklistId));
            }
            return await rentalContracts.ToListAsync();
        }

        public async Task<bool> HasActiveContractAsync(Guid customerId)
        {
            return await (_dbContext.RentalContracts.Where(r => r.CustomerId == customerId
            && r.Status != (int)RentalContractStatus.Completed
            && r.Status != (int)RentalContractStatus.Cancelled).FirstOrDefaultAsync()) != null;
        }

        public override async Task<RentalContract?> GetByIdAsync(Guid id)
        {
            return await _dbContext.RentalContracts.Where(r => r.Id == id)
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(x => x.Station)
                .Include(x => x.Invoices)
                    .ThenInclude(i => i.InvoiceItems)
                .Include(x => x.Invoices)
                    .ThenInclude(i => i.Deposit)
                .Include(x => x.HandoverStaff)
                    .ThenInclude(h => h.User)
                .Include(x => x.ReturnStaff)
                    .ThenInclude(h => h.User)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.CitizenIdentity)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.DriverLicense)
                .FirstOrDefaultAsync();
        }

        public async Task<RentalContract?> GetByChecklistIdAsync(Guid id)
        {
            var vehicleChecklist = (await _dbContext.VehicleChecklists.Where(vc => vc.Id == id)
                .Include(vc => vc.Contract)
                    .ThenInclude(r => r.Invoices).FirstOrDefaultAsync());

            return vehicleChecklist == null ? null : vehicleChecklist.Contract;
        }
    }
}