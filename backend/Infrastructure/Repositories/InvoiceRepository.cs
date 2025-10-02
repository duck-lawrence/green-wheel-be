using Application.Dtos.Common.Request;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Invoice>> GetByContractAsync(Guid ContractId)
        {
            return await _dbContext.Invoices.Where(i => i.ContractId == ContractId).ToListAsync();
        }

        public async Task<Invoice?> GetByIdOptionAsync(Guid id, bool includeItems = false, bool includeDeposit = false)
        {
            IQueryable<Invoice> query = _dbContext.Invoices.AsQueryable();
            if (includeItems)
            {
                query = query.Include(i => i.InvoiceItems);
            }
            if (includeDeposit)
            {
                query = query.Include(i => i.Deposit);
            }
            return await query.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
