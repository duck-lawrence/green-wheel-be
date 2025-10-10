using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklist.Respone;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class VehicleChecklistService : IVehicleChecklistService
    {
        private readonly IVehicleChecklistUow _uow;
        private readonly IMapper _mapper;

        public VehicleChecklistService(IVehicleChecklistUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<VehicleChecklistViewRes> CreateVehicleChecklistAsync(ClaimsPrincipal userclaims, CreateVehicleChecklistReq req)
        {
            var staffId = userclaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            var components = await _uow.VehicleRepository.GetVehicleComponentsAsync(req.VehicleId);
            if (components == null)
            {
                throw new NotFoundException(Message.VehicleComponentMessage.ComponentNotFound);
            }
            Guid checkListId;
            do
            {
                checkListId = Guid.NewGuid();
            } while (await _uow.VehicleChecklistRepository.GetByIdAsync(checkListId) != null);
            var checklist = new VehicleChecklist()
            {
                Id = checkListId,
                IsSignedByCustomer = false,
                IsSignedByStaff = false,
                StaffId = Guid.Parse(staffId),
                CustomerId = req.CustomerId,
                VehicleId = req.VehicleId,
                ContractId = req.ContractId,
                DeletedAt = null
            };
            await _uow.VehicleChecklistRepository.AddAsync(checklist);
            var checklistItems = new List<VehicleChecklistItem>();
            foreach (var component in components)
            {
                Guid checkListItemId;
                do
                {
                    checkListItemId = Guid.NewGuid();
                } while (await _uow.VehicleChecklistItemRepository.GetByIdAsync(checkListItemId) != null);
                checklistItems.Add(new VehicleChecklistItem()
                {
                    Id = checkListItemId,
                    ComponentId = component.Id,
                    Component = component,
                    ChecklistId = checkListId
                });
            }
            await _uow.VehicleChecklistItemRepository.AddRangeAsync(checklistItems);
            await _uow.SaveChangesAsync();
            var checkListViewRes = _mapper.Map<VehicleChecklistViewRes>(checklist);
            return checkListViewRes;
        }

        public async Task<VehicleChecklistViewRes> GetByIdAsync(Guid id)
        {
            var vehicleChecklist = await _uow.VehicleChecklistRepository.GetByIdAsync(id);
            if (vehicleChecklist == null)
            {
                throw new NotFoundException(Message.VehicleChecklistMessage.VehicleChecklistNotFound);
            }
            var checklistViewRes = _mapper.Map<VehicleChecklistViewRes>(vehicleChecklist);
            return checklistViewRes;
        }

        public async Task UpdateVehicleChecklistAsync(UpdateVehicleChecklistReq req)
        {
            var checklists = await _uow.VehicleChecklistRepository.GetAllAsync(
                new Expression<Func<VehicleChecklist, object>>[]
                {
                    v => v.VehicleChecklistItems
                });
            var checklist = checklists.Where(c => c.Id == req.VehicleChecklistId).FirstOrDefault();
            if (checklist == null)
                throw new NotFoundException(Message.VehicleChecklistMessage.VehicleChecklistNotFound);
            foreach (var itemReq in req.ChecklistItems)
            {
                var existingItem = checklist.VehicleChecklistItems
                    .FirstOrDefault(i => i.Id == itemReq.Id);

                if (existingItem == null)
                    continue;

                existingItem.Status = itemReq.Status;
                if (itemReq.Notes != null)
                {
                    existingItem.Notes = itemReq.Notes;
                }
            }
            await _uow.SaveChangesAsync();
        }
    }
}