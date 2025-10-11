using Application.Dtos.UserSupport.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class SupportRequestProfile : Profile
    {
        public SupportRequestProfile()
        {
            CreateMap<Ticket, SupportRes>();
        }
    }
}