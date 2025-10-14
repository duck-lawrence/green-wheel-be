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
            if(req.ContractId != null)
            {
                return await CreateVehicleChecklistInSideContract(Guid.Parse(staffId), (Guid)req.ContractId, req.Type);
            }
            else if (req.VehicleId != null)
            {
                return await CreateVehicleChecklistOutSideContract(Guid.Parse(staffId), (Guid)req.VehicleId, req.Type);
            }
            return null;
        }
        private async Task<VehicleChecklistViewRes> CreateVehicleChecklistOutSideContract(Guid staffId, Guid vehicleId, int type)
        {
           
            var components = await _uow.VehicleRepository.GetVehicleComponentsAsync(vehicleId);
            if (components == null)
            {
                throw new NotFoundException(Message.VehicleComponentMessage.ComponentNotFound);
            }
            Guid checkListId = Guid.NewGuid(); 
            var checklist = new VehicleChecklist()
            {
                Id = checkListId,
                IsSignedByCustomer = false,
                IsSignedByStaff = false,
                StaffId = staffId,
                VehicleId = vehicleId,
                Type = type
            };
            await _uow.VehicleChecklistRepository.AddAsync(checklist);
            var checklistItems = new List<VehicleChecklistItem>();
            foreach (var component in components)
            {
                Guid checkListItemId = Guid.NewGuid();
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

        private async Task<VehicleChecklistViewRes> CreateVehicleChecklistInSideContract(Guid staffId, Guid contractId, int type)
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
            Guid checkListId = Guid.NewGuid();
            
            var checklist = new VehicleChecklist()
            {
                Id = checkListId,
                IsSignedByCustomer = false,
                IsSignedByStaff = false,
                StaffId = staffId,
                CustomerId = contract.CustomerId,
                VehicleId = (Guid)contract.VehicleId,
                ContractId = contractId,
                Type = type
            };
            await _uow.VehicleChecklistRepository.AddAsync(checklist);
            var checklistItems = new List<VehicleChecklistItem>();
            foreach (var component in components)
            {
                checklistItems.Add(new VehicleChecklistItem()
                {
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

            var checklist = await _uow.VehicleChecklistRepository.GetByIdAsync(req.VehicleChecklistId);
            if (checklist == null)
                throw new NotFoundException(Message.VehicleChecklistMessage.VehicleChecklistNotFound);
            var contract = await _uow.VehicleChecklistRepository.GetRentalContractByCheckListIdAsync(req.VehicleChecklistId);
            if(contract == null)
            {
                await UpdateVehicleChecklistOutSideContractAsync(checklist, req.ChecklistItems);
            }
            else
            {
                await UpdateVehicleChecklistInsideContractAsync(checklist, req.ChecklistItems, contract, req.ReturnInvoiceId);
            }
            checklist.IsSignedByStaff = req.IsSignedByStaff;
            checklist.IsSignedByCustomer = req.IsSignedByCustomer;

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
            IEnumerable<UpdateChecklistItemReq> checklistReq, RentalContract contract, Guid? returnInvoiceId)
        {
            Invoice returnInvoice = null;
            if(checklist.Type == (int)VehicleChecklistType.Return)
            {
                returnInvoice = await _uow.InvoiceRepository.GetByIdAsync((Guid)returnInvoiceId!);
                if(returnInvoice == null)
                {
                    throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
                }
            }
            IEnumerable<InvoiceItem> invoiceItems = [];
            foreach (var itemReq in checklistReq)
            {
                var existingItem = checklist.VehicleChecklistItems
                    .FirstOrDefault(i => i.Id == itemReq.Id);

                if (existingItem == null) continue;

                if(itemReq.Status != (int)DamageStatus.Good && checklist.Type == (int)VehicleChecklistType.Return)
                {
                    Guid invoiceItemId = Guid.NewGuid();
                    var invoiceItem = new InvoiceItem()
                    {
                        Id = invoiceItemId,
                        InvoiceId = (Guid)returnInvoiceId!,
                        Quantity = 1,
                        UnitPrice = DamageCompensationHelper.CalculateCompensation(existingItem.Component.DamageFee, itemReq.Status),
                        Type = (int)InvoiceItemType.Damage,
                    };
                    invoiceItems = invoiceItems.Append(invoiceItem);
                }

                existingItem.Status = itemReq.Status;
                if (itemReq.Notes != null)
                {
                    existingItem.Notes = itemReq.Notes;
                }
            }
            if (invoiceItems != null)
            {
                //nếu vô đc trong này thì chắc chắn đã lấy đc return Invoice ở trên rồi
                returnInvoice!.Subtotal = returnInvoice.Subtotal + InvoiceHelper.CalculateSubTotalAmount(invoiceItems);
                await _uow.InvoiceRepository.AddAsync(returnInvoice);
                await _uow.InvoiceItemRepository.AddRangeAsync(invoiceItems);
            }
        }
    }
}