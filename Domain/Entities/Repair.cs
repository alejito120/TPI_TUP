using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Domain.Entities
{
    public class Repair : BaseEntity
    {
        public DateTime DateJoined { get; set; }
        public DateTime? CompletionDate { get; set; }
        public RepairStatus State { get; set; } = RepairStatus.Received;
        public string ProblemDescription { get; set; } = string.Empty;
        public int EquipmentID { get; set; }
        public EquipmentHardware Equipment { get; set; } = null!;

        public int TechnicianId { get; set; }
        public User Technician { get; set; } = null!;

        public Budget? Budget { get; set; }
        public ICollection<RepairStatusHistory> Record { get; set; } = [];
        public ICollection<CashMovement> Movimientos { get; set; } = [];
    }

}
