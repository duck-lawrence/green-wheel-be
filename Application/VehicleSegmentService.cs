using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleSegment.Request;
using Application.Dtos.VehicleSegment.Respone;
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
    public class VehicleSegmentService : IVehicleSegmentService
    {
        private readonly IVehicleSegmentRepository _vehicleSegmentRepository;
        private readonly IMapper _mapper;
        public VehicleSegmentService(IVehicleSegmentRepository vehicleSegmentRepository, IMapper mapper)
        {
            _vehicleSegmentRepository = vehicleSegmentRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(CreateSegmentReq req)
        {
            var allSegment = await _vehicleSegmentRepository.GetAllAsync();
            if(allSegment.Any(s => s.Name.ToLower() == req.Name.ToLower()))
            {
                throw new ConflictDuplicateException(Message.VehicleSegmentMessage.NameAlreadyExists);
            }
            return await _vehicleSegmentRepository.AddAsync(_mapper.Map<VehicleSegment>(req));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _vehicleSegmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<VehicleSegmentViewRes>> GetAllVehicleSegment()
        {
            var vehicleSegments = await _vehicleSegmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleSegmentViewRes>>(vehicleSegments) ?? [];
        }

        public async Task UpdateAsync(Guid id, UpdateSegmentReq req)
        {
            var segment = await  _vehicleSegmentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(Message.VehicleSegmentMessage.NotFound);
            if(req.Name != null)
            {
                var allSegment = await _vehicleSegmentRepository.GetAllAsync();
                if (allSegment.Any(s => s.Name.ToLower() == req.Name.ToLower() && s.Id != id))
                {
                    throw new ConflictDuplicateException(Message.VehicleSegmentMessage.NameAlreadyExists);
                }
                segment.Name = req.Name;
            }
            if(req.Description != null)
            {
                segment.Description = req.Description;
            }
            await _vehicleSegmentRepository.UpdateAsync(segment);
        }
    }
}
