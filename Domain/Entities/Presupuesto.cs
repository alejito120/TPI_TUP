using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class Presupuesto : BaseEntity
    {
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoPresupuesto Estado { get; set; } = EstadoPresupuesto.Pendiente;

        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; } = null!;
    }
}
