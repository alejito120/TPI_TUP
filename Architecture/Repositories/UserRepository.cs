using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Entities;
using HardwareTech.Domain.Enums;
using HardwareTech.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HardwareTech.Infraestructure.Repositories
{
    internal class UserRepository : RepositoryGeneric<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetByRolAsync(Rol rol)
        {
            return await _dbSet.AsNoTracking().Where(u => u.RolUser == rol).ToListAsync();
        }
    }
}