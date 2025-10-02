using Application.Dtos.Deposit.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    internal class DepositProfile : Profile
    {
        public DepositProfile()
        {
            CreateMap<Deposit, DepositViewRespone>();
        }
    }
}
