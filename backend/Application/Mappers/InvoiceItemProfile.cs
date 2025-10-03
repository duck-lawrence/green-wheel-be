using Application.Dtos.InvoiceItem.Response;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class InvoiceItemProfile : Profile
    {
        public InvoiceItemProfile()
        {
            CreateMap<InvoiceItem, InvoiceItemViewRes>()
                .ForMember(dest => dest.SubTotal,
                    otp => otp.MapFrom(src => src.Quantity * src.UnitPrice));
        }
    }
}
