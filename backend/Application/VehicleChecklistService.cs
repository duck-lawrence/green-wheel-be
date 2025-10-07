using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklist.Respone;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
            if(components == null)
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
            foreach(var component in components)
            {
                Guid checkListItemId;
                do
                {
                    checkListItemId = Guid.NewGuid();
                } while (await _uow.VehicleChecklistItemRepository.GetByIdAsync(checkListItemId) != null);
                var checklistItem = new VehicleChecklistItem()
                {
                    Id = checkListItemId,
                    ComponentId = component.Id,
                    Component = component,
                    ChecklistId = checkListId
                };
                //checklist.VehicleChecklistItems.Add(checklistItem);
                await _uow.VehicleChecklistItemRepository.AddAsync(checklistItem);
            }
            await _uow.SaveChangesAsync();
            var checkListViewRes = _mapper.Map<VehicleChecklistViewRes>(checklist);
            return checkListViewRes;

        }

       
    }
}
