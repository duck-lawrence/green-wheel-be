using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleSegment.Respone;
using Application.Repositories;
using AutoMapper;
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
        public async Task<IEnumerable<VehicleSegmentViewRes>> GetAllVehicleSegment()
        {
            var vehicleSegments = await _vehicleSegmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleSegmentViewRes>>(vehicleSegments) ?? [];
        }
    }
}
