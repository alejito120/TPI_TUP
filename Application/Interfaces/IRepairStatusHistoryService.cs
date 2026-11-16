using HardwareTech.Application.DTOs.Responses;

namespace HardwareTech.Application.Interfaces
{
    public interface IRepairStatusHistoryService
    {
        Task<RepairStatusHistoryResponse?> GetByIdAsync(int id);

        Task<IEnumerable<RepairStatusHistoryResponse>> GetAllAsync();

        Task<IEnumerable<RepairStatusHistoryResponse>> GetByRepairIdAsync(int repairId);
    }
}