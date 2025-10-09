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
    public class InvoiceItemRepository : GenericRepository<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddRangeAsync(IEnumerable<InvoiceItem> items)
        {
            foreach(var item in items)
            {
                await _dbContext.InvoiceItems.AddAsync(item);
            }
        }

        public async Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _dbContext.InvoiceItems
                .Include(i => i.ChecklistItem)
                    .ThenInclude(cli => cli.Component)
                .Where(i => i.InvoiceId == invoiceId).ToListAsync();
        }
    }
}
