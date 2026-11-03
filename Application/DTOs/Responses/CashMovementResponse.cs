using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record CashMovementResponse(
        string Type,
        decimal Amount,
        DateTime Date
        );
}
