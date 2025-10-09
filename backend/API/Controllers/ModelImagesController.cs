using Application.Abstractions;
using Application.Dtos.VehicleModel.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/vehicle-models/{modelId:guid}/images")]
    public class ModelImagesController : ControllerBase
    {
        private readonly IModelImageService _modelImageService;

        public ModelImagesController(IModelImageService modelImageService)
        {
            _modelImageService = modelImageService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadModelImages(Guid modelId, [FromForm] UploadModelImagesReq req)
        {
            var result = await _modelImageService.UploadModelImagesAsync(modelId, req.Files);
            return Ok(result.Select(x => new { x.Id, x.Url, x.PublicId }));
        }

        [HttpDelete]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteModelImages(Guid modelId, [FromBody] DeleteModelImagesReq req)
        {
            await _modelImageService.DeleteModelImagesAsync(modelId, req.ImageIds);
            return NoContent();
        }
    }
}