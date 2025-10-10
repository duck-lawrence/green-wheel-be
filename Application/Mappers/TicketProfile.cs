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
            CreateMap<Ticket, TicketRes>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((TicketStatus)src.Status).ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ((TicketType)src.Type).ToString()))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Assignee != null
                    ? $"{src.Assignee.User.FirstName} {src.Assignee.User.LastName}"
                    : null));
        }
    }
}