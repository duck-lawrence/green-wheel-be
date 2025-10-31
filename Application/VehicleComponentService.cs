using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.RentalContract.Respone;
using Application.Dtos.VehicleComponent.Request;
using Application.Dtos.VehicleComponent.Respone;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class VehicleComponentService : IVehicleComponentService
    {
        private readonly IMapper _mapper;
        private readonly IVehicleComponentRepository _vehicleComponentRepository;
        public VehicleComponentService(IMapper mapper, IVehicleComponentRepository vehicleComponentRepository)
        {
            _vehicleComponentRepository = vehicleComponentRepository;
            _mapper = mapper;
        }
        public async Task<Guid> AddAsync(CreateVehicleComponentReq req)
        {
            var id = Guid.NewGuid();
            var vehicleComponent = _mapper.Map<VehicleComponent>(req);
            vehicleComponent.Id = id;
            return await _vehicleComponentRepository.AddAsync(vehicleComponent);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _vehicleComponentRepository.DeleteAsync(id);
        }

        public async Task<PageResult<VehicleComponentViewRes>> GetAllAsync(Guid? id, string? name, PaginationParams pagination)
        {
            var pageResult = await  _vehicleComponentRepository.GetAllAsync(id, name, pagination);
            var itemsMapped = _mapper.Map<IEnumerable<VehicleComponentViewRes>>(pageResult.Items);
            return new PageResult<VehicleComponentViewRes>(
                itemsMapped,
                pageResult.PageNumber,
                pageResult.PageSize,
                pageResult.Total
            );
        }

        public async Task<VehicleComponentViewRes> GetByIdAsync(Guid id)
        {
            var vehicleComponent = await _vehicleComponentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.VehicleComponentMessage.NotFound);
            return _mapper.Map<VehicleComponentViewRes>(vehicleComponent);
        }

        public async Task UpdateAsync(Guid id, UpdateVehicleComponentReq req)
        {
            var vehicleComponent = await _vehicleComponentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.VehicleComponentMessage.NotFound);
            if(!string.IsNullOrEmpty(req.Name))
            {
                vehicleComponent.Name = req.Name;
            }
            if(!string.IsNullOrEmpty(req.Description))
            {
                vehicleComponent.Description = req.Description;
            }
            if(req.DamageFee != null && req.DamageFee >= 0) vehicleComponent.DamageFee = (decimal)req.DamageFee;
            await _vehicleComponentRepository.UpdateAsync(vehicleComponent);
        }
    }
}
