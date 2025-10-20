using API.Filters;
using Application;
using Application.Abstractions;
using Application.Constants;
using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklistItem.Request;
using Application.Dtos.VehicleModel.Respone;
using Domain.Entities;
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
            return Ok(new { id });
        }


        /*
         * status code
         * 200 success
         * 404 not found
         * 403 don't have permission
         * 401 unauthorize
         */
        [HttpPut("{id}")]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> UpdateVehicleChecklist([FromBody] UpdateVehicleChecklistReq req, Guid id)
        {
            await _vehicleChecklistService.UpdateAsync(req, id);
            return Ok();
        }

        /*
         * status code
         * 200 success
         * 404 not found
         * 403 don't have permission
         * 401 unauthorize
         */
        [HttpPut("items/{id}")]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> UpdateVehicleChecklistItems(Guid id, UpdateChecklistItemReq req)
        {
            await _vehicleChecklistService.UpdateItemsAsync(id, req.Status, req.Notes);
            return Ok();

        }
        /*
            * status code
            * 200 success
            * 404 not found
            */
        [HttpGet("{id}")]
        [RoleAuthorize(RoleName.Staff, RoleName.Customer)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = HttpContext.User;
            var checklistViewRes = await _vehicleChecklistService.GetByIdAsync(id, user);
            return Ok(checklistViewRes);
        }

        [HttpGet]
        [RoleAuthorize(RoleName.Staff, RoleName.Customer)]
        public async Task<IActionResult> GetAll(Guid? contractId, int? type)
        {
            var user = HttpContext.User;
            var checklistsViewRes = await _vehicleChecklistService.GetAll(contractId, type, user);
            return Ok(checklistsViewRes);
        }

        [HttpPost("items/{itemId}/image")]
        [RoleAuthorize(RoleName.Staff)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadChecklistItemImage(Guid itemId, [FromForm(Name = "file")] IFormFile file)
        {
            var img = await _imageService.UploadChecklistItemImageAsync(itemId, file);
            return Ok(new { img });
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