using Application.Dtos.VehicleComponent.Request;
using Application.Dtos.VehicleComponent.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class VehicleComponentProfile : Profile
    {
        public VehicleComponentProfile()
        {
            CreateMap<VehicleComponent, VehicleComponentViewRes>();
            CreateMap<CreateVehicleComponentReq, VehicleComponent>();
        }
    }
}
