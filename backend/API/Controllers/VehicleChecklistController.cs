using API.Filters;
using Application;
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

        public VehicleChecklistController(IVehicleChecklistService vehicleChecklistService)
        {
            _vehicleChecklistService = vehicleChecklistService;
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
            var vehicleCheckList = await _vehicleChecklistService.CreateVehicleChecklist(staff, req);
            return Ok(vehicleCheckList);
        }

        [HttpPut]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> UpdateVehicleChecklist([FromBody] UpdateVehicleChecklistReq req)
        {
            await _vehicleChecklistService.UpdateVehicleChecklistAsync(req);
            return Ok();
        }

        [HttpGet("{id}")]
        [RoleAuthorize(RoleName.Staff)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var checklistViewRes = await _vehicleChecklistService.GetByIdAsync(id);
            return Ok(checklistViewRes);
        }

        //[HttpPost("image")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> UploadImage(Guid modelId, IFormFile file)
        //{
        //}

        //[HttpDelete("image")]
        //public async Task<IActionResult> DeleteImage(Guid modelId)
        //{
        //}
    }
}