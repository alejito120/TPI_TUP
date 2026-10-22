using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record CashMovementRequest(
        string Type,
        decimal Amount,
        DateTime Date
        );
}
