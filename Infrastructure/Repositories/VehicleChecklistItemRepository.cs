using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VehicleChecklistItemRepository : GenericRepository<VehicleChecklistItem>, IVehicleChecklistItemRepository
    {
        public VehicleChecklistItemRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }
    }
}
