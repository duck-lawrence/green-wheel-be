using Application.Dtos.Role.Response;
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