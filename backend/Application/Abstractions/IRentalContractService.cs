using Application.Dtos.RentalContract.Request;
using Application.Dtos.RentalContract.Respone;
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
    }
}
