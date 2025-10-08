﻿using Application.Dtos.Brand.Respone;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandViewRes>();
        }
    }
}
