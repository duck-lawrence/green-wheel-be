using Application.Dtos.Vehicle.Request;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<CreateVehicleReq, Vehicle>();
        }

    }
}
