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

        /// <summary>
        /// Retrieves all vehicle segments available in the system.
        /// </summary>
        /// <returns>List of vehicle segments.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">No vehicle segments found.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllVehicleSegment()
        {
            var vehicleSegments = await _vehicleSegmentSerivce.GetAllVehicleSegment();
            return Ok(vehicleSegments);
        }
    }
}
