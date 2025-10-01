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
    public class InvoiceItemRepository : GenericRepository<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public Task AddRangeAsync(IEnumerable<InvoiceItem> items)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            throw new NotImplementedException();
        }
    }
}
