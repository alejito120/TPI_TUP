using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using HardwareTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    public interface IUserService
    {
        // definimos el metodo principal que se debe usar 
        Task<UserResponse> RegisterAsync(UserRequest request);
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> GetByIdAsync(int id);
        Task<IEnumerable<UserResponse>> GetByRolAsync(Rol rol);
        Task<UserResponse> AssignRolAsync(int userId, Rol newRol);
        Task DeleteAsync(int userIdToDelete, int adminId);
    }
}
