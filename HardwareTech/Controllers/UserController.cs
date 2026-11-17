using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Enums;
using HardwareTech.Extensions;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("rol/{rol}")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetByRol(Rol rol)
        {
            var users = await _userService.GetByRolAsync(rol);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/rol")]
        public async Task<ActionResult<UserResponse>> AssignRol(int id, [FromBody] Rol newRol)
        {
            var user = await _userService.AssignRolAsync(id, newRol);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var adminId = User.GetUserId();
            if (adminId == null)
                return Unauthorized();

            await _userService.DeleteAsync(id, adminId.Value);
            return NoContent();
        }
    }
}