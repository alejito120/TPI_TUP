using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record BudgetResponse(
        int Id,
        decimal Amount,
        DateTime Date,
        string State,
        int RepairId
        );
}
