using API.Filters;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.VehicleModel.Request;
using Application.Dtos.VehicleModel.Respone;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [RoleAuthorize(["Staff", "Admin"])]
    [Route("api/vehicle-models/{modelId:guid}")]
    public class ModelImagesController : ControllerBase
    {
        private readonly IModelImageService _modelImageService;
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IMapper _mapper;

        public ModelImagesController(
            IModelImageService modelImageService,
            IVehicleModelService vehicleModelService,
            IMapper mapper)
        {
            _modelImageService = modelImageService;
            _vehicleModelService = vehicleModelService;
            _mapper = mapper;
        }

        // ----------------- GALLERY IMAGES -----------------

        [HttpPost("sub-images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadSubImages(Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var images = await _modelImageService.UploadModelImagesAsync(modelId, req.Files);

            var res = _mapper.Map<IEnumerable<VehicleModelImageRes>>(images);

            return Ok(new
            {
                data = res,
                message = Message.CloudinaryMessage.UploadSuccess
            });
        }

        [HttpDelete("sub-images")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteSubImages(Guid modelId, [FromBody] DeleteModelImagesReq req)
        {
            await _modelImageService.DeleteModelImagesAsync(modelId, req.ImageIds);
            return Ok(new { message = Message.CloudinaryMessage.DeleteSuccess });
        }

        // ----------------- MAIN IMAGE -----------------

        [HttpPost("main-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMainImage(Guid modelId, IFormFile file)
        {
            var imageUrl = await _vehicleModelService.UploadMainImageAsync(modelId, file);

            var res = new VehicleModelImageRes
            {
                ModelId = modelId,
                ImageUrl = imageUrl,
                ImagePublicId = $"models/{modelId}/main"
            };

            return Ok(new
            {
                data = res,
                message = Message.CloudinaryMessage.UploadSuccess
            });
        }

        [HttpDelete("main-image")]
        public async Task<IActionResult> DeleteMainImage(Guid modelId)
        {
            await _vehicleModelService.DeleteMainImageAsync(modelId);
            return Ok(new { message = Message.CloudinaryMessage.DeleteSuccess });
        }

        // ----------------- MAIN IMAGE & SUB IMAGE -----------------
        [HttpPost("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var (mainImageUrl, galleryImages) = await _modelImageService.UploadAllModelImagesAsync(modelId, req.Files);
            var response = new
            {
                main = new
                {
                    modelId,
                    imageUrl = mainImageUrl
                },
                gallery = galleryImages.Select(x => new { x.Id, x.Url, x.PublicId })
            };
            return Ok(
                new
                {
                    data = response,
                    Message = Message.CloudinaryMessage.UploadSuccess
                });
        }
    }
}