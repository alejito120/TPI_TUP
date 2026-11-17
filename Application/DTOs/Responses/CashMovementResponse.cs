using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record CashMovementResponse(
        int Id,
        string Type,
        decimal Amount,
        DateTime Date,
        int RepairId
        ); 
}
