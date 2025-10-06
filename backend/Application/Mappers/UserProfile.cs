using System;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // CreateMap<Role, RoleSummaryRes>();
            // User → UserProfileViewRes, điền Role, RoleId, RoleDetail, StationId
            //Nếu bỏ phần này, AutoMapper sẽ không set các property mới, khiến DTO gửi về frontend thiếu thông tin (Phúc thêm)
            // Mục đích:  response /api/users/me trả về đầy đủ thông tin role, 
            // giúp useAuth ở frontend biết chắc user có role “staff”.
            CreateMap<User, UserProfileViewRes>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (Guid?)src.RoleId))
                // .ForMember(dest => dest.RoleDetail, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.StationId, opt => opt.MapFrom(src => src.Staff != null ? (Guid?)src.Staff.StationId : null));

            CreateMap<UserRegisterReq, User>()
                .ForMember(dest => dest.Password,
                           opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));
        }
    }
}
