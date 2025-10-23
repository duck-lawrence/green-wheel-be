using Application.Repositories;

namespace Application.UnitOfWorks
{
    public interface IRentalContractUow : IUnitOfwork
    {
        IVehicleRepository VehicleRepository { get; }
        IVehicleModelRepository VehicleModelRepository { get; }
        IRentalContractRepository RentalContractRepository { get; }
        IUserRepository UserRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IInvoiceItemRepository InvoiceItemRepository { get; }
        IDepositRepository DepositRepository { get; }
        IStationRepository StationRepository { get; }
        ICitizenIdentityRepository CitizenIdentityRepository { get; }
        IDriverLicenseRepository DriverLicenseRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}