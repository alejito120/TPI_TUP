using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record BudgetRequest(
        decimal Amount,
        int RepairId
        );
}
