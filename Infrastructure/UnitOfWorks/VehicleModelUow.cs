using Application.Repositories;
using Application.UnitOfWorks;
using Infrastructure.ApplicationDbContext;

namespace Infrastructure.UnitOfWorks
{
    public class VehicleModelUow : UnitOfwork, IVehicleModelUow
    {
        public IVehicleRepository VehicleRepository { get; }

        public IVehicleModelRepository VehicleModelRepository { get; }

        public IModelComponentRepository ModelComponentRepository { get; }

        public IVehicleComponentRepository VehicleComponentRepository { get; }

        public VehicleModelUow(IGreenWheelDbContext context,
            IVehicleRepository vehicleRepository,
            IVehicleModelRepository vehicleModelRepositor,
            IModelComponentRepository modelComponentRepository,
            IVehicleComponentRepository vehicleComponentRepository) : base(context)
        {
            VehicleRepository = vehicleRepository;
            VehicleModelRepository = vehicleModelRepositor;
            ModelComponentRepository = modelComponentRepository;
            VehicleComponentRepository = vehicleComponentRepository;
        }
    }
}
