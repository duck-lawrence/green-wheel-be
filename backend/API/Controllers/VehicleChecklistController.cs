using API.Filters;
using Application.Abstractions;
using Application.Dtos.VehicleChecklist.Request;
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

        [HttpPost]
        [RoleAuthorize("Staff")]
        public async Task<IActionResult> CreateVehicleChecklist(CreateVehicleChecklistReq req)
        {
            var staff = HttpContext.User;
            var vehicleCheckList = await _vehicleChecklistService.CreateVehicleChecklistAsync(staff, req);
            return Ok(vehicleCheckList);
        }
        [HttpPut]
        [RoleAuthorize("Staff")]
        public async Task<IActionResult> UpdateVehicleChecklist([FromBody]UpdateVehicleChecklistReq req)
        {
            await _vehicleChecklistService.UpdateVehicleChecklistAsync(req);
            return Ok();
        }

        [HttpGet("{id}")]
        [RoleAuthorize("Staff")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var checklistViewRes = await _vehicleChecklistService.GetByIdAsync(id);
            return Ok(checklistViewRes);
        }
    }
}
