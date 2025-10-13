using Application.Constants;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RentalContractRepository : GenericRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<RentalContract>> GetByCustomerAsync(Guid customerId, int? status = null)
        {
            var contracts = await _dbContext.RentalContracts.Where(r => r.CustomerId == customerId)
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(x => x.Station)
                .Include(x => x.Invoices)
                .ToListAsync();
            if (status != null)
            {
                contracts = (List<RentalContract>)contracts.Where(c => c.Status == status);
            }
            return contracts;
        }

        public async Task<IEnumerable<RentalContract>> GetAllAsync(int? status = null, string? phone = null,
            string? citizenIdentityNumber = null, string? driverLicenseNumber = null)
        {
            var rentalContracts = _dbContext.RentalContracts
                .Include(x => x.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(x => x.Station)
                .Include(x => x.Invoices)
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
                .Include(x => x.Customer)
                    .ThenInclude(u => u.CitizenIdentity)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.DriverLicense)
                .FirstOrDefaultAsync();
                
        }

        
    }
}