using Application.Dtos.CitizenIdentity.Response;
using Application.Dtos.DriverLicense.Response;
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
            CreateMap<UserRegisterReq, User>()
                .ForMember(dest => dest.Password,
                           opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));

            CreateMap<User, UserProfileViewRes>();
            CreateMap<CitizenIdentity, CitizenIdentityRes>()
                .ForMember(dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex == 0 ? "Nam" : "Nữ"))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ExpiresAt,
                    opt => opt.MapFrom(src => src.ExpiresAt.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ImagePublicId,
                    opt => opt.MapFrom(src => src.ImagePublicId));

            CreateMap<DriverLicense, DriverLicenseRes>()
                .ForMember(dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex == 0 ? "Nam" : "Nữ"))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => GetLicenseClassName(src.Class)))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ExpiresAt,
                    opt => opt.MapFrom(src => src.ExpiresAt.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ImagePublicId,
                    opt => opt.MapFrom(src => src.ImagePublicId));
        }

        private static string GetLicenseClassName(int cls) =>
            cls switch
            {
                1 => "A1",
                2 => "A2",
                3 => "B1",
                4 => "B2",
                5 => "C",
                6 => "D",
                _ => "Không xác định"
            };
    }
}