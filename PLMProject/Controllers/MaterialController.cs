using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _service;
        public MaterialController(IMaterialService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetAll()
        {
            var materials = await _service.GetAllAsync();
            return Ok(materials);
        }
        [HttpGet("{materialId}")]
        public async Task<ActionResult<Material>> GetById(int materialId)
        {
            var material = await _service.GetByIdAsync(materialId);
            return Ok(material);
        }
        [Authorize(Roles = "admin")]
        [HttpPost()]
        public async Task<IActionResult> AddMaterial([FromBody] MaterialDto material)
        {
            var success = await _service.AddMaterialAsync(material);
            if (success == null)
                return BadRequest("Invalid material");
            return Ok(success);
        }
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateMaterial([FromBody] MaterialDto material)
        {
            var success = await _service.UpdateMaterialAsync(material);
            if (success == null)
                return BadRequest("Invalid material");
            return Ok(success);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{materialId}")]
        public async Task<IActionResult> Delete(int materialId)
        {
            var success = await _service.DeleteAsync(materialId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content if successfully deleted
        }

    }
}
