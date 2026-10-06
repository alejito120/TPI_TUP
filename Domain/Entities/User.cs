using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Domain.Entities
{
    internal class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhonbeNumber { get; set; } = string.Empty;
    }
}
