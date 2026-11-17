using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Application.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IRepositoryGeneric<Budget> _budgetRepository;
        private readonly IRepositoryGeneric<Repair> _repairRepository;
        private readonly IRepositoryGeneric<RepairStatusHistory> _historyRepository;
        private readonly IDolarApiService _dolarApiService;

        public BudgetService(
            IRepositoryGeneric<Budget> budgetRepository,
            IRepositoryGeneric<Repair> repairRepository,
            IRepositoryGeneric<RepairStatusHistory> historyRepository,
            IDolarApiService dolarApiService)
        {
            _budgetRepository = budgetRepository;
            _repairRepository = repairRepository;
            _historyRepository = historyRepository;
            _dolarApiService = dolarApiService;
        }

        public async Task<BudgetResponse> CreateAsync(BudgetRequest request)
        {
            if (request.Amount <= 0)
                throw new ValidationException("El monto del presupuesto debe ser mayor a cero.");

            var repair = await _repairRepository.GetByIdAsync(request.RepairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {request.RepairId}.");

            if (repair.State == RepairStatus.InProgress ||
                repair.State == RepairStatus.Completed ||
                repair.State == RepairStatus.Rejected)
            {
                throw new BusinessException("Solo se puede emitir un presupuesto para una reparación en estado Recibida o Presupuestada.");
            }

            if (await _budgetRepository.ExistsAsync(b => b.RepairId == request.RepairId))
                throw new BusinessException($"La reparación con ID {request.RepairId} ya tiene un presupuesto asignado.");

            var budget = new Budget
            {
                Amount = request.Amount,
                Date = DateTime.UtcNow,
                State = BudgetStatus.Pending,
                RepairId = request.RepairId
            };

            await _budgetRepository.AddAsync(budget);

            // aca puse para que si es aceptado, pasa a estar en espera y se guarda en el historial
            if (repair.State != RepairStatus.AwaitingAcceptance)
            {
                var previousState = repair.State;
                repair.State = RepairStatus.AwaitingAcceptance;
                await _repairRepository.UpdateAsync(repair);

                await _historyRepository.AddAsync(new RepairStatusHistory
                {
                    RepairId = repair.Id,
                    PreviousState = previousState,
                    NewState = RepairStatus.AwaitingAcceptance,
                    ChangeDate = DateTime.UtcNow,
                    Observation = $"Presupuesto emitido por ${budget.Amount}. Esperando aceptación del cliente."
                });
            }

            return MapToResponse(budget);
        }

        public async Task<BudgetResponse> GetByIdAsync(int id)
        {
            var budget = await _budgetRepository.GetByIdAsync(id);
            if (budget == null)
                throw new NotFoundException($"No se encontró el presupuesto con ID {id}.");

            return MapToResponse(budget);
        }

        public async Task<IEnumerable<BudgetResponse>> GetAllAsync()
        {
            var budgets = await _budgetRepository.GetAllAsync();
            return budgets.Select(MapToResponse);
        }

        public async Task<BudgetResponse> GetByRepairIdAsync(int repairId)
        {
            if (!await _repairRepository.ExistsAsync(r => r.Id == repairId))
                throw new NotFoundException($"No se encontró la reparación con ID {repairId}.");

            var budgets = await _budgetRepository.FindAsync(b => b.RepairId == repairId);
            var budget = budgets.FirstOrDefault();

            if (budget == null)
                throw new NotFoundException($"La reparación con ID {repairId} aún no tiene un presupuesto registrado.");

            return MapToResponse(budget);
        }

        public async Task<BudgetResponse> AcceptAsync(int id)
        {
            return await ProcessBudgetDecisionAsync(
                id,
                BudgetStatus.Accepted,
                RepairStatus.InProgress,
                "Presupuesto aceptado por el cliente. Se inicia la reparación.");
        }

        public async Task<BudgetResponse> RejectAsync(int id)
        {
            return await ProcessBudgetDecisionAsync(
                id,
                BudgetStatus.Rejected,
                RepairStatus.Rejected,
                "Presupuesto rechazado por el cliente. Se cancela la reparación.");
        }

        public async Task DeleteAsync(int id)
        {
            var budget = await _budgetRepository.GetByIdAsync(id);
            if (budget == null)
                throw new NotFoundException($"No se encontró el presupuesto con ID {id}.");

            await _budgetRepository.DeleteAsync(budget);
        }

        private async Task<BudgetResponse> ProcessBudgetDecisionAsync(
            int id,
            BudgetStatus newBudgetState,
            RepairStatus newRepairState,
            string observation)
        {
            var budget = await _budgetRepository.GetByIdAsync(id);
            if (budget == null)
                throw new NotFoundException($"No se encontró el presupuesto con ID {id}.");

            if (budget.State != BudgetStatus.Pending)
                throw new BusinessException($"El presupuesto ya fue procesado anteriormente (Estado actual: {budget.State}).");

            var repair = await _repairRepository.GetByIdAsync(budget.RepairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {budget.RepairId}.");

            budget.State = newBudgetState;
            await _budgetRepository.UpdateAsync(budget);

            if (repair.State != newRepairState)
            {
                var previousState = repair.State;
                repair.State = newRepairState;
                await _repairRepository.UpdateAsync(repair);

                await _historyRepository.AddAsync(new RepairStatusHistory
                {
                    RepairId = repair.Id,
                    PreviousState = previousState,
                    NewState = newRepairState,
                    ChangeDate = DateTime.UtcNow,
                    Observation = observation
                });
            }

            return MapToResponse(budget);
        }

        public async Task<BudgetUsdResponse> GetBudgetInUsdAsync(int repairId)
        {
            if (!await _repairRepository.ExistsAsync(r => r.Id == repairId))
                throw new NotFoundException(
                    $"No se encontró la reparación con ID {repairId}.");

            var budgets = await _budgetRepository.FindAsync(
                b => b.RepairId == repairId);

            var budget = budgets.FirstOrDefault();

            if (budget == null)
                throw new NotFoundException(
                    $"La reparación con ID {repairId} aún no tiene un presupuesto registrado.");

            var dollarRate = await _dolarApiService.GetDollarRateAsync();

            return new BudgetUsdResponse(
                budget.Id,
                budget.Amount,
                dollarRate,
                Math.Round(budget.Amount / dollarRate, 2)
            );
        }

        private static BudgetResponse MapToResponse(Budget budget)
        {
            return new BudgetResponse(
                budget.Id,
                budget.Amount,
                budget.Date,
                budget.State.ToString(),
                budget.RepairId
            );
        }
    }
}