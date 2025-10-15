using Application.Constants;
using Application.Dtos.Invoice.Response;
using Application.Helpers;
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
                otp => otp.MapFrom((src, dest, destMember, context) =>    
                    {
                        var reservationFee = context.Items.ContainsKey("ReservationFee")
                            ? Convert.ToDecimal(context.Items["ReservationFee"])
                            : 0m; // default value nếu không có
                        if (src.Type == (int)InvoiceType.Handover)
                            return InvoiceHelper.CalculateTotalAmount(src) - reservationFee;
                        else return InvoiceHelper.CalculateTotalAmount(src);
                    }
                ));
        }
    }
}
