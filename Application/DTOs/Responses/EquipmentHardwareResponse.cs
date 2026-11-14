using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record EquipmentHardwareResponse(
        int Id,
        string Type,
        string Brand,
        string Model,
        String SerialNumber,
        int ClientId
        );
}
