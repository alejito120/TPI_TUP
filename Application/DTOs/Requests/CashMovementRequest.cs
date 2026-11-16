using HardwareTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record CashMovementRequest(
        MovementType Type,
        decimal Amount,
        int RepairId  
        );
}
