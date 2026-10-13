using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        public ICollection<EquipmentHardware> Equipos { get; set; } = new List<EquipmentHardware>();
        public ICollection<Reparacion> ReparacionesAsignadas { get; set; } = new List<Reparacion>();
    }
}
