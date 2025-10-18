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
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application
{
    public class VehicleChecklistService : IVehicleChecklistService
    {
        private readonly IVehicleChecklistUow _uow;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepository;

        public VehicleChecklistService(IVehicleChecklistUow uow, IMapper mapper, IMemoryCache cache, IUserRepository userRepository)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
            _userRepository = userRepository;
        }

        

        public async Task<Guid> Create(ClaimsPrincipal userclaims, CreateVehicleChecklistReq req)
        {
            var staffId = userclaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString();
            if(req.Type != (int)VehicleChecklistType.OutOfContract)
            {
                return await CreateVehicleChecklistInSideContract(Guid.Parse(staffId), (Guid)req.ContractId, req.Type);
            }
            else
            {
                return await CreateVehicleChecklistOutSideContract(Guid.Parse(staffId), (Guid)req.VehicleId, req.Type);
            }
        }
        private async Task<Guid> CreateVehicleChecklistOutSideContract(Guid staffId, Guid vehicleId, int type)
        {
           
            var components = await _uow.VehicleComponentRepository.GetByVehicleIdAsync(vehicleId);
            if (components == null)
            {
                throw new NotFoundException(Message.VehicleComponentMessage.NotFound);
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
            
            return checklist.Id;
        }

        private async Task<Guid> CreateVehicleChecklistInSideContract(Guid staffId, Guid contractId, int type)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(contractId) ??
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            
            var components = await _uow.VehicleComponentRepository.GetByVehicleIdAsync((Guid)contract.VehicleId)
            ?? throw new NotFoundException(Message.VehicleComponentMessage.NotFound);
            
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
            return checklist.Id;
        }

        

        public async Task UpdateAsync(UpdateVehicleChecklistReq req, Guid id)
        {

            var checklist = await _uow.VehicleChecklistRepository.GetByIdAsync(id);
            if (checklist == null)
                throw new NotFoundException(Message.VehicleChecklistMessage.NotFound);
            if (checklist.IsSignedByCustomer && checklist.IsSignedByStaff)
                throw new BusinessException(Message.VehicleChecklistMessage.ThisChecklistAlreadyProcess);
            if(checklist.Type == (int)VehicleChecklistType.OutOfContract)
            {
                await UpdateVehicleChecklistOutSideContractAsync(checklist, req.ChecklistItems);
            }
            else
            {
                var contract = await _uow.RentalContractRepository.GetByChecklistIdAsync(id)
                    ?? throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
                await UpdateVehicleChecklistInsideContractAsync(checklist, req.ChecklistItems, contract!);
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
            IEnumerable<UpdateChecklistItemReq> checklistReq, RentalContract contract)
        {
            Invoice returnInvoice = null;

            if(checklist.Type == (int)VehicleChecklistType.Return)
            {
                returnInvoice = contract.Invoices.Where(i => i.Type == (int)InvoiceType.Return).FirstOrDefault();
            }
            IEnumerable<InvoiceItem> invoiceItems = [];
            foreach (var itemReq in checklistReq)
            {
                var existingItem = checklist.VehicleChecklistItems
                    .FirstOrDefault(i => i.Id == itemReq.Id);

                if (existingItem == null) continue;

                if(itemReq.Status != (int)DamageStatus.Good && checklist.Type == (int)VehicleChecklistType.Return)
                {
                    var invoiceItem = new InvoiceItem()
                    {
                        Id = Guid.NewGuid(),
                        InvoiceId = returnInvoice.Id,
                        Quantity = 1,
                        UnitPrice = DamageCompensationHelper.CalculateCompensation(existingItem.Component.DamageFee, itemReq.Status),
                        Type = (int)InvoiceItemType.Damage,
                        ChecklistItemId = itemReq.Id
                    };
                    invoiceItems = invoiceItems.Append(invoiceItem);
                }

                existingItem.Status = itemReq.Status;
                if (itemReq.Notes != null)
                {
                    existingItem.Notes = itemReq.Notes;
                }
            }
            if (!invoiceItems.IsNullOrEmpty())
            {
                //nếu vô đc trong này thì chắc chắn đã lấy đc return Invoice ở trên rồi
                returnInvoice!.Subtotal = returnInvoice.Subtotal + InvoiceHelper.CalculateSubTotalAmount(invoiceItems);
                // await _uow.InvoiceRepository.AddAsync(returnInvoice);
                await _uow.InvoiceItemRepository.AddRangeAsync(invoiceItems);
            }
        }

        public async Task UpdateItemsAsync(Guid id, int status, string? notes)
        {
            var item = await _uow.VehicleChecklistItemRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.VehicleChecklistItemMessage.NotFound);
            var checklist = await _uow.VehicleChecklistRepository.GetByIdAsync(item.ChecklistId)
                ?? throw new NotFoundException(Message.VehicleChecklistMessage.NotFound);
            if (checklist.IsSignedByCustomer && checklist.IsSignedByStaff)
                throw new BusinessException(Message.VehicleChecklistMessage.ThisChecklistAlreadyProcess);
            item.Status = status;
            if (!string.IsNullOrEmpty(notes)) item.Notes = notes;
            await _uow.VehicleChecklistItemRepository.UpdateAsync(item);
        }
        public async Task<VehicleChecklistViewRes> GetByIdAsync(Guid id, ClaimsPrincipal userClaims)
        {

            var vehicleChecklist = await _uow.VehicleChecklistRepository.GetByIdAsync(id);
            if (vehicleChecklist == null)
            {
                throw new NotFoundException(Message.VehicleChecklistMessage.NotFound);
            }
            var userId = Guid.Parse(userClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString());
            if (await CheckAuthorize(userId, vehicleChecklist.ContractId) == false)
            {
                throw new ForbidenException(Message.UserMessage.DoNotHavePermission);
            }
            var checklistViewRes = _mapper.Map<VehicleChecklistViewRes>(vehicleChecklist);
            return checklistViewRes;
        }
        public async Task<IEnumerable<VehicleChecklistViewRes>> GetAll(Guid? contractId, int? type, ClaimsPrincipal userClaims)
        {
            var userId = Guid.Parse(userClaims.FindFirst(JwtRegisteredClaimNames.Sid).Value.ToString());
            var vehicleChecklists = await _uow.VehicleChecklistRepository.GetAll(contractId, type);
            if (await CheckAuthorize(userId, contractId) == false) //nếu là user thì get all theo id
                throw new ForbidenException(Message.UserMessage.DoNotHavePermission);

            var checklistsViewRes = _mapper.Map<IEnumerable<VehicleChecklistViewRes>>(vehicleChecklists);
            return checklistsViewRes ?? [];
        }
        private async Task<bool> CheckAuthorize(Guid userId, Guid? contractId = null)
        {
            var roles = _cache.Get<List<Role>>("AllRoles");
            var userInDB = await _userRepository.GetByIdAsync(userId);
            var userRole = roles.FirstOrDefault(r => r.Id == userInDB.RoleId).Name;
            if (userRole == RoleName.Staff)
            {
                return true;
            }
            else //(userRole == RoleName.Customer
            {
                if (contractId != null)
                {
                    var contract = await _uow.RentalContractRepository.GetByIdAsync((Guid)contractId);
                    if (contract.CustomerId == userId) return true;
                }
                return false;
            }
        }
    }
}