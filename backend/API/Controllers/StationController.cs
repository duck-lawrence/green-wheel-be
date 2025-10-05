using Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/stations")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationSerivce;

        public StationController(IStationService stationService)
        {
            _stationSerivce = stationService;
        }

        /*
         * Status code
         * 200: success
         * 404: not found
         */
        [HttpGet]
        public async Task<IActionResult> GetAllStation()
        {
            var stations = await _stationSerivce.GetAllStation();
            return Ok(stations);
        }
    }
}
