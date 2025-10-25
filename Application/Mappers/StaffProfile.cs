using Application.Dtos.Staff.Request;
using Application.Dtos.Staff.Response;
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

            CreateMap<Staff, StaffRes>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.Station, opt => opt.MapFrom(src => src.Station));

        }
    }
}
