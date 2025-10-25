using Application.Constants;
using Application.Dtos.Ticket.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketRes>();
        }
    }
}