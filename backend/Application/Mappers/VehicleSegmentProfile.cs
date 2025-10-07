using Application.Dtos.VehicleSegment.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class VehicleSegmentProfile : Profile
    {
        public VehicleSegmentProfile()
        {
            CreateMap<VehicleSegment, VehicleSegmentViewRes>();
        }
    }
}
