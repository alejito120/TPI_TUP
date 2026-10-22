using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Domain.Entities
{
    public class RepairStatusHistory : BaseEntity
    {
        public int RepairId { get; set; }
        public Repair Repair { get; set; } = null!;

        // Nulo en el primer registro de una reparación (todavía no había estado anterior)
        public RepairStatus? PreviousState { get; set; }
        public RepairStatus NewState { get; set; }

        public DateTime ChangeDate { get; set; }
        public string Observation { get; set; } = string.Empty;
    }
}
