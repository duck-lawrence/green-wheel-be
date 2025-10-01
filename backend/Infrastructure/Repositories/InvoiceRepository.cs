using Application.Dtos.Common.Request;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<Invoice>> GetByContractAsync(Guid ContractId)
        {
            throw new NotImplementedException();
        }
    }
}
