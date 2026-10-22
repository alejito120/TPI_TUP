using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record RepairResponse(
        int Id,
        DateTime DateJoined,
        string State,
        string ProblemDescription,
        string Equipment,
        string Technician
        );
}
