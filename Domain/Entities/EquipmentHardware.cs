using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class Equipo : BaseEntity
    {
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;

        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; } = null!;

        public ICollection<Reparacion> Reparaciones { get; set; } = new List<Reparacion>();
    }

}
