using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;

namespace HardwareTech.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        // Nunca se guarda la contraseña en texto plano: acá va el hash (lo usa la autenticación).
        public string PasswordHash { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        // Equipos que posee (cuando el usuario es Cliente)
        public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();

        // Reparaciones que tiene a cargo (cuando el usuario es Técnico)
        public ICollection<Reparacion> ReparacionesAsignadas { get; set; } = new List<Reparacion>();
    }
}
