using HardwareTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    // establece los requisitos minimos que debemos cumpliar a la hora de crear al usuario (no literal, si y solo si lo heredas en la clase pero ya se sabe eso)
    public interface IUserRepository : IRepositoryGeneric<User>
    {
        // para mi estos son los esenciales, abierto a que me lo cuestionen obvio
        Task<bool> ExistByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
    }
}
