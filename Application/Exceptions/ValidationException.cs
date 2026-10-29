using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Exceptions
{
    public class ValidationException : Exception //la utilizamos cuando el dato viene mal
    {
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}