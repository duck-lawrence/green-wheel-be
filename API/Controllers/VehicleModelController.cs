using API.Filters;
using Application;
using Application.Abstractions;
using Application.Constants;
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
        private readonly IModelImageService _modelImageService;

        public VehicleModelController(IVehicleModelService vehicleModelService,
            IModelImageService modelImageService)
        {
            _vehicleModelService = vehicleModelService;
            _modelImageService = modelImageService;
        }

        /*
         401: unauthorized
         403: not have permission
         --400: invalid type
         200: success
         */

        [RoleAuthorize(RoleName.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateVehicleModel([FromBody] CreateVehicleModelReq createVehicleModelReq)
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

        [RoleAuthorize(RoleName.Admin)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateVehicleModel([FromRoute] Guid id, UpdateVehicleModelReq updateVehicleModelReq)
        {
            await _vehicleModelService.UpdateVehicleModelAsync(id, updateVehicleModelReq);
            return Ok();
        }

        /*
         200: success
         */

        [HttpGet("search")]
        public async Task<IActionResult> SearchVehicleModel([FromQuery] VehicleFilterReq vehicleFilterReq)
        {
            var verhicelModelView = await _vehicleModelService.SearchVehicleModel(vehicleFilterReq);
            return Ok(verhicelModelView);
        }

        /*
         200: success
         */

        [HttpGet]
        public async Task<IActionResult> GetAll(string? name, Guid? segmentId)
        {
            var verhicelModelView = await _vehicleModelService.GetAllAsync(name, segmentId);
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

        [RoleAuthorize(RoleName.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleModel([FromRoute] Guid id)
        {
            await _vehicleModelService.DeleteVehicleModleAsync(id);
            return Ok();
        }

        // ---------- SUB-IMAGES (gallery) ----------

        [HttpPost("sub-images")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadSubImages([FromRoute] Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var res = await _modelImageService.UploadModelImagesAsync(modelId, req.Files);
            return Ok(new { data = res, message = Message.CloudinaryMessage.UploadSuccess });
        }

        [HttpDelete("sub-images")]
        [Consumes("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteSubImages([FromRoute] Guid modelId, [FromBody] DeleteModelImagesReq req)
        {
            await _modelImageService.DeleteModelImagesAsync(modelId, req.ImageIds);
            return Ok(new { message = Message.CloudinaryMessage.DeleteSuccess });
        }

        // ---------- MAIN IMAGE ----------

        [HttpPost("main-image")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadMainImage([FromRoute] Guid modelId, [FromForm(Name = "file")] IFormFile file)
        {
            var imageUrl = await _vehicleModelService.UploadMainImageAsync(modelId, file);
            return Ok(new { data = new { modelId, imageUrl }, message = Message.CloudinaryMessage.UploadSuccess });
        }

        [HttpDelete("main-image")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteMainImage([FromRoute] Guid modelId)
        {
            await _vehicleModelService.DeleteMainImageAsync(modelId);
            return Ok(new { message = Message.CloudinaryMessage.DeleteSuccess });
        }

        // ---------- MAIN + GALLERY ----------
        [HttpPost("images")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadAllImages([FromRoute] Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var (mainImage, galleryImages) = await _modelImageService.UploadAllModelImagesAsync(modelId, req.Files);
            return Ok(new { data = new { main = mainImage, gallery = galleryImages }, message = Message.CloudinaryMessage.UploadSuccess });
        }
    }
}