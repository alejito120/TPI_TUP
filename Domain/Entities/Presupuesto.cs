using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Domain.Entities
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; } = null!;
    }
}
