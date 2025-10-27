using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;

namespace Infrastructure.Repositories
{
    public class VehicleSegmentRepository : GenericRepository<VehicleSegment>, IVehicleSegmentRepository
    {
        public VehicleSegmentRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

    }
}
