using Application.Dtos.VehicleChecklistItem.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class VehicleChecklistItemProfile : Profile
    {
        public VehicleChecklistItemProfile()
        {
            CreateMap<VehicleChecklistItem, ChecklistItemImageRes>();
            CreateMap<VehicleChecklistItem, VehicleChecklistItemViewRes>();
        }
    }
}
