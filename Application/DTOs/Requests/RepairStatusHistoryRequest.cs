using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record RepairStatusHistoryRequest(
        int RepairId,
        string PreviousState,
        string NewState,
        string Observation
        );
}
