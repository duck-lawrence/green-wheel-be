using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklist.Respone;
using Application.Dtos.VehicleChecklistItem.Request;
using Application.Helpers;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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

        

        public async Task<VehicleChecklistViewRes> CreateVehicleChecklist(ClaimsPrincipal userclaims, CreateVehicleChecklistReq req)
        {
            var staffId = userclaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            if (req.VehicleId != null)
            {
                return await CreateVehicleChecklistOutSideContract(Guid.Parse(staffId), (Guid)req.VehicleId);
            }else if(req.ContractId != null)
            {
                return await CreateVehicleChecklistInSideContract(Guid.Parse(staffId), (Guid)req.ContractId);
            }
            return null;
        }
        private async Task<VehicleChecklistViewRes> CreateVehicleChecklistOutSideContract(Guid staffId, Guid vehicleId)
        {
           
            var components = await _uow.VehicleRepository.GetVehicleComponentsAsync(vehicleId);
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
                StaffId = staffId,
                VehicleId = vehicleId,
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

        private async Task<VehicleChecklistViewRes> CreateVehicleChecklistInSideContract(Guid staffId, Guid contractId)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(contractId);
            if(contract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            var components = await _uow.VehicleRepository.GetVehicleComponentsAsync((Guid)contract.VehicleId);
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
                StaffId = staffId,
                CustomerId = contract.CustomerId,
                VehicleId = (Guid)contract.VehicleId,
                ContractId = contractId,
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
            var contract = await _uow.VehicleChecklistRepository.GetRentalContractByCheckListIdAsync(req.VehicleChecklistId);
            if(contract == null)
            {
                await UpdateVehicleChecklistOutSideContractAsync(checklist, req.ChecklistItems);
            }
            else
            {
                await UpdateVehicleChecklistInsideContractAsync(checklist, req.ChecklistItems, contract);
            }

            await _uow.SaveChangesAsync();
        }

        private async Task UpdateVehicleChecklistOutSideContractAsync(VehicleChecklist checklist, 
            IEnumerable<UpdateChecklistItemReq> checklictReq)
        {
            foreach (var itemReq in checklictReq)
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
            
        }

        private async Task UpdateVehicleChecklistInsideContractAsync(VehicleChecklist checklist, 
            IEnumerable<UpdateChecklistItemReq> checklictReq, RentalContract contract)
        {
            Guid invoiceId;
            do
            {
                invoiceId = Guid.NewGuid();

            } while (await _uow.InvoiceRepository.GetByIdOptionAsync(invoiceId) != null);
            var invoice = new Invoice()
            {
                Id = invoiceId,
                ContractId = contract.Id,
                Status = (int)InvoiceStatus.Pending,
                Tax = Common.Tax.OtherVAT,
                Notes = $"GreenWheel – Invoice for your order {contract.Id}"
            };
            IEnumerable<InvoiceItem> invoieItems = [];
            foreach (var itemReq in checklictReq)
            {
                var existingItem = checklist.VehicleChecklistItems
                    .FirstOrDefault(i => i.Id == itemReq.Id);

                if (existingItem == null) continue;

                if(itemReq.Status != (int)DamageStatus.Good)
                {
                    Guid invoiceItemId;
                    do
                    {
                        invoiceItemId = Guid.NewGuid();

                    } while ((await _uow.InvoiceItemRepository.GetByIdAsync(invoiceItemId)) != null);
                    var invoiceItem = new InvoiceItem()
                    {
                        Id = invoiceItemId,
                        InvoiceId = invoiceId,
                        Quantity = 1,
                        UnitPrice = DamageCompensationHelper.CalculateCompensation(1000, itemReq.Status),
                        Type = (int)InvoiceItemType.Damage,
                    };
                    invoieItems.Append(invoiceItem);
                }

                existingItem.Status = itemReq.Status;
                if (itemReq.Notes != null)
                {
                    existingItem.Notes = itemReq.Notes;
                }
            }
            if (!invoieItems.IsNullOrEmpty())
            {
                invoice.Subtotal = InvoiceHelper.CalculateTotalAmount(invoieItems);
                invoice.InvoiceType = (int)InvoiceType.Return;
                await _uow.InvoiceRepository.AddAsync(invoice);
                await _uow.InvoiceItemRepository.AddRangeAsync(invoieItems);
            }
            
        }
    }
}