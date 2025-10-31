using Application.Abstractions;
using Application.Dtos.Brand.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/brand")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _brandService.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _brandService.GetAllAsync());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _brandService.DeleteAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BrandReq dto)
        {
            return Ok(await _brandService.CreateAsync(dto));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBrandReq dto)
        {
            return Ok(await _brandService.UpdateAsync(id, dto));
        }
    }
}
