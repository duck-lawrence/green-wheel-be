using Application.Constants;
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
    public class DepositRepository : GenericRepository<Deposit>, IDepositRepository
    {
        public DepositRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Deposit?> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _dbContext.Deposits.FirstOrDefaultAsync(x => x.InvoiceId == invoiceId);
        }

        public async Task<Deposit> GetByContractIdAsync(Guid contractId)
        {
            return (await _dbContext.Deposits
                            .Include(d => d.Invoice)
                                .ThenInclude(i => i.Contract)
                                .Where(d => d.Invoice.Type == (int)InvoiceType.Handover
                                          && d.Invoice.ContractId == contractId).FirstOrDefaultAsync())!;
        }
    }
}
