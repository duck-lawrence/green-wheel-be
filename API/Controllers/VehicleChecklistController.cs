using API.Filters;
using Application;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleModel.Respone;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/vehicle-checklists")]
    [ApiController]
    public class VehicleChecklistController : ControllerBase
    {
        private readonly IVehicleChecklistService _vehicleChecklistService;
        private readonly IChecklistItemImageService _imageService;

        public VehicleChecklistController(IVehicleChecklistService vehicleChecklistService, IChecklistItemImageService imageService)
        {
            _vehicleChecklistService = vehicleChecklistService;
            _imageService = imageService;
        }

        /*
         * status code
         * 200 success
         *
         */

        [HttpPost]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> CreateVehicleChecklist(CreateVehicleChecklistReq req)
        {
            var staff = HttpContext.User;
            var id = await _vehicleChecklistService.Create(staff, req);
            return Ok(new {id});
        }


        /*
         * status code
         * 200 success
         * 404 not found
         * 403 don't have permission
         * 401 unauthorize
         */
        [HttpPut]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> UpdateVehicleChecklist([FromBody] UpdateVehicleChecklistReq req)
        {
            await _vehicleChecklistService.UpdateAsync(req);
            return Ok();
        }

        /*
         * status code
         * 200 success
         * 404 not found
         */
        [HttpGet("{id}")]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var checklistViewRes = await _vehicleChecklistService.GetByIdAsync(id);
            return Ok(checklistViewRes);
        }

        [HttpPost("items/{itemId}/image")]
        [RoleAuthorize(RoleName.Staff)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadChecklistItemImage(Guid itemId, [FromForm(Name = "file")] IFormFile file)
        {
            var result = await _imageService.UploadChecklistItemImageAsync(itemId, file);
            return Ok(result);
        }

        [HttpDelete("items/{itemId}/image")]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> DeleteChecklistItemImage(Guid itemId)
        {
            var result = await _imageService.DeleteChecklistItemImageAsync(itemId);
            return Ok(result);
        }
    }
}