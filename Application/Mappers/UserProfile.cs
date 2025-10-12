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
                    opt => opt.MapFrom(src => src.Staff != null ? src.Staff.Station : null))
                .ForMember(dest => dest.NeedSetPassword,
                    opt => opt.MapFrom(src => src.Password == null));

            CreateMap<UserRegisterReq, User>()
                .ForMember(dest => dest.Password,
                           opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));

            CreateMap<CreateUserReq, User>();

            CreateMap<DriverLicense, DriverLicenseRes>()
            .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class.ToString()))
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex.ToString()));

            CreateMap<CitizenIdentity, CitizenIdentityRes>()
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex.ToString()));
        }
    }
}