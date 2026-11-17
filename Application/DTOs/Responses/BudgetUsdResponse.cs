namespace HardwareTech.Application.DTOs.Responses
{
    public record BudgetUsdResponse(
        int BudgetId,
        decimal AmountArs,
        decimal DollarRate,
        decimal AmountUsd
    );
}
