using HardwareTech.Domain.Entities;

namespace HardwareTech.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}