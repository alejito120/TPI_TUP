using HardwareTech.Application.DTOs.Requests;
using HardwareTech.Application.DTOs.Responses;

namespace HardwareTech.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}