using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMApp.Models;
using PLMProject.Services;
using System.ComponentModel.DataAnnotations;

namespace PLMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetById(int userId)
        {
            var user = await _service.GetByIdAsync(userId);
            return Ok(user);
        }
        [HttpGet("by-email")]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            var user = await _service.GetByEmailAsync(email);
            return Ok(user);
        }
        [HttpPost("user")]
        public async Task<IActionResult> AddUser([FromBody] UserRegister userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _service.UserExistsAsync(userDto.Email);
            if (userExists)
                return BadRequest("A user with this email already exists.");

            var success = await _service.AddUserAsync(userDto.Email,userDto.Name, userDto.Password, userDto.PhoneNumber);
            if (!success)
                return StatusCode(500, "An error occurred while creating the user.");

            return Ok(new { message = "User registered successfully!" });
        }

        [Authorize]
        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var success = await _service.UpdateUserAsync(user);
            if (!success)
                return BadRequest("Invalid user");
            return Ok("User updated successfully");
        }
        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var success = await _service.DeleteAsync(userId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content if successfully deleted
        }
    }

    public class UserRegister
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }

}
