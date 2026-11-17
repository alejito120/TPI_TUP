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

            // 1. Migrar contraseñas viejas que hayan quedado en texto plano
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

            // 2. Usuarios del Sistema (Admin, Técnico y Clientes)
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@hardwaretech.com");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Name = "Administrador",
                    LastName = "General",
                    Email = "admin@hardwaretech.com",
                    PhoneNumber = "1122334455",
                    RolUser = Rol.Admin,
                    PasswordHash = hasher.HashPassword("Admin1234!")
                };
                await context.Users.AddAsync(adminUser);
            }

            var techUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "tecnico@hardwaretech.com");
            if (techUser == null)
            {
                techUser = new User
                {
                    Name = "Martín",
                    LastName = "Fierro",
                    Email = "tecnico@hardwaretech.com",
                    PhoneNumber = "1198765432",
                    RolUser = Rol.Technical,
                    PasswordHash = hasher.HashPassword("Tecnico1234!")
                };
                await context.Users.AddAsync(techUser);
            }

            var client1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "carlos.gomez@gmail.com");
            if (client1 == null)
            {
                client1 = new User
                {
                    Name = "Carlos",
                    LastName = "Gómez",
                    Email = "carlos.gomez@gmail.com",
                    PhoneNumber = "1133445566",
                    RolUser = Rol.Client,
                    PasswordHash = hasher.HashPassword("Carlos1234!")
                };
                await context.Users.AddAsync(client1);
            }

            var client2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "laura.martinez@gmail.com");
            if (client2 == null)
            {
                client2 = new User
                {
                    Name = "Laura",
                    LastName = "Martínez",
                    Email = "laura.martinez@gmail.com",
                    PhoneNumber = "1177889900",
                    RolUser = Rol.Client,
                    PasswordHash = hasher.HashPassword("Laura1234!")
                };
                await context.Users.AddAsync(client2);
            }

            await context.SaveChangesAsync();

            // 3. Equipos de Hardware
            if (!await context.Equipments.AnyAsync())
            {
                var eq1 = new EquipmentHardware
                {
                    Type = "Notebook",
                    Brand = "Lenovo",
                    Model = "ThinkPad E14 Gen 4",
                    SerialNumber = "LNV-THK-8921",
                    ClientId = client1.Id
                };

                var eq2 = new EquipmentHardware
                {
                    Type = "PC Gamer",
                    Brand = "Asus",
                    Model = "ROG Strix G15",
                    SerialNumber = "ASUS-ROG-4412",
                    ClientId = client1.Id
                };

                var eq3 = new EquipmentHardware
                {
                    Type = "Notebook",
                    Brand = "Apple",
                    Model = "MacBook Air M1",
                    SerialNumber = "APL-MBA-9901",
                    ClientId = client2.Id
                };

                await context.Equipments.AddRangeAsync(eq1, eq2, eq3);
                await context.SaveChangesAsync();

                // 4. Reparaciones
                var rep1 = new Repair
                {
                    EquipmentID = eq1.Id,
                    TechnicianId = techUser.Id,
                    ProblemDescription = "No enciende tras corte de energía. Falla en circuito de carga.",
                    DateJoined = DateTime.UtcNow.AddDays(-5),
                    State = RepairStatus.InProgress
                };

                var rep2 = new Repair
                {
                    EquipmentID = eq2.Id,
                    TechnicianId = techUser.Id,
                    ProblemDescription = "Mantenimiento preventivo, limpieza profunda y cambio de pasta térmica en GPU.",
                    DateJoined = DateTime.UtcNow.AddDays(-8),
                    CompletionDate = DateTime.UtcNow.AddDays(-1),
                    State = RepairStatus.Completed
                };

                var rep3 = new Repair
                {
                    EquipmentID = eq3.Id,
                    TechnicianId = techUser.Id,
                    ProblemDescription = "Derrame superficial de café sobre teclado. Teclas pegadas.",
                    DateJoined = DateTime.UtcNow.AddDays(-2),
                    State = RepairStatus.Received
                };

                await context.Repairs.AddRangeAsync(rep1, rep2, rep3);
                await context.SaveChangesAsync();

                // 5. Historial de Estados
                var histories = new List<RepairStatusHistory>
                {
                    new()
                    {
                        RepairId = rep1.Id,
                        PreviousState = null,
                        NewState = RepairStatus.Received,
                        ChangeDate = DateTime.UtcNow.AddDays(-5),
                        Observation = "Equipo recibido en recepción con cargador original."
                    },
                    new()
                    {
                        RepairId = rep1.Id,
                        PreviousState = RepairStatus.Received,
                        NewState = RepairStatus.Quoted,
                        ChangeDate = DateTime.UtcNow.AddDays(-4),
                        Observation = "Diagnóstico completado: MOSFET de entrada en corto."
                    },
                    new()
                    {
                        RepairId = rep1.Id,
                        PreviousState = RepairStatus.Quoted,
                        NewState = RepairStatus.InProgress,
                        ChangeDate = DateTime.UtcNow.AddDays(-3),
                        Observation = "Presupuesto aceptado por el cliente. En banco de trabajo."
                    },
                    new()
                    {
                        RepairId = rep2.Id,
                        PreviousState = null,
                        NewState = RepairStatus.Received,
                        ChangeDate = DateTime.UtcNow.AddDays(-8),
                        Observation = "Ingreso para mantenimiento completo."
                    },
                    new()
                    {
                        RepairId = rep2.Id,
                        PreviousState = RepairStatus.Received,
                        NewState = RepairStatus.InProgress,
                        ChangeDate = DateTime.UtcNow.AddDays(-6),
                        Observation = "Limpieza de coolers y cambio de pasta Arctic MX-4."
                    },
                    new()
                    {
                        RepairId = rep2.Id,
                        PreviousState = RepairStatus.InProgress,
                        NewState = RepairStatus.Completed,
                        ChangeDate = DateTime.UtcNow.AddDays(-1),
                        Observation = "Pruebas de estrés aprobadas (temperatura máxima 65°C)."
                    },
                    new()
                    {
                        RepairId = rep3.Id,
                        PreviousState = null,
                        NewState = RepairStatus.Received,
                        ChangeDate = DateTime.UtcNow.AddDays(-2),
                        Observation = "Ingresado en mostrador. Pendiente de apertura para evaluar líquido."
                    }
                };

                await context.RepairStatusHistories.AddRangeAsync(histories);

                // 6. Presupuestos
                var budget1 = new Budget
                {
                    RepairId = rep1.Id,
                    Amount = 48000.00m,
                    Date = DateTime.UtcNow.AddDays(-4),
                    State = BudgetStatus.Accepted
                };

                var budget2 = new Budget
                {
                    RepairId = rep2.Id,
                    Amount = 22000.00m,
                    Date = DateTime.UtcNow.AddDays(-7),
                    State = BudgetStatus.Accepted
                };

                var budget3 = new Budget
                {
                    RepairId = rep3.Id,
                    Amount = 65000.00m,
                    Date = DateTime.UtcNow.AddDays(-1),
                    State = BudgetStatus.Pending
                };

                await context.Budgets.AddRangeAsync(budget1, budget2, budget3);

                // 7. Movimientos de Caja
                var movements = new List<CashMovement>
                {
                    new()
                    {
                        RepairId = rep1.Id,
                        Type = MovementType.Income,
                        Amount = 24000.00m,
                        Date = DateTime.UtcNow.AddDays(-3)
                    },
                    new()
                    {
                        RepairId = rep1.Id,
                        Type = MovementType.Discharge,
                        Amount = 14500.00m,
                        Date = DateTime.UtcNow.AddDays(-2)
                    },
                    new()
                    {
                        RepairId = rep2.Id,
                        Type = MovementType.Income,
                        Amount = 22000.00m,
                        Date = DateTime.UtcNow.AddDays(-1)
                    }
                };

                await context.CashMovements.AddRangeAsync(movements);
                await context.SaveChangesAsync();
            }
        }
    }
}
