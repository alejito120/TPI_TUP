using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HardwareTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Technical")]
    public class CashMovementController : ControllerBase
    {
        private readonly ICashMovementService _movementService;

        public CashMovementController(ICashMovementService movementService)
        {
            _movementService = movementService;
        }

        [HttpPost]
        public async Task<ActionResult<CashMovementResponse>> Create([FromBody] CashMovementRequest request)
        {
            var movement = await _movementService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashMovementResponse>>> GetAll()
        {
            var movements = await _movementService.GetAllAsync();
            return Ok(movements);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CashMovementResponse>> GetById(int id)
        {
            var movement = await _movementService.GetByIdAsync(id);
            return Ok(movement);
        }

        [HttpGet("repair/{repairId:int}")]
        public async Task<ActionResult<IEnumerable<CashMovementResponse>>> GetByRepairId(int repairId)
        {
            var movements = await _movementService.GetByRepairIdAsync(repairId);
            return Ok(movements);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<CashMovementResponse>>> GetByType(MovementType type)
        {
            var movements = await _movementService.GetByTypeAsync(type);
            return Ok(movements);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _movementService.DeleteAsync(id);
            return NoContent();
        }
    }
}