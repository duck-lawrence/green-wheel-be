using Application.Repositories;
using Application.UnitOfWorks;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class RentalContractUow : IRentalContractUow
    {
        private readonly IGreenWheelDbContext _context;

        public IVehicleRepository Vehicles { get; }
        public IRentalContractRepository RentalContracts { get; }
        public IUserRepository Users { get; }
        public IStationRepository Stations { get; }
        public IVehicleModelRepository VehicleModels { get; }
        public IInvoiceRepository Invoices { get; }
        public IInvoiceItemRepository InvoiceItems { get; }
        public IDepositRepository Deposits { get; }
        public IModelImageRepository ModelImages { get; }

        public RentalContractUow(
        IGreenWheelDbContext context,
        IVehicleRepository vehicleRepository,
        IRentalContractRepository rentalContractRepository,
        IUserRepository userRepository,
        IVehicleModelRepository vehicleModelRepository,
        IInvoiceRepository invoiceRepository,
        IInvoiceItemRepository invoiceItemRepository,
        IDepositRepository depositRepository,
        IStationRepository stationRepository,
        IModelImageRepository modelImageRepository)
        {
            _context = context;
            Vehicles = vehicleRepository;
            RentalContracts = rentalContractRepository;
            Users = userRepository;
            VehicleModels = vehicleModelRepository;
            Invoices = invoiceRepository;
            Deposits = depositRepository;
            Stations = stationRepository;
            InvoiceItems = invoiceItemRepository;
            ModelImages = modelImageRepository;
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