using Application.Dtos.Brand.Respone;
using AutoMapper;
using Domain.Entities;

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