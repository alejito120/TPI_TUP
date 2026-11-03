using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record RepairRequest(
    string ProblemDescription,
    int EquipmentId,
    int TechnicianId
    );
}
