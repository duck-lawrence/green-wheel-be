using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
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
        public async override Task<VehicleChecklist?> GetByIdAsync(Guid id)
        {
            var vehicleChecklist = await _dbContext.VehicleChecklists.Where(vc => vc.Id == id)
                .Include(vc => vc.VehicleChecklistItems)
                    .ThenInclude(vci => vci.Component).FirstOrDefaultAsync();
            return vehicleChecklist;
        }

        public async Task<RentalContract?> GetRentalContractByCheckListIdAsync(Guid id)
        {
            var vehicleChecklist = (await _dbContext.VehicleChecklists.Where(vc => vc.Id == id)
                .Include(vc => vc.Contract).FirstOrDefaultAsync());
            
            return vehicleChecklist == null ? null : vehicleChecklist.Contract;
        }
    }
}
