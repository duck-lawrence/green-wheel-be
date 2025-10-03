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
        Task<RentalContractViewRes> CreateRentalContractAsync(ClaimsPrincipal userClaim, CreateRentalContractReq createRentalContractReq);
        Task<IEnumerable<RentalContractForStaffViewRes>> GetByStatus(int status);
         Task VerifyRentalContract(Guid id);
        Task UpdateStatus(RentalContract rentalContract, int status);

    }
}
