using HardwareTech.Application.Interfaces;
using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using HardwareTech.Application.Exceptions;

namespace HardwareTech.Application.Services
{

    // establecemos el contrato que queremos usar, para definir que metodos queremos usar en este servicio (estoy redactando con pajora la verdad)
    public class UserServices(IUserRepository user, IPasswordHasher hasher) : IUserService 
    {
        // almacenamos la interfaz del repositorio de usuarios y la interfaz del hasher de contraseñas
        // hacemos referencia a un objeto
        private readonly IUserRepository _user = user;
        private readonly IPasswordHasher _hasher = hasher;

        public async Task<UserResponse> RegisterAsync(UserRequest request)
        {

            // aca normalizamos el corre que entra
            var email = request.Email.Trim().ToLowerInvariant();

            // validamos que el Email no exista en la base de datos
            if (await _user.ExistByEmailAsync(email))
                throw new EmailAlreadyExistsException(email);

            // aca creamos el usuario y lo guardamos en la base de datos
            var user = new Domain.Entities.User
            {
                Name = request.Name.Trim(),
                Email = email,
                PasswordHash = _hasher.HashPassword(request.Password)
            };

            //aca guardamos los datos en la base de datos y nos devuelve la informacion suficiente del usuario creado
            await _user.AddAsync(user);
            

            return new UserResponse(
                user.Id,
                user.Name,
                user.Email,
                user.RolUser.ToString()
            );

        }



    }
}
