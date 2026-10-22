using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class EquipmentHardware : BaseEntity
    {
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;

        public int ClientId { get; set; }
        public User Client { get; set; } = null!;

        public ICollection<Repair> Reparaciones { get; set; } = [];
    }

}
