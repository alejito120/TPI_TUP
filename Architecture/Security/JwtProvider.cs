using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HardwareTech.Infraestructure.Security
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // 1. Leemos las configuraciones de appsettings.json
            var secretKey = _configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("La clave secreta 'JwtSettings:Secret' no está configurada.");
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var durationInMinutes = int.Parse(_configuration["JwtSettings:DurationInMinutes"] ?? "120");

            // 2. Preparamos la clave de seguridad para firmar
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Cargamos los Claims (la identidad que viaja adentro del token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                // Aseguramos el claim del Rol tanto en formato URI como en formato corto "role"
                new Claim(ClaimTypes.Role, user.RolUser.ToString()),
                new Claim("role", user.RolUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 4. Creamos el objeto del token con emisor, audiencia, expiración y firma
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(durationInMinutes),
                signingCredentials: credentials
            );

            // 5. Convertimos el token a string (formato estándar eyJhbGciOi...)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
