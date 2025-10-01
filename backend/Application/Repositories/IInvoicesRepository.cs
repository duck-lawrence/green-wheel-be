using Application.Dtos.Common.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IInvoicesRepository : IGenericRepository<Invoice>
    {
         Task<PagedResult<Invoice>> GetInvoiceAsync(PaginationParams pagination);
    }
}
