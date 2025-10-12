using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IInvoiceItemRepository : IGenericRepository<InvoiceItem>
    {
        Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId);
    }
}
