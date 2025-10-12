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
        Task<IEnumerable<RentalContractViewRes>> GetByCustomerPhoneAndContractStatus(int? status = null, string? phone = null);
        Task HandoverRentalContractAsync(ClaimsPrincipal staffClaims, Guid id, HandoverContractReq req);
        Task<InvoiceViewRes?> ReturnRentalContractAsync(ClaimsPrincipal staffClaims, Guid id);
        Task<RentalContractViewRes?> GetContractByUserId(ClaimsPrincipal userClaims);
    }
}
