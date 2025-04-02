using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;
using System.ComponentModel.DataAnnotations;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetById(int productId)
        {
            var product = await _service.GetByIdAsync(productId);
            return Ok(product);
        }
        [Authorize(Roles = "admin")]
        [HttpPost()]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequestDto product)
        {
            var addedProduct = await _service.AddProductAsync(product);
            if (addedProduct == null)
                return BadRequest("Invalid product");

            return Ok(addedProduct);
        }
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductRequestDto product)
        {
            var updatedProduct = await _service.UpdateProductAsync(product);
            if (updatedProduct == null)
                return BadRequest("Invalid product");
            return Ok(updatedProduct);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var success = await _service.DeleteAsync(productId);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }
        [Authorize(Roles = "admin")]
        [HttpGet("grouped-by-stage")]
        public async Task<ActionResult<IDictionary<string, int>>> GetProductsGroupedByCurrentStage()
        {
            var stageCounts = await _service.GetProductsGroupedByCurrentStageAsync();
            return Ok(stageCounts);
        }

        [HttpGet("{productId}/stage-durations")]
        public async Task<ActionResult<List<StageDurationDto>>> GetStageDurations(int productId)
        {
            var stageDurations = await _service.GetProductStageDurationsAsync(productId);

            if (stageDurations == null)
            {
                return NotFound();
            }

            return Ok(stageDurations);
        }
    }


}
