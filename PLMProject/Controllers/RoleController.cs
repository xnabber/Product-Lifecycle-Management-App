using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
        {
            var roles = await _service.GetAllAsync();
            return Ok(roles);
        }
        [HttpGet("{roleId}")]
        public async Task<ActionResult<Role>> GetById(int roleId)
        {
            var role = await _service.GetByIdAsync(roleId);
            return Ok(role);
        }
        [Authorize(Roles = "admin")]
        [HttpPost("role")]
        public async Task<IActionResult> AddRole([FromBody] Role role)
        {
            var success = await _service.AddRoleAsync(role);
            if (!success)
                return BadRequest("Invalid role");
            return Ok("Role added successfully");
        }
        [Authorize(Roles = "admin")]
        [HttpPut("role")]
        public async Task<IActionResult> UpdateRole([FromBody] Role role)
        {
            var success = await _service.UpdateRoleAsync(role);
            if (!success)
                return BadRequest("Invalid role");
            return Ok("Role updated successfully");
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            var success = await _service.DeleteAsync(roleId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content if successfully deleted
        }

    }
}
