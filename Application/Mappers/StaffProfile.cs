using Application.Dtos.Staff.Request;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<CreateStaffReq, User>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateStaffReq, Staff>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

        }
    }
}
