using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class MovimientoCaja : BaseEntity
    {
        public TipoMovimiento Tipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; } = null!;
    }
}

