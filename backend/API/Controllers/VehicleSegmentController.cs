using Application;
using Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/vehicle-segments")]
    [ApiController]
    public class VehicleSegmentController : ControllerBase
    {
        private readonly IVehicleSegmentService _vehicleSegmentSerivce;

        public VehicleSegmentController(IVehicleSegmentService vehicleSegmentSerivce)
        {
            _vehicleSegmentSerivce = vehicleSegmentSerivce;
        }

        /*
         * Status code
         * 200: success
         * 404: not found
         */
        [HttpGet]
        public async Task<IActionResult> GetAllVehicleSegment()
        {
            var vehicleSegments = await _vehicleSegmentSerivce.GetAllVehicleSegment();
            return Ok(vehicleSegments);
        }
    }
}
