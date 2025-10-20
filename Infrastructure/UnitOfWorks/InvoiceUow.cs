using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class InvoiceUow : IInvoiceUow
    {
        private readonly IGreenWheelDbContext _context;
        public IMomoPaymentLinkRepository MomoPaymentLinkRepository { get ; set ; }
        public IInvoiceRepository InvoiceRepository { get ; set ; }
        public IInvoiceItemRepository InvoiceItemRepository { get; set; }
        public IRentalContractRepository RentalContractRepository { get; set; }
        public IDepositRepository DepositRepository { get; set; }

        public InvoiceUow(IGreenWheelDbContext context, 
            IMomoPaymentLinkRepository momoPaymentLink,
            IInvoiceRepository invoiceRepository,
            IRentalContractRepository rentalContractRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IDepositRepository depositRepository)
            {
                _context = context;
                MomoPaymentLinkRepository = momoPaymentLink;
                InvoiceRepository = invoiceRepository;
                RentalContractRepository = rentalContractRepository;
                InvoiceItemRepository = invoiceItemRepository;
                DepositRepository = depositRepository;
            }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (_context is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
