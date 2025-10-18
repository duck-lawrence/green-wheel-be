using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.Vehicle.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /*
         401: unauthorized
         403: not have permission
         200: success
         */

        [RoleAuthorize(RoleName.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(CreateVehicleReq createVehicleReq)
        {
            var vehicleId = await _vehicleService.CreateVehicleAsync(createVehicleReq);
            return Ok(new
            {
                VehicleId = vehicleId
            });
        }

        /*
        401: unauthorized
         403: not have permission
        200: success
        404: vehicle does not exist
        */

        [RoleAuthorize(RoleName.Staff, RoleName.Admin)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateVehicle([FromRoute] Guid id, UpdateVehicleReq updateVehicleReq)
        {
            await _vehicleService.UpdateVehicleAsync(id, updateVehicleReq);
            return Ok();
        }

        /*
         401: unauthorized
         403: not have permission
         200 success
         404: vehicle not found
         */

        [RoleAuthorize("Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id)
        {
            await _vehicleService.DeleteVehicle(id);
            return Ok();
        }

        [RoleAuthorize(RoleName.Staff, RoleName.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll(string? name, Guid? stationId, int? status, string? licensePlate)
        {
            var vehicle = await _vehicleService.GetAllAsync(name, stationId, status, licensePlate);
            return Ok(vehicle);
        }

        [RoleAuthorize(RoleName.Staff, RoleName.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleById(id);
            return Ok(vehicle);
        }
    }
}