using Application.Repositories;

namespace Application.UnitOfWorks
{
    public interface IRentalContractUow : IDisposable
    {
        IVehicleRepository Vehicles { get; }
        IVehicleModelRepository VehicleModels { get; }
        IRentalContractRepository RentalContracts { get; }
        IUserRepository Users { get; }
        IInvoiceRepository Invoices { get; }
        IInvoiceItemRepository InvoiceItems { get; }
        IDepositRepository Deposits { get; }
        IStationRepository Stations { get; }
        IVehicleImageRepository VehicleImages { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}