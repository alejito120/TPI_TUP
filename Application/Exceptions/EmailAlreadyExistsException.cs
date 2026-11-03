using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Exceptions
{
    // esto se llama exprecion semantica (por ahi si lo buscas el google se puede entender mejor)
    // esta se lanza cuando queremos registrar un usuario con un email que ya existe en la base de datos
    internal class EmailAlreadyExistsException : Exception
    {
        // lo usamos dentro de Userservices
        public EmailAlreadyExistsException(string email)
            : base($"Ya existe un usuario con el Email {email}.") { }
    }
}
