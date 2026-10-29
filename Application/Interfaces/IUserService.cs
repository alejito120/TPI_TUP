using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    public interface IUserService
    {
        // definimos el metodo principal que se debe usar 
        Task<UserResponse> RegisterAsync(UserRequest request);
    }
}
