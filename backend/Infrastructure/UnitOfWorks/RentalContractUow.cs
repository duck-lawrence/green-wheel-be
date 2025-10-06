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

        public IVehicleRepository VehicleRepository { get; }
        public IRentalContractRepository RentalContractRepository { get; }
        public IUserRepository UserRepository { get; }
        public IStationRepository StationRepository { get; }
        public IVehicleModelRepository VehicleModelRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IInvoiceItemRepository InvoiceItemRepository { get; }
        public IDepositRepository DepositRepository { get; }
        public ICitizenIdentityRepository CitizenIdentityRepository { get; }
        public IDriverLicenseRepository DriverLicenseRepository { get; }
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
        ICitizenIdentityRepository citizenIdentityRepository,
        IDriverLicenseRepository driverLicenseRepository)
        { 
            _context = context;
            VehicleRepository = vehicleRepository;
            RentalContractRepository = rentalContractRepository;
            UserRepository = userRepository;
            VehicleModelRepository = vehicleModelRepository;
            InvoiceRepository = invoiceRepository;
            DepositRepository = depositRepository;
            StationRepository = stationRepository;
            InvoiceItemRepository = invoiceItemRepository;
            CitizenIdentityRepository = citizenIdentityRepository;
            DriverLicenseRepository = driverLicenseRepository;
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
