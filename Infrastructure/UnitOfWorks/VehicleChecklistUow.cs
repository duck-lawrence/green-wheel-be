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
    public class VehicleChecklistUow : UnitOfwork, IVehicleChecklistUow
    {
        public IVehicleChecklistItemRepository VehicleChecklistItemRepository { get; }
        public IVehicleCheckListRepository VehicleChecklistRepository { get; }
        public IVehicleRepository VehicleRepository { get; }
        public IRentalContractRepository RentalContractRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IInvoiceItemRepository InvoiceItemRepository { get; }
        public IVehicleComponentRepository VehicleComponentRepository { get; }
        public VehicleChecklistUow(IGreenWheelDbContext context,
            IVehicleChecklistItemRepository vehicleChecklistItemRepository,
            IVehicleCheckListRepository vehicleCheckListRepository,
            IVehicleRepository vehicleRepository,
            IRentalContractRepository rentalContractRepository,
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IVehicleComponentRepository vehicleComponentRepository) : base(context)
        {
            VehicleChecklistItemRepository = vehicleChecklistItemRepository;
            VehicleChecklistRepository = vehicleCheckListRepository;
            VehicleRepository = vehicleRepository;
            RentalContractRepository = rentalContractRepository;
            InvoiceRepository = invoiceRepository;
            InvoiceItemRepository = invoiceItemRepository;
            VehicleComponentRepository = vehicleComponentRepository;
        }
    }
}