using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HardwareTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairStatusHistoryController : ControllerBase
    {
        private readonly IRepairStatusHistoryService _historyService;

        public RepairStatusHistoryController(
            IRepairStatusHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairStatusHistoryResponse>>> GetAll()
        {
            var histories = await _historyService.GetAllAsync();

            return Ok(histories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RepairStatusHistoryResponse>> GetById(int id)
        {
            var history = await _historyService.GetByIdAsync(id);

            return Ok(history);
        }

        [HttpGet("repair/{repairId:int}")]
        public async Task<ActionResult<IEnumerable<RepairStatusHistoryResponse>>> GetByRepairId(int repairId)
        {
            var histories = await _historyService.GetByRepairIdAsync(repairId);

            return Ok(histories);
        }
    }
}
