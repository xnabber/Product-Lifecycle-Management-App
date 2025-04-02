using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BomController : ControllerBase
    {
        private readonly IBomService _service;
        public BomController(IBomService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bom>>> GetAll()
        {
            var boms = await _service.GetAllAsync();
            return Ok(boms);
        }
        [HttpGet("{bomId}")]
        public async Task<ActionResult<Bom>> GetById(int bomId)
        {
            var bom = await _service.GetByIdAsync(bomId);
            return Ok(bom);
        }
        [Authorize(Roles = "admin")]
        [HttpPost()]
        public async Task<IActionResult> AddBom([FromBody] Bom bom)
        {
            if (string.IsNullOrWhiteSpace(bom.Name))
                return BadRequest("BOM name is required");

            var newBom = await _service.AddBomAsync(bom.Name);
            if (newBom == null)
                return BadRequest("Failed to add BOM");

            return Ok(newBom); // Return the newly created BOM object
        }

        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateBom([FromBody] Bom bom)
        {
            var success = await _service.UpdateBomAsync(bom);
            if (!success)
                return BadRequest("Invalid bom");
            return Ok("Bom updated successfully");
        }
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> Delete(int bomId)
        {
            var success = await _service.DeleteAsync(bomId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content if successfully deleted
        }

    }
}
