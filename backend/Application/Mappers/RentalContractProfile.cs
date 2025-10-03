using Application.Dtos.RentalContract.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class RentalContractProfile : Profile
    {
        public RentalContractProfile()
        {
            CreateMap<RentalContract, RentalContractViewRes>();
            CreateMap<RentalContract, RentalContractForStaffViewRes>();
        }
    }
}
