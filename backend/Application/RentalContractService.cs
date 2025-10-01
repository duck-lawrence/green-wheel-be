
using Application.Abstractions;
using Application.Dtos.RentalContract.Request;
using Application.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractUow _uow;
        public RentalContractService(IRentalContractUow uow)
        {
            _uow = uow;
        }
        public Task<Guid> CreateRentalContractAsync(CreateRentalContractReq createRentalContractReq)
        {
            throw new NotImplementedException();
        }
    }
}
