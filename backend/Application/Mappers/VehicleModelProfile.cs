using Application.Dtos.VehicleModel.Request;
using Application.Dtos.VehicleModel.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class VehicleModelProfile : Profile
    {
        public VehicleModelProfile()
        {
            CreateMap<CreateVehicleModelReq, VehicleModel>();
            CreateMap<VehicleModel, VehicleModelViewRes>();
        }
    }
}
