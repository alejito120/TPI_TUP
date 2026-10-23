using Microsoft.EntityFrameworkCore;
using HardwareTech.Domain.Entities;

namespace HardwareTech.Infraestructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EquipmentHardware> Equipments { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<CashMovement> CashMovements { get; set; }
        public DbSet<RepairStatusHistory> RepairStatusHistories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Budget>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CashMovement>()
                .Property(c => c.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Technician)
                .WithMany(u => u.AssignedRepairs)
                .HasForeignKey(r => r.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
