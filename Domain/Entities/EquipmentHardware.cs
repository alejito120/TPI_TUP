using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Domain.Entities
{
    public class EquipmentHardware
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User Cliente { get; set; } = null!;

        public ICollection<Reparacion> Reparaciones { get; set; } = new List<Reparacion>();
    }
}
