using Application.Constants;
using Application.Dtos.Dispatch.Request;
using Application.Dtos.Dispatch.Response;
using Application.Dtos.Staff.Response;
using Application.Dtos.Station.Respone;
using Application.Dtos.Vehicle.Respone;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class DispatchProfile : Profile
    {
        public DispatchProfile()
        {
            CreateMap<CreateDispatchReq, DispatchRequest>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.FromStationId, opt => opt.MapFrom(src => src.FromStationId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => (int)DispatchRequestStatus.Pending))
                .ForAllOtherMembers(opt => opt.Ignore());

            // Entity -> Res
            CreateMap<DispatchRequest, DispatchRes>()
                .ForMember(dest => dest.FromStationName, opt => opt.MapFrom(src => src.FromStation.Name))
                .ForMember(dest => dest.ToStationName, opt => opt.MapFrom(src => src.ToStation.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (DispatchRequestStatus)src.Status))
                .ForMember(dest => dest.RequestAdminName,
                    opt => opt.MapFrom(src => src.RequestAdmin.User.FirstName + " " + src.RequestAdmin.User.LastName))
                .ForMember(dest => dest.ApprovedAdminName,
                    opt => opt.MapFrom(src => src.ApprovedAdmin != null
                        ? src.ApprovedAdmin.User.FirstName + " " + src.ApprovedAdmin.User.LastName
                        : null))
                .ForAllOtherMembers(opt => opt.MapAtRuntime());

            // Staffs + Vehicles
            CreateMap<DispatchRequestStaff, DispatchRequestStaffRes>();
            CreateMap<DispatchRequestVehicle, DispatchRequestVehicleRes>();

            // Staff / Vehicle / Model / Station
            CreateMap<Staff, StaffRes>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.Station, opt => opt.MapFrom(src => src.Station));

            CreateMap<Vehicle, VehicleRes>();
            CreateMap<VehicleModel, VehicleModelRes>();
            CreateMap<Station, StationSimpleRes>();

            ////Entity->Response DTO
            //    CreateMap<DispatchRequest, DispatchRes>()
            //        .ForMember(dest => dest.FromStationName,
            //            opt => opt.MapFrom(src => src.FromStation.Name))
            //        .ForMember(dest => dest.ToStationName,
            //            opt => opt.MapFrom(src => src.ToStation.Name))
            //        .ForMember(dest => dest.RequestAdminName,
            //            opt => opt.MapFrom(src => src.RequestAdmin.User.FirstName + src.RequestAdmin.User.LastName))
            //        // int -> enum
            //        .ForMember(dest => dest.Status,
            //            opt => opt.MapFrom(src => (DispatchRequestStatus)src.Status))
            //        .ForAllOtherMembers(opt => opt.MapAtRuntime());

            //// CreateReq -> Entity (chỉ map field cần, còn lại service xử lý)
            //CreateMap<CreateDispatchReq, DispatchRequest>()
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.ToStationId, opt => opt.MapFrom(src => src.FromStationId))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => (int)DispatchRequestStatus.Pending))
            //    .ForAllOtherMembers(opt => opt.Ignore());

            //// UpdateReq -> Entity (dùng khi cần map status trực tiếp)
            //CreateMap<UpdateDispatchReq, DispatchRequest>()
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            //    .ForAllOtherMembers(opt => opt.Ignore());

            //// staffIds -> DispatchRequestStaffs
            //CreateMap<Guid, DispatchRequestStaff>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            //    .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src))
            //    .ForAllOtherMembers(opt => opt.Ignore());

            //// vehicleIds -> DispatchRequestVehicles
            //CreateMap<Guid, DispatchRequestVehicle>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            //    .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src))
            //    .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}