using Application.Dtos.User.Respone;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<UserRegisterReq, User>()
            //    .ForMember(dest => dest.Password,
            //               opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)));

            CreateMap<User, UserProfileViewRes>();
                

        }
            
    }
}
