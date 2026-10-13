using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Domain.Entities
{
    public class Reparacion
    {
        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string DescripcionProblema { get; set; } = string.Empty;

        public int EquipoId { get; set; }
        public EquipmentHardware Equipo { get; set; } = null!;

        public int TecnicoId { get; set; }
        public User Tecnico { get; set; } = null!;

        public Presupuesto? Presupuesto { get; set; }
        public ICollection<HistorialEstadoReparacion> Historial { get; set; } = new List<HistorialEstadoReparacion>();
        public ICollection<MovimientoCaja> Movimientos { get; set; } = new List<MovimientoCaja>();
    }
}
