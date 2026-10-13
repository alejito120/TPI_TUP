using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Domain.Entities
{
    public class MovimientoCaja
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // Ingreso/Egreso
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; } = null!;
    }
}
