using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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
                LastName = request.LastName.Trim(),
                PhoneNumber = request.PhoneNumber.Trim(),
                Email = email,
                PasswordHash = _hasher.HashPassword(request.Password)
            };

            //aca guardamos los datos en la base de datos y nos devuelve la informacion suficiente del usuario creado
            await _user.AddAsync(user);
            

            return MapToResponse(user);

        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _user.GetAllAsync();
            return users.Select(MapToResponse);
        }
        
        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _user.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"No se encontró el usuario con ID {id}.");
            return MapToResponse(user);
        }

        public async Task<IEnumerable<UserResponse>> GetByRolAsync(Rol rol)
        {
            var users = await _user.FindAsync(u => u.RolUser == rol);
            return users.Select(MapToResponse);
        }

        public async Task<UserResponse> AssignRolAsync(int userId, Rol newRol)
        {
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"No se encontró el usuario con ID {userId}.");
            user.RolUser = newRol;
            await _user.UpdateAsync(user);
            return MapToResponse(user);
        }

        public async Task DeleteAsync(int userIdToDelete, int adminId)
        {
            // Buscamos al usuario que intenta hacer el borrado
            var admin = await _user.GetByIdAsync(adminId);
            if (admin == null)
                throw new NotFoundException($"No se encontro el usuario administrador con ID {adminId}.");

            // Verificamos que realmente tenga el rol Admin
            if (admin.RolUser != Rol.Admin)
                throw new BusinessException("Solo un administrador puede eliminar usuarios.");

            // Buscamos al usuario a eliminar y lo borramos
            var userToDelete = await _user.GetByIdAsync(userIdToDelete);
            if (userToDelete == null)
                throw new NotFoundException($"No se encontro el usuario con ID {userIdToDelete}.");

            await _user.DeleteAsync(userToDelete);
        }




        // con esto se ahorra codigo, siempre creamos de la misma forma a un usuario
        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse(
                user.Id,
                user.Name,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.RolUser.ToString()
            );
        }

    }
}
