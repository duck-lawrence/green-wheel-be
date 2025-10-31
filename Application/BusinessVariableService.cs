using Application.Abstractions;
using Application.Constants;
using Application.Dtos.BusinessVariable.Request;
using Application.Dtos.BusinessVariable.Respone;
using Application.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BusinessVariableService : IBusinessVariableService
    {
        private readonly IBusinessVariableRepository _businessVariableRepository;
        private readonly IMapper _mapper;
        public BusinessVariableService(IBusinessVariableRepository businessVariableRepository, IMapper mapper)
        {
            _businessVariableRepository = businessVariableRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BusinessVariableViewRes>> GetAllAsync()
        {
            var businessVariables = await _businessVariableRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BusinessVariableViewRes>>(businessVariables) ?? [];
        }

        public async Task UpdateAsync(Guid id, UpdateBusinessVariableReq req)
        {
            var businessVariable = await _businessVariableRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException(Message.BusinessVariable.NotFound);
            businessVariable.Value = req.Value;
            await _businessVariableRepository.UpdateAsync(businessVariable);
        }
    }
}
