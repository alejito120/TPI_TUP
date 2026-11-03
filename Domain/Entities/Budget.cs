using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Common;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Domain.Entities
{
    public class Budget : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public RepairStatus State { get; set; } = RepairStatus.Received;

        public int RepairId { get; set; }
        public Repair Repair { get; set; } = null!;
    }
}
