using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class Reparacion : BaseEntity
    {
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public EstadoReparacion Estado { get; set; } = EstadoReparacion.Recibido;
        public string DescripcionProblema { get; set; } = string.Empty;

        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; } = null!;

        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; } = null!;

        public Presupuesto? Presupuesto { get; set; }
        public ICollection<HistorialEstadoReparacion> Historial { get; set; } = new List<HistorialEstadoReparacion>();
        public ICollection<MovimientoCaja> Movimientos { get; set; } = new List<MovimientoCaja>();
    }

}
