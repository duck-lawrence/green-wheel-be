using Application.Dtos.Common.Request;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Application.Helpers;

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
                query = query.Include(i => i.InvoiceItems)
                    .ThenInclude(i => i.ChecklistItem)
                        .ThenInclude(cli => cli.Component);
            }
            if (includeDeposit)
            {
                query = query.Include(i => i.Deposit);
            }
            return await query.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination)
        {
            var query = _dbContext.Invoices
                .AsNoTracking()
                .OrderByDescending(i => i.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .ApplyPagination(pagination)
                .ToListAsync();

            return new PageResult<Invoice>(items, pagination.PageNumber, pagination.PageSize, totalCount);
        }
    }
}