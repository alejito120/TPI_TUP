using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HardwareTech.Application.DTOs.Requests
{
    public record EquipmentHardwareRequest(
        string Type,
        string Brand,
        string Model,
        String SerialNumber,
        int UserId
        );
}
