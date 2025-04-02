using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _service;
        public UserRoleController(IUserRoleService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAll()
        {
            var userRoles = await _service.GetAllAsync();
            return Ok(userRoles);
        }
        [HttpGet("{userId}/{roleId}")]
        public async Task<ActionResult<UserRole>> GetById(int userId, int roleId)
        {
            var userRole = await _service.GetByIdAsync(userId, roleId);
            return Ok(userRole);
        }
        [HttpPost("userRole")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRole userRole)
        {
            var success = await _service.AddUserRoleAsync(userRole);
            if (!success)
                return BadRequest("Invalid user role");
            return Ok("User role added successfully");
        }
        [HttpPut("userRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRole userRole)
        {
            var success = await _service.UpdateUserRoleAsync(userRole);
            if (!success)
                return BadRequest("Invalid user role");
            return Ok("User role updated successfully");
        }
        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> Delete(int userId, int roleId)
        {
            var success = await _service.DeleteAsync(userId, roleId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content if successfully deleted
        }
    }
}
