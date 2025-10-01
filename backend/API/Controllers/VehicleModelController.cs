using Application;
using Application.Abstractions;
using Application.Dtos.VehicleModel.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/vihicle-models")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IVehicleModelService _vehicleModelService;

        public VehicleModelController(IVehicleModelService vehicleModelService)
        {
            _vehicleModelService = vehicleModelService;
        }
        /*
         --400: invalid type
         200: success
         500: faild to save to DB
         */
        [HttpPost("create-vehicle-model")]
        public async Task<IActionResult> CreateVehicleModel(CreateVehicleModelReq createVehicleModelReq)
        {
            var id = await _vehicleModelService.CreateVehicleModelAsync(createVehicleModelReq);
            return Ok(new
            {
                Id = id
            });
        }
        /*
         200: success
         500: faild to save to Db
         --400: invalid type
         404: not found
         */
        [HttpPatch("update-vehicle-model/{id}")]
        public async Task<IActionResult> UpdateVehicleModel([FromRoute] Guid id, UpdateVehicleModelReq updateVehicleModelReq)
        {
            await _vehicleModelService.UpdateVehicleModelAsync(id, updateVehicleModelReq);
            return Ok();
        }

        /*
         200: success
         500: faild in Db maybe
         */
        [HttpGet("get-all-vehicle-models")]
        public async Task<IActionResult> GetAllVehicleModel(VehicleFilterReq vehicleFilterReq)
        {
            var verhicelModelView = await _vehicleModelService.GetAllVehicleModels(vehicleFilterReq);
            return Ok(verhicelModelView);
        }
        /*
         404: not found
         500: faild in db
         200: success
         */
        [HttpDelete("delete-vehicle-model/{id}")]
        public async Task<IActionResult> DeleteVehicleModel([FromRoute] Guid id)
        {
            await _vehicleModelService.DeleteVehicleModleAsync(id);
            return Ok();
        }
    }
}
