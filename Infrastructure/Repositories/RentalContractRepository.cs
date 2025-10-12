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
                .Include(r => r.Invoices)
                .ToListAsync();
            if (status != null)
            {
                contracts = (List<RentalContract>)contracts.Where(c => c.Status == status);
            }
            return contracts;
        }

        public async Task<IEnumerable<RentalContract>> GetAllAsync(int? status = null, string? phone = null)
        {
            var rentalContracts = await _dbContext.RentalContracts
                .Include(x => x.Invoices)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.CitizenIdentity)
                .Include(x => x.Customer)
                    .ThenInclude(u => u.DriverLicense)
                .ToListAsync();
            if (!string.IsNullOrEmpty(phone) && status != null)
            {
                rentalContracts = rentalContracts.Where(rc => rc.Status == status && rc.Customer.Phone == phone).ToList();
            }else if (!string.IsNullOrEmpty(phone))
            {
                rentalContracts = rentalContracts.Where(rc => rc.Customer.Phone == phone).ToList();
            }else if(status != null)
            {
                rentalContracts = rentalContracts.Where(rc => rc.Status == status).ToList();
            }
            return rentalContracts;
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
                .Include(r => r.Invoices).FirstOrDefaultAsync();
        }

        
    }
}
