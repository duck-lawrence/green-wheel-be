using Application.Dtos.Invoice.Response;
using Application.Dtos.RentalContract.Request;
using Application.Dtos.RentalContract.Respone;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IRentalContractService
    {
        Task CreateRentalContractAsync(Guid UserID, CreateRentalContractReq createRentalContractReq);
         Task VerifyRentalContract(Guid id, bool haveVehicle = true, int? vehicleStatus = null);
        Task UpdateStatusAsync(Guid id);
        Task<RentalContractViewRes> GetByIdAsync(Guid id);
        Task<IEnumerable<RentalContractViewRes>> GetAll(GetAllRentalContactReq req);
        Task HandoverProcessRentalContractAsync(ClaimsPrincipal staffClaims, Guid id, HandoverContractReq req);
        Task<Guid> ReturnProcessRentalContractAsync(ClaimsPrincipal staffClaims, Guid id);
        Task<IEnumerable<RentalContractViewRes>> GetMyContracts(ClaimsPrincipal userClaims, int? status);
        Task CancelRentalContract(Guid id);
        Task ChangeVehicleAsync(Guid id);
        Task ProcessCustomerConfirm(Guid id, int resolutionOption);
    }
}
