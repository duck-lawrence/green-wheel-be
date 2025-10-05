using Application.Abstractions;
using Application.Dtos.Vehicle.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/vehicles/{vehicleId:guid}/images")]
    public class VehicleImagesController : ControllerBase
    {
        private readonly IVehicleImageService _vehicleImageService;

        public VehicleImagesController(IVehicleImageService vehicleImageService)
        {
            _vehicleImageService = vehicleImageService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadVehicleImages(Guid vehicleId, [FromForm] UploadVehicleImagesReq req)
        {
            var result = await _vehicleImageService.UploadVehicleImagesAsync(vehicleId, req.Files);
            return Ok(result.Select(x => new { x.Id, x.Url, x.PublicId }));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVehicleImages(Guid vehicleId, [FromBody] DeleteVehicleImagesReq req)
        {
            await _vehicleImageService.DeleteVehicleImagesAsync(vehicleId, req.ImageIds);
            return NoContent();
        }
    }
}