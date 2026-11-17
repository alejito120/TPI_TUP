
using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;

namespace HardwareTech.Application.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetResponse> CreateAsync(BudgetRequest request);

        Task<BudgetResponse> GetByIdAsync(int id);

        Task<IEnumerable<BudgetResponse>> GetAllAsync();

        Task<BudgetResponse> GetByRepairIdAsync(int repairId);

        Task<BudgetResponse> AcceptAsync(int id);

        Task<BudgetResponse> RejectAsync(int id);

        Task DeleteAsync(int id);
        Task<BudgetUsdResponse> GetBudgetInUsdAsync(int repairId);
    }
}