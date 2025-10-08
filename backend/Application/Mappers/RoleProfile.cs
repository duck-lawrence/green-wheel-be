using Application.Dtos.Role.Respone;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleViewRes>();
        }
    }
}