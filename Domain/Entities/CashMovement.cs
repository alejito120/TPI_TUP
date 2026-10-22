using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Domain.Entities
{
    public class CashMovement : BaseEntity
    {
        public MovementType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int RepairId { get; set; }
        public Repair Repair { get; set; } = null!;
    }
}

