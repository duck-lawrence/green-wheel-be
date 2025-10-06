using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.VehicleModel.Request;
using Application.Dtos.VehicleModel.Respone;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly IVehicleModelRepository _vehicleModelRepository;
        private readonly IMapper _mapper;

        public VehicleModelService(IVehicleModelRepository vehicleModelRepository, IMapper mapper)
        {
            _vehicleModelRepository = vehicleModelRepository;
            _mapper = mapper;
        }
        public async Task<Guid> CreateVehicleModelAsync(CreateVehicleModelReq createVehicleModelReq)
        {
            Guid id;
            do
            {
                id = new Guid();
            } while (await _vehicleModelRepository.GetByIdAsync(id) != null);
            var vehicleModel = _mapper.Map<VehicleModel>(createVehicleModelReq);
            vehicleModel.CreatedAt = vehicleModel.UpdatedAt = DateTimeOffset.UtcNow;
            vehicleModel.DeletedAt = null;
            return await _vehicleModelRepository.AddAsync(vehicleModel);
        }

        public async Task<bool> DeleteVehicleModleAsync(Guid id)
        {
            return await _vehicleModelRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<VehicleModelViewRes>> GetAllVehicleModels(VehicleFilterReq vehicleFilterReq)
        {
            return await _vehicleModelRepository.FilterVehicleModelsAsync(vehicleFilterReq.StationId, vehicleFilterReq.StartDate, vehicleFilterReq.EndDate, vehicleFilterReq.SegmentId);
        }

        public async Task<VehicleModelViewRes> GetByIdAsync(Guid id, Guid stationId ,DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var vehicleModelViewRes = await _vehicleModelRepository.GetByIdAsync(id, stationId, startDate, endDate);
            if(vehicleModelViewRes == null)
            {
                throw new NotFoundException(Message.VehicleModelMessage.VehicleModelNotFound);
            }
            return vehicleModelViewRes;
        }

        public async Task<int> UpdateVehicleModelAsync(Guid Id, UpdateVehicleModelReq updateVehicleModelReq)
        {
            var vehicleModelFromDB = await _vehicleModelRepository.GetByIdAsync(Id);
            if(vehicleModelFromDB == null)
            {
                throw new NotFoundException(Message.VehicleModelMessage.VehicelModelNotFound);
            }
            if(updateVehicleModelReq.Name != null)
            {
                vehicleModelFromDB.Name = updateVehicleModelReq.Name;   
            }
            if(updateVehicleModelReq.Description != null)
            {
                vehicleModelFromDB.Description = updateVehicleModelReq.Description;
            }
            if (updateVehicleModelReq.CostPerDay != null)
            {
                vehicleModelFromDB.CostPerDay = (decimal)updateVehicleModelReq.CostPerDay;
            }
            if (updateVehicleModelReq.DepositFee != null)
            {
                vehicleModelFromDB.DepositFee = (decimal)updateVehicleModelReq.DepositFee;
            }
            if (updateVehicleModelReq.SeatingCapacity != null)
            {
                vehicleModelFromDB.SeatingCapacity = (int)updateVehicleModelReq.SeatingCapacity;
            }
            if (updateVehicleModelReq.NumberOfAirbags != null)
            {
                vehicleModelFromDB.SeatingCapacity = (int)updateVehicleModelReq.SeatingCapacity;
            }
            if (updateVehicleModelReq.MotorPower != null)
            {
                vehicleModelFromDB.MotorPower = (decimal)updateVehicleModelReq.MotorPower;
            }
            if (updateVehicleModelReq.BatteryCapacity != null)
            {
                vehicleModelFromDB.BatteryCapacity = (decimal)updateVehicleModelReq.BatteryCapacity;
            }
            if (updateVehicleModelReq.EcoRangeKm != null)
            {
                vehicleModelFromDB.EcoRangeKm = (decimal)updateVehicleModelReq.EcoRangeKm;
            }
            if (updateVehicleModelReq.SportRangeKm != null)
            {
                vehicleModelFromDB.SportRangeKm = (decimal)updateVehicleModelReq.SportRangeKm;
            }
            if (updateVehicleModelReq.BrandId != null)
            {
                vehicleModelFromDB.BrandId = (Guid)updateVehicleModelReq.BrandId;
            }
            if (updateVehicleModelReq.SegmentId != null)
            {
                vehicleModelFromDB.SegmentId = (Guid)updateVehicleModelReq.SegmentId;
            }

            return await _vehicleModelRepository.UpdateAsync(vehicleModelFromDB);
        }
    }
}
