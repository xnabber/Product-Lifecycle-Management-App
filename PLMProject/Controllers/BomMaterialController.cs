using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BomMaterialController : ControllerBase
    {
        private readonly IBomMaterialService _service;

        public BomMaterialController(IBomMaterialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BomMaterial>>> GetAll()
        {
            var bomMaterials = await _service.GetAllAsync();
            return Ok(bomMaterials);
        }

        [HttpGet("{bomId}/{materialId}")]
        public async Task<ActionResult<BomMaterial>> GetById(int bomId, string materialId)
        {
            var bomMaterial = await _service.GetByIdAsync(bomId, materialId);
            if (bomMaterial == null) return NotFound();
            return Ok(bomMaterial);
        }

        [HttpGet("bom/{bomId}")]
        public async Task<ActionResult<IEnumerable<BomMaterial>>> GetByBom(int bomId)
        {
            var bomMaterials = await _service.GetByBom(bomId);
            return Ok(bomMaterials);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddBomMaterial([FromBody] BomMaterialDto bomMaterial)
        {
            var success = await _service.AddBomMaterialAsync(bomMaterial);
            if (success == null) return BadRequest("Invalid BOM or Material");
            return Ok(success);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateBomMaterial([FromBody] BomMaterialDto bomMaterial)
        {
            var success = await _service.UpdateBomMaterialAsync(bomMaterial);
            if (success == null) return BadRequest("Invalid BOM or Material");
            return Ok(success);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{bomId}/{materialId}")]
        public async Task<IActionResult> Delete(int bomId, string materialId)
        {
            var success = await _service.DeleteAsync(bomId, materialId);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
