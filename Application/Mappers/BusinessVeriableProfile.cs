using Application.Dtos.BusinessVariable.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class BusinessVeriableProfile : Profile
    {
        public BusinessVeriableProfile()
        {
            CreateMap<BusinessVariable, BusinessVariableViewRes>();
        }
    }
}
