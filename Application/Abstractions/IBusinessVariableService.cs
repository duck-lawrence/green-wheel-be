using Application.Dtos.BusinessVariable.Request;
using Application.Dtos.BusinessVariable.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IBusinessVariableService
    {
        Task<IEnumerable<BusinessVariableViewRes>> GetAllAsync();
        Task UpdateAsync(Guid id, UpdateBusinessVariableReq req);
    }
}
