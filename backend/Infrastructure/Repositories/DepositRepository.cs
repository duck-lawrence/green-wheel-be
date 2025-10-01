using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
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

        public Task<Deposit?> GetByInvoiceIdAsync(Guid invoiceId)
        {
            throw new NotImplementedException();
        }
    }
}
