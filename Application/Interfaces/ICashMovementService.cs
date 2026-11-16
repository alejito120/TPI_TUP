using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Application.Interfaces
{
    public interface ICashMovementService
    {
        Task<CashMovementResponse> CreateAsync(CashMovementRequest request);

        Task<CashMovementResponse> GetByIdAsync(int id);

        Task<IEnumerable<CashMovementResponse>> GetAllAsync();

        Task<IEnumerable<CashMovementResponse>> GetByRepairIdAsync(int repairId);

        Task<IEnumerable<CashMovementResponse>> GetByTypeAsync(MovementType type);

        Task DeleteAsync(int id);
    }
}