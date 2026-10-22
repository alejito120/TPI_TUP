using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Common;
using HardwareTech.Domain.Enums;

namespace HardwareTech.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Nunca se guarda la contraseña en texto plano: acá va el hash (lo usa la autenticación).
        public string PasswordHash { get; set; } = string.Empty;

        public Rol RolUser { get; set; } = Rol.Client;

        // Equipos que posee (cuando el usuario es Cliente)
        public ICollection<EquipmentHardware> Equipment { get; set; } = [];

        // Reparaciones que tiene a cargo (cuando el usuario es Técnico)
        public ICollection<Repair> AssignedRepairs { get; set; } = [];
    }
}
