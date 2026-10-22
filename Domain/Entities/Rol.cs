using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class Rol : BaseEntity
    {
        public string NombreRol { get; set; } = string.Empty;
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}

