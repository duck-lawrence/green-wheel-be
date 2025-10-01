using Application.Dtos.RentalContract.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IRentalContractService
    {
        Task<Guid> CreateRentalContractAsync(CreateRentalContractReq createRentalContractReq);
    }
}
