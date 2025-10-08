using Application.Dtos.CitizenIdentity.Response;
using Application.Dtos.DriverLicense.Response;
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
            CreateMap<User, UserProfileViewRes>()
                .ForMember(dest => dest.LicenseUrl,
                    opt => opt.MapFrom(src => src.DriverLicense != null ? src.DriverLicense.ImageUrl : null))
                .ForMember(dest => dest.CitizenUrl,
                    opt => opt.MapFrom(src => src.CitizenIdentity != null ? src.CitizenIdentity.ImageUrl : null))
                .ForMember(dest => dest.Station,
                    opt => opt.MapFrom(src => src.Staff != null ? src.Staff.Station : null));

            CreateMap<UserRegisterReq, User>()
                .ForMember(dest => dest.Password,
                           opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));

            CreateMap<User, UserProfileViewRes>();
            
            CreateMap<CreateUserReq, User>();
            
            CreateMap<CitizenIdentity, CitizenIdentityRes>();

            CreateMap<DriverLicense, DriverLicenseRes>();
        
        }
    }
}