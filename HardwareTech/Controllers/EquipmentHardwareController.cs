using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HardwareTech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentHardwareController : ControllerBase
    {
        private readonly IEquipmentHardwareService _equipmentService;

        public EquipmentHardwareController(IEquipmentHardwareService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentHardwareResponse>> Create([FromBody] EquipmentHardwareRequest request)
        {
            var equipment = await _equipmentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, equipment);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentHardwareResponse>>> GetAll()
        {
            var equipments = await _equipmentService.GetAllAsync();
            return Ok(equipments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EquipmentHardwareResponse>> GetById(int id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);
            return Ok(equipment);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<IEnumerable<EquipmentHardwareResponse>>> GetByClientId(int clientId)
        {
            var equipments = await _equipmentService.GetByClientIdAsync(clientId);
            return Ok(equipments);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EquipmentHardwareResponse>> Update(int id, [FromBody] EquipmentHardwareRequest request)
        {
            var equipment = await _equipmentService.UpdateAsync(id, request);
            return Ok(equipment);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _equipmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}