using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.DTOs.Responses
{
    public record UserResponse(
        int Id,
        string Name,
        string Email,
        int RolUser
     );
}
