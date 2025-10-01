using Application.Repositories;
namespace Application.UnitOfWorks
{
    public interface IRentalContractUow : IDisposable
    {
            IVehicleRepository Vehicles { get; }
            IRentalContractRepository RentalContracts { get; }
            IUserRepository Users { get; }
    }
}
