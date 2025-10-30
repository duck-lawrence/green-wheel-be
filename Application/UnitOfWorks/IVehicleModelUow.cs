using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitOfWorks
{
    public interface IVehicleModelUow : IUnitOfwork
    {
        IVehicleRepository VehicleRepository { get; }
        IVehicleModelRepository VehicleModelRepository { get; }
        IModelComponentRepository ModelComponentRepository { get; }
        IVehicleComponentRepository VehicleComponentRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
