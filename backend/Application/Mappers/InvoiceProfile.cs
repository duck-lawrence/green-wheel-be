using Application.Dtos.Invoice.Response;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceViewRes>()
                .ForMember(dest => dest.Total,
                otp => otp.MapFrom(src => src.Subtotal + src.Subtotal * src.Tax));
                
            
        }
    }

}
