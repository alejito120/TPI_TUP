using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Application.Services
{
    public class RepairStatusHistoryService : IRepairStatusHistoryService
    {
        private readonly IRepositoryGeneric<RepairStatusHistory> _historyRepository;
        // .
        private readonly IRepositoryGeneric<Repair> _repairRepository;

        public RepairStatusHistoryService(
            IRepositoryGeneric<RepairStatusHistory> historyRepository,
            // .
            IRepositoryGeneric<Repair> repairRepository)
        {
            _historyRepository = historyRepository;
            // .
            _repairRepository = repairRepository;
        }

        public async Task<RepairStatusHistoryResponse?> GetByIdAsync(int id)
        {
            var history = await _historyRepository.GetByIdAsync(id);

            if (history == null)
                throw new NotFoundException(
                    $"No se encontró el historial con ID {id}.");

            // .
            return MapToResponse(history);
        }

        public async Task<IEnumerable<RepairStatusHistoryResponse>> GetAllAsync()
        {
            var histories = await _historyRepository.GetAllAsync();

            // .
            return histories
                .OrderBy(h => h.ChangeDate)
                .ThenBy(h => h.Id)
                .Select(MapToResponse);
        }

        public async Task<IEnumerable<RepairStatusHistoryResponse>> GetByRepairIdAsync(int repairId)
        {
            // .
            if (!await _repairRepository.ExistsAsync(r => r.Id == repairId))
                throw new NotFoundException(
                    $"No se encontró la reparación con ID {repairId}.");

            var histories = await _historyRepository.FindAsync(
                h => h.RepairId == repairId);

            // .
            return histories
                .OrderBy(h => h.ChangeDate)
                .ThenBy(h => h.Id)
                .Select(MapToResponse);
        }

        // .
        private static RepairStatusHistoryResponse MapToResponse(RepairStatusHistory history)
        {
            return new RepairStatusHistoryResponse(
                history.Id,
                history.RepairId,
                history.PreviousState?.ToString() ?? "Sin estado previo",
                history.NewState.ToString(),
                history.ChangeDate,
                history.Observation
            );
        }
    }
}
