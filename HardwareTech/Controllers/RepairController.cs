using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HardwareTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairController : ControllerBase
    {
        private readonly IRepairService _repairService;

        public RepairController(IRepairService repairService)
        {
            _repairService = repairService;
        }

        [HttpPost]
        public async Task<ActionResult<RepairResponse>> Create([FromBody] RepairRequest request)
        {
            var repair = await _repairService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = repair.Id }, repair);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairResponse>>> GetAll()
        {
            var repairs = await _repairService.GetAllAsync();
            return Ok(repairs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RepairResponse>> GetById(int id)
        {
            var repair = await _repairService.GetByIdAsync(id);
            return Ok(repair);
        }

        [HttpGet("state/{state}")]
        public async Task<ActionResult<IEnumerable<RepairResponse>>> GetByState(RepairStatus state)
        {
            var repairs = await _repairService.GetByStateAsync(state);
            return Ok(repairs);
        }

        [HttpGet("technician/{technicianId:int}")]
        public async Task<ActionResult<IEnumerable<RepairResponse>>> GetByTechnician(int technicianId)
        {
            var repairs = await _repairService.GetByTechnicianAsync(technicianId);
            return Ok(repairs);
        }

        [HttpPatch("{id:int}/technician/{technicianId:int}")]
        public async Task<IActionResult> AssignTechnician(int id, int technicianId)
        {
            await _repairService.AssignTechnicianAsync(id, technicianId);
            return NoContent();
        }

        [HttpPatch("{id:int}/state")]
        public async Task<IActionResult> ChangeState(int id, [FromBody] RepairStatus newState)
        {
            await _repairService.ChangeStateAsync(id, newState);
            return NoContent();
        }

        [HttpPatch("{id:int}/complete")]
        public async Task<IActionResult> CompleteRepair(int id)
        {
            await _repairService.CompleteRepairAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repairService.DeleteAsync(id);
            return NoContent();
        }
    }
}