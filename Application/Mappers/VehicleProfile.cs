using Application.Dtos.Brand.Respone;
using Application.Dtos.VehicleModel.Respone;
using Application.Dtos.VehicleSegment.Respone;
using Application.Dtos.Vehicle.Request;
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
            CreateMap<CreateVehicleReq, Vehicle>();
            CreateMap<Vehicle, VehicleViewRes>();
            CreateMap<Vehicle, VehicleRes>();
        }
    }
}