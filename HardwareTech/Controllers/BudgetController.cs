using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HardwareTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost]
        public async Task<ActionResult<BudgetResponse>> Create([FromBody] BudgetRequest request)
        {
            var budget = await _budgetService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetResponse>>> GetAll()
        {
            var budgets = await _budgetService.GetAllAsync();
            return Ok(budgets);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BudgetResponse>> GetById(int id)
        {
            var budget = await _budgetService.GetByIdAsync(id);
            return Ok(budget);
        }

        [HttpGet("repair/{repairId:int}")]
        public async Task<ActionResult<BudgetResponse>> GetByRepairId(int repairId)
        {
            var budget = await _budgetService.GetByRepairIdAsync(repairId);
            return Ok(budget);
        }

        [HttpPatch("{id:int}/accept")]
        public async Task<ActionResult<BudgetResponse>> Accept(int id)
        {
            var budget = await _budgetService.AcceptAsync(id);
            return Ok(budget);
        }

        [HttpPatch("{id:int}/reject")]
        public async Task<ActionResult<BudgetResponse>> Reject(int id)
        {
            var budget = await _budgetService.RejectAsync(id);
            return Ok(budget);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _budgetService.DeleteAsync(id);
            return NoContent();
        }
    }
}