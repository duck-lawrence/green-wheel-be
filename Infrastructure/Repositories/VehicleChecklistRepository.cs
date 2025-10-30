using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Helpers;
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

        public async Task<IEnumerable<VehicleChecklist>?> GetByContractAndType(Guid contractId, int type)
        {
            var vehicleChecklists = _dbContext.VehicleChecklists
                .Include(vc => vc.VehicleChecklistItems)
                    .ThenInclude(vci => vci.Component)
                .Include(vc => vc.Vehicle)
                .Include(vc => vc.Staff)
                    .ThenInclude(s => s.User)
                .Include(vc => vc.Customer)
                .OrderByDescending(x => x.CreatedAt)
                .Where(vc => vc.ContractId == contractId && vc.Type == type)
                    .AsQueryable();
            return await vehicleChecklists.ToListAsync();
        }

        public async Task<PageResult<VehicleChecklist>> GetAllPagination(Guid? contractId, int? type, PaginationParams pagination)
        {
            var vehicleChecklists = _dbContext.VehicleChecklists
                .Include(vc => vc.VehicleChecklistItems)
                    .ThenInclude(vci => vci.Component)
                .Include(vc => vc.Vehicle)
                .Include(vc => vc.Staff)
                    .ThenInclude(s => s.User)
                .Include(vc => vc.Customer)
                .OrderByDescending(x => x.CreatedAt)
                    .AsQueryable();
            if (contractId != null)
            {
                vehicleChecklists = vehicleChecklists.Where(c => c.ContractId == contractId);
            }
            if (type != null)
            {
                vehicleChecklists = vehicleChecklists.Where(c => c.Type == type);
            }

            var totalCount = await vehicleChecklists.CountAsync();
            var checklists = await vehicleChecklists.ApplyPagination(pagination).ToListAsync();
            return new PageResult<VehicleChecklist>(checklists, pagination.PageNumber, pagination.PageSize, totalCount);
        }


    }
}
