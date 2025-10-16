using Application.AppExceptions;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class DispatchValidationHelper
    {
        public static async Task ValidateStaffsInStationAsync(IStaffRepository staffRepository, Guid[]? staffIds, Guid fromStationId)
        {
            if (staffIds == null || staffIds.Length == 0)
                return;

            var countValid = await staffRepository.CountStaffsInStationAsync(staffIds, fromStationId);
            if (countValid != staffIds.Length)
                throw new BadRequestException("To many staff not belong to station");
        }

        public static async Task ValidateVehiclesInStationAsync(IVehicleRepository vehicleRepository, Guid[]? vehicleId, Guid stationId)
        {
            if (vehicleId.Length == 0 || vehicleId == null) return;

            var countValidate = await vehicleRepository.CountVehiclesInStationAsync(vehicleId, stationId);
            if (countValidate != vehicleId.Length)
                throw new BadRequestException("some vehicles not in station");
        }

        public static void EnsureDifferentStations(Guid fromStationId, Guid toStationId)
        {
            if (fromStationId == toStationId) throw new ForbidenException("two station same ID");
        }
    }
}