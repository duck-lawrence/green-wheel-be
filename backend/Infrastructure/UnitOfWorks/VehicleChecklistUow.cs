using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class VehicleChecklistUow : IVehicleChecklistUow
    {
        private readonly IGreenWheelDbContext _context;
        public IVehicleChecklistItemRepository VehicleChecklistItemRepository { get; }
        public IVehicleCheckListRepository VehicleChecklistRepository { get; }
        public IVehicleRepository VehicleRepository { get; }
        public IRentalContractRepository RentalContractRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IInvoiceItemRepository InvoiceItemRepository { get; }

        public VehicleChecklistUow(IGreenWheelDbContext context,
            IVehicleChecklistItemRepository vehicleChecklistItemRepository,
            IVehicleCheckListRepository vehicleCheckListRepository,
            IVehicleRepository vehicleRepository,
            IRentalContractRepository rentalContractRepository,
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository)
        {
            _context = context;
            VehicleChecklistItemRepository = vehicleChecklistItemRepository;
            VehicleChecklistRepository = vehicleCheckListRepository;
            VehicleRepository = vehicleRepository;
            RentalContractRepository = rentalContractRepository;
            InvoiceRepository = invoiceRepository;
            InvoiceItemRepository = invoiceItemRepository;
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
