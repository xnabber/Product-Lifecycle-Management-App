using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly IStageService _service;
        public StageController(IStageService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stage>>> GetAll()
        {
            var stages = await _service.GetAllAsync();
            return Ok(stages);
        }
        [HttpGet("{stageId}")]
        public async Task<ActionResult<Stage>> GetById(int stageId)
        {
            var stage = await _service.GetByIdAsync(stageId);
            return Ok(stage);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("stage")]
        public async Task<IActionResult> AddStage([FromBody] Stage stage)
        {
            var success = await _service.AddStageAsync(stage);
            if (!success)
                return BadRequest("Invalid stage");
            return Ok("Stage added successfully");
        }
        [Authorize(Roles = "admin")]
        [HttpPut("stage")]
        public async Task<IActionResult> UpdateStage([FromBody] Stage stage)
        {
            var success = await _service.UpdateStageAsync(stage);
            if (!success)
                return BadRequest("Invalid stage");
            return Ok("Stage updated successfully");
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{stageId}")]
        public async Task<IActionResult> Delete(int stageId)
        {
            var success = await _service.DeleteAsync(stageId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content if successfully deleted
        }

    }
}
