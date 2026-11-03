using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    public interface IPasswordHasher
    {
        // generamos el hash de la contraseña que nos llega, para guardarla en la base de datos
        string HashPassword(string password);

        // esto va a comparar la contraseña que ingresa el usuario con la que tenemos guardada en la base de datos, para ver si es la misma
        bool Verify(string password, string hash); // claramente lo vamos a ver hasheada nosotros
    }
}
