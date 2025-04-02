using Microsoft.AspNetCore.Mvc;
using PLMApp.Services;
using PLMApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PLMApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStageHistoryController : ControllerBase
    {
        private readonly IProductStageHistoryService _service;

        public ProductStageHistoryController(IProductStageHistoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStageHistory>>> GetAll()
        {
            var histories = await _service.GetAllAsync();
            return Ok(histories);
        }

        [HttpGet("{productId}/{stageId}/{startOfStage}")]
        public async Task<ActionResult<ProductStageHistory>> GetById(int productId, int stageId, DateTime startOfStage )
        {
            var history = await _service.GetByIdAsync(productId, stageId, startOfStage);
            return Ok(history);
        }
        [Authorize(Roles = "admin")]
        [HttpPost()]
        public async Task<IActionResult> AddStageTransition([FromBody] ProductStageHistoryDto request)
        {
            var success = await _service.AddStageTransitionAsync(request);
            if (success == null)
                return BadRequest("Invalid request");

            return Ok(success);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{productId}/{stageId}/{startOfStage}")]
        public async Task<IActionResult> Delete(int productId, int stageId, DateTime startOfStage)
        {
            var success = await _service.DeleteAsync(productId, stageId, startOfStage);
            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // 204 No Content if successfully deleted
        }
    }
}
