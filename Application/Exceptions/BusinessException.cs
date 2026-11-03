using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Exceptions
{
    public class BusinessException : Exception //Cuando rompe una regla del negocio, por ejemplo, la reparacion ya fue finalizada.
    {
        public BusinessException(string message)
            : base(message)
        {
        }
    }
}