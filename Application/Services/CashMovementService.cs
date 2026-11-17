using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Application.Services
{
    public class CashMovementService : ICashMovementService
    {
        private readonly IRepositoryGeneric<CashMovement> _movementRepository;
        private readonly IRepositoryGeneric<Repair> _repairRepository;

        public CashMovementService(
            IRepositoryGeneric<CashMovement> movementRepository,
            IRepositoryGeneric<Repair> repairRepository)
        {
            _movementRepository = movementRepository;
            _repairRepository = repairRepository;
        }

        public async Task<CashMovementResponse> CreateAsync(CashMovementRequest request)
        {
            if (!Enum.IsDefined(typeof(MovementType), request.Type))
                throw new ValidationException("El tipo de movimiento especificado no es válido.");

            if (request.Amount <= 0)
                throw new ValidationException("El monto del movimiento debe ser mayor a cero.");

            var repair = await _repairRepository.GetByIdAsync(request.RepairId);
            if (repair == null)
                throw new NotFoundException($"No se encontró la reparación con ID {request.RepairId}.");

            if (repair.State == RepairStatus.Rejected)
                throw new BusinessException("No se pueden registrar movimientos de caja en una reparación rechazada.");

            var movement = new CashMovement
            {
                Type = request.Type,
                Amount = request.Amount,
                Date = DateTime.UtcNow,
                RepairId = request.RepairId
            };

            await _movementRepository.AddAsync(movement);

            return MapToResponse(movement);
        }

        public async Task<CashMovementResponse> GetByIdAsync(int id)
        {
            var movement = await _movementRepository.GetByIdAsync(id);
            if (movement == null)
                throw new NotFoundException($"No se encontró el movimiento de caja con ID {id}.");

            return MapToResponse(movement);
        }

        public async Task<IEnumerable<CashMovementResponse>> GetAllAsync()
        {
            var movements = await _movementRepository.GetAllAsync();
            return movements
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Id)
                .Select(MapToResponse);
        }

        public async Task<IEnumerable<CashMovementResponse>> GetByRepairIdAsync(int repairId)
        {
            if (!await _repairRepository.ExistsAsync(r => r.Id == repairId))
                throw new NotFoundException($"No se encontró la reparación con ID {repairId}.");

            var movements = await _movementRepository.FindAsync(m => m.RepairId == repairId);
            return movements
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Id)
                .Select(MapToResponse);
        }

        public async Task<IEnumerable<CashMovementResponse>> GetByTypeAsync(MovementType type)
        {
            if (!Enum.IsDefined(typeof(MovementType), type))
                throw new ValidationException("El tipo de movimiento especificado no es válido.");

            var movements = await _movementRepository.FindAsync(m => m.Type == type);
            return movements
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Id)
                .Select(MapToResponse);
        }

        public async Task DeleteAsync(int id)
        {
            var movement = await _movementRepository.GetByIdAsync(id);
            if (movement == null)
                throw new NotFoundException($"No se encontró el movimiento de caja con ID {id}.");

            await _movementRepository.DeleteAsync(movement);
        }

        private static CashMovementResponse MapToResponse(CashMovement movement)
        {
            return new CashMovementResponse(
                movement.Id,
                movement.Type.ToString(),
                movement.Amount,
                movement.Date,
                movement.RepairId
            );
        }
    }
}
