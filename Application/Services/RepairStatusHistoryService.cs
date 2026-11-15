using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Application.Services
{
    public class RepairStatusHistoryService : IRepairStatusHistoryService
    {
        private readonly IRepositoryGeneric<RepairStatusHistory> _historyRepository;

        public RepairStatusHistoryService(
            IRepositoryGeneric<RepairStatusHistory> historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<RepairStatusHistoryResponse?> GetByIdAsync(int id)
        {
            var history = await _historyRepository.GetByIdAsync(id);

            if (history == null)
                throw new NotFoundException(
                    $"No se encontró el historial con ID {id}.");

            return new RepairStatusHistoryResponse(
                history.Id,
                history.RepairId,
                history.PreviousState?.ToString() ?? "Sin estado previo",
                history.NewState.ToString(),
                history.ChangeDate,
                history.Observation
            );
        }

        public async Task<IEnumerable<RepairStatusHistoryResponse>> GetAllAsync()
        {
            var histories = await _historyRepository.GetAllAsync();

            return histories.Select(history =>
                new RepairStatusHistoryResponse(
                    history.Id,
                    history.RepairId,
                    history.PreviousState?.ToString() ?? "Sin estado previo",
                    history.NewState.ToString(),
                    history.ChangeDate,
                    history.Observation
                ));
        }

        public async Task<IEnumerable<RepairStatusHistoryResponse>> GetByRepairIdAsync(int repairId)
        {
            var histories = await _historyRepository.FindAsync(
                h => h.RepairId == repairId);

            return histories.Select(history =>
                new RepairStatusHistoryResponse(
                    history.Id,
                    history.RepairId,
                    history.PreviousState?.ToString() ?? "Sin estado previo",
                    history.NewState.ToString(),
                    history.ChangeDate,
                    history.Observation
                ));
        }
    }
}
