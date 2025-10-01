using API.Filters;
using Application;
using Application.Abstractions;
using Application.Dtos.VehicleModel.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/vehicle-models")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IVehicleModelService _vehicleModelService;

        public VehicleModelController(IVehicleModelService vehicleModelService)
        {
            _vehicleModelService = vehicleModelService;
        }
        /*
         401: unauthorized
         403: not have permission
         --400: invalid type
         200: success
         */
        [RoleAuthorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVehicleModel(CreateVehicleModelReq createVehicleModelReq)
        {
            var id = await _vehicleModelService.CreateVehicleModelAsync(createVehicleModelReq);
            return Ok(new
            {
                Id = id
            });
        }
        /*
         401: unauthorized
         403: not have permission
         200: success
         --400: invalid type
         404: not found
         */
        [RoleAuthorize("Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateVehicleModel([FromRoute] Guid id, UpdateVehicleModelReq updateVehicleModelReq)
        {
            await _vehicleModelService.UpdateVehicleModelAsync(id, updateVehicleModelReq);
            return Ok();
        }

        /*
         200: success
         */
        [HttpGet]
        public async Task<IActionResult> GetAllVehicleModel([FromQuery]VehicleFilterReq vehicleFilterReq)
        {
            var verhicelModelView = await _vehicleModelService.GetAllVehicleModels(vehicleFilterReq);
            return Ok(verhicelModelView);
        }
        /*
         200: success
         404: not found
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicelModelById([FromRoute] Guid id, Guid stationId,
                                                 DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var verhicelModelView = await _vehicleModelService.GetByIdAsync(id, stationId, startDate, endDate);
            return Ok(verhicelModelView);
        }
        /*
         401: unauthorized
         403: not have permission
         404: vehicle model not found
         200: success
         */
        [RoleAuthorize("Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleModel([FromRoute] Guid id)
        {
            await _vehicleModelService.DeleteVehicleModleAsync(id);
            return Ok();
        }

        
    }
}
