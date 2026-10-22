using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record RepairRequest(
        string ProblemDescription,
        string Equipment,
        string Technician
        );
}
