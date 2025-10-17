using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitOfWorks
{
    public interface IVehicleChecklistUow : IDisposable
    {
        IVehicleChecklistItemRepository VehicleChecklistItemRepository { get; }
        IVehicleCheckListRepository VehicleChecklistRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IRentalContractRepository RentalContractRepository { get; }
        IInvoiceItemRepository InvoiceItemRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IVehicleComponentRepository VehicleComponentRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
