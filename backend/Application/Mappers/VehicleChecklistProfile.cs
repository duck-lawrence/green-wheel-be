using Application.Dtos.VehicleChecklist.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class VehicleChecklistProfile : Profile
    {
        public VehicleChecklistProfile()
        {
            CreateMap<VehicleChecklist, VehicleChecklistViewRes>();
        }
    }
}
