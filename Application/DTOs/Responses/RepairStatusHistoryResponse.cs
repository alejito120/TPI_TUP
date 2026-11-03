using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record RepairStatusHistoryResponse(
        int Id,
        int RepairId,
        string PreviousState,
        string NewState,
        DateTime ChangeDate,
        string Observation

        );
}
