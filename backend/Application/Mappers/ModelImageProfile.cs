using Application.Dtos.VehicleModel.Respone;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class ModelImageProfile : Profile
    {
        public ModelImageProfile()
        {
            CreateMap<ModelImage, VehicleModelImageRes>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.ImagePublicId, opt => opt.MapFrom(src => src.PublicId));
        }
    }
}