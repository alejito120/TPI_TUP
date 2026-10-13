using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<User> Usuarios { get; set; } = new List<User>();
    }
}

