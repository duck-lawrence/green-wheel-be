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
                    .ThenInclude(vci => vci.Component)
                .Include(vc => vc.Vehicle)
                .Include(vc => vc.Staff)
                    .ThenInclude(s => s.User)
                .Include(vc => vc.Customer)
                    .FirstOrDefaultAsync();
            return vehicleChecklist;
        }

        public async Task<IEnumerable<VehicleChecklist>?> GetAll(Guid? contractId)
        {
            var vehicleChecklists = await _dbContext.VehicleChecklists
                .Include(vc => vc.VehicleChecklistItems)
                    .ThenInclude(vci => vci.Component)
                .Include(vc => vc.Vehicle)
                .Include(vc => vc.Staff)
                    .ThenInclude(s => s.User)
                .Include(vc => vc.Customer)
                    .ToListAsync();
            if(id != null)
            {
                vehicleChecklists = (List<VehicleChecklist>)vehicleChecklists.Where(c => c.ContractId == id);
            }
            return vehicleChecklists;
        }


    }
}
