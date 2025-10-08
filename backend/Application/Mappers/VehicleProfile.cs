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
            CreateMap<CreateVehicleReq, Vehicle>();
            CreateMap<Vehicle, VehicleViewRes>();
        }
    }
}
