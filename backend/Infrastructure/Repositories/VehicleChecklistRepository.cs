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
    public class VehicleChecklistRepository : GenericRepository<VehicleChecklist>, IVehicleCheckListRepository
    {
        
        public VehicleChecklistRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }
    }
}
