using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Domain.Entities
{
    public class HistorialEstadoReparacion
    {
        public int Id { get; set; }
        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; } = null!;
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}
