using Application.Dtos.Brand.Respone;
using Application.Dtos.VehicleModel.Respone;
using Application.Dtos.VehicleSegment.Respone;
﻿using Application.Dtos.Vehicle.Request;
using Application.Dtos.Vehicle.Respone;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Brand, BrandViewRes>();
            CreateMap<VehicleSegment, VehicleSegmentViewRes>();

            CreateMap<VehicleModel, VehicleModelViewRes>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Segment, opt => opt.MapFrom(src => src.Segment))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ModelImages.Select(mi => mi.Url)))
                .ForMember(dest => dest.AvailableVehicleCount, opt => opt.Ignore());
            CreateMap<CreateVehicleReq, Vehicle>();
            CreateMap<Vehicle, VehicleViewRes>();
        }
    }
}