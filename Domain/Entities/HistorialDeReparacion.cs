using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Domain.Entities
{
    public class HistorialEstadoReparacion : BaseEntity
    {
        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; } = null!;

        // Nulo en el primer registro de una reparación (todavía no había estado anterior)
        public EstadoReparacion? EstadoAnterior { get; set; }
        public EstadoReparacion EstadoNuevo { get; set; }

        public DateTime FechaCambio { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}
