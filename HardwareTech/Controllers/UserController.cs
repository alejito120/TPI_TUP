using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HardwareTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Register([FromBody] UserRequest request)
        {
            var user = await _userService.RegisterAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("rol/{rol}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetByRol(Rol rol)
        {
            var users = await _userService.GetByRolAsync(rol);
            return Ok(users);
        }

        [HttpPatch("{id:int}/rol")]
        public async Task<ActionResult<UserResponse>> AssignRol(int id, [FromBody] Rol newRol)
        {
            var user = await _userService.AssignRolAsync(id, newRol);
            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int adminId)
        {
            await _userService.DeleteAsync(id, adminId);
            return NoContent();
        }
    }
}