using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareTech.Infraestructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            // 1. Migrar contraseñas viejas que hayan quedado en texto plano (sin formato salt.hash)
            var legacyUsers = await context.Users
                .Where(u => !u.PasswordHash.Contains("."))
                .ToListAsync();

            if (legacyUsers.Any())
            {
                foreach (var user in legacyUsers)
                {
                    var plainPassword = string.IsNullOrWhiteSpace(user.PasswordHash) ? "123456" : user.PasswordHash;
                    user.PasswordHash = hasher.HashPassword(plainPassword);
                }
                await context.SaveChangesAsync();
            }

            // 2. Asegurar que siempre exista el usuario Administrador del sistema
            var defaultAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@hardwaretech.com");
            if (defaultAdmin == null)
            {
                var adminUser = new User
                {
                    Name = "Administrador",
                    LastName = "General",
                    Email = "admin@hardwaretech.com",
                    PhoneNumber = "1122334455",
                    RolUser = Rol.Admin,
                    PasswordHash = hasher.HashPassword("Admin1234!")
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
