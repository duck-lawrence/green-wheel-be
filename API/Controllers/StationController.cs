using Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Manages station-related operations such as listing, creating, and updating stations.
    /// </summary>
    [Route("api/stations")]
    [ApiController]
    public class StationController(IStationService stationService) : ControllerBase
    {
        private readonly IStationService _stationSerivce = stationService;

        /// <summary>
        /// Retrieves all stations available in the system.
        /// </summary>
        /// <returns>List of all stations.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">No stations found.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllStation()
        {
            var stations = await _stationSerivce.GetAllStation();
            return Ok(stations);
        }
    }
}
