using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.VehicleModel.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [RoleAuthorize(new[] { "Staff", "Admin" })]
    [Route("api/vehicle-models/{modelId:guid}")]
    public class ModelImagesController : ControllerBase
    {
        private readonly IModelImageService _modelImageService;
        private readonly IVehicleModelService _vehicleModelService;

        public ModelImagesController(
            IModelImageService modelImageService,
            IVehicleModelService vehicleModelService)
        {
            _modelImageService = modelImageService;
            _vehicleModelService = vehicleModelService;
        }

        // ---------- SUB-IMAGES (gallery) ----------
        [HttpPost("sub-images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadSubImages([FromRoute] Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var res = await _modelImageService.UploadModelImagesAsync(modelId, req.Files);
            return Ok(new { data = res, message = Message.CloudinaryMessage.UploadSuccess });
        }

        [HttpDelete("sub-images")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteSubImages([FromRoute] Guid modelId, [FromBody] DeleteModelImagesReq req)
        {
            await _modelImageService.DeleteModelImagesAsync(modelId, req.ImageIds);
            return Ok(new { message = Message.CloudinaryMessage.DeleteSuccess });
        }

        // ---------- MAIN IMAGE ----------
        [HttpPost("main-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMainImage([FromRoute] Guid modelId, [FromForm(Name = "file")] IFormFile file)
        {
            var imageUrl = await _vehicleModelService.UploadMainImageAsync(modelId, file);
            return Ok(new { data = new { modelId, imageUrl }, message = Message.CloudinaryMessage.UploadSuccess });
        }

        [HttpDelete("main-image")]
        public async Task<IActionResult> DeleteMainImage([FromRoute] Guid modelId)
        {
            await _vehicleModelService.DeleteMainImageAsync(modelId);
            return Ok(new { message = Message.CloudinaryMessage.DeleteSuccess });
        }

        // ---------- MAIN + GALLERY ----------
        [HttpPost("images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAllImages([FromRoute] Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var (mainImage, galleryImages) = await _modelImageService.UploadAllModelImagesAsync(modelId, req.Files);
            return Ok(new { data = new { main = mainImage, gallery = galleryImages }, message = Message.CloudinaryMessage.UploadSuccess });
        }
    }
}