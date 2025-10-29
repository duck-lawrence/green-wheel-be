using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleComponent.Request;
using Application.Dtos.VehicleComponent.Respone;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
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

        public async Task<IEnumerable<VehicleComponentViewRes>> GetAllAsync(Guid? id)
        {
            var vehicleComponents = await  _vehicleComponentRepository.GetAllAsync(id);
            return _mapper.Map<IEnumerable<VehicleComponentViewRes>>(vehicleComponents) ?? [];
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
