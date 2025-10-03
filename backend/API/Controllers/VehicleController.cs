using API.Filters;
using Application.Abstractions;
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
        [RoleAuthorize("Admin")]
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
        [RoleAuthorize("Staff", "Admin")]
        [HttpPatch("{Id}")]
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
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id)
        {
            await _vehicleService.DeleteVehicle(id);
            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetVehicle(Guid stationId, Guid modelId, 
        //    DateTimeOffset startDate, DateTimeOffset endDate)
        //{
        //   var vehicle =  await _vehicleService.GetVehicle(stationId, modelId, startDate, endDate);
        //    return Ok(vehicle);
        //}
    }
}
