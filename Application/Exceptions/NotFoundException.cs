using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Exceptions
{
    public class NotFoundException : Exception  //La utilizamos cuando algo no existe
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}