using Application.AppExceptions;
using Application.Constants;
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
                throw new BadRequestException(Message.DispatchMessage.StaffNotInFromStation);
        }

        public static async Task ValidateVehiclesInStationAsync(IVehicleRepository vehicleRepository, Guid[]? vehicleId, Guid fromStationId)
        {
            if (vehicleId == null || vehicleId.Length == 0) return;

            var countValid = await vehicleRepository.CountVehiclesInStationAsync(vehicleId, fromStationId);
            if (countValid != vehicleId.Length)
                throw new BadRequestException(Message.DispatchMessage.VehicleNotInFromStation);
        }

        public static void EnsureDifferentStations(Guid fromStationId, Guid toStationId)
        {
            if (fromStationId == toStationId)
                throw new ForbidenException(Message.DispatchMessage.ToStationMustDifferent);
        }

        public static void EnsureCanUpdate(
            Guid currentStationId,
            Guid expectedStationId,
            DispatchRequestStatus currentStatus,
            DispatchRequestStatus requiredStatus,
            string forbiddenMessage,
            string invalidStatusMessage)
        {
            if (currentStationId != expectedStationId)
                throw new ForbidenException(forbiddenMessage);
            if (currentStatus != requiredStatus)
                throw new BadRequestException(invalidStatusMessage);
        }
    }
}