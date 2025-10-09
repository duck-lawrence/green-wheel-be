using Application.Dtos.StationFeedback.Request;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class StationFeedbackProfile : Profile
    {
        public StationFeedbackProfile()
        {
            CreateMap<StationFeedback, StationFeedbackRes>().ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName));
            CreateMap<StationFeedbackCreateReq, StationFeedback>();
        }
    }
}