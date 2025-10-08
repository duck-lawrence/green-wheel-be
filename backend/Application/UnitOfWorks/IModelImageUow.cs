using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;

namespace Application.UnitOfWorks
{
    public interface IModelImageUow : IDisposable
    {
        IModelImageRepository ModelImageRepository { get; }
        IVehicleModelRepository VehicleModelRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}