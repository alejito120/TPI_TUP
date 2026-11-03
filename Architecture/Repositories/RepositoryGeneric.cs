using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HardwareTech.Application.Interfaces;
using HardwareTech.Domain.Common;
using HardwareTech.Infraestructure.Data;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace HardwareTech.Infraestructure.Repositories
{
    // Implementación genérica con EF Core: sirve para cualquier entidad que herede de BaseEntity.
    internal class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : BaseEntity
    {
        // El contexto es nuestra puerta de acceso a la base de datos.
        private readonly ApplicationDbContext _context;

        // DbSet<T> representa la tabla de la entidad T (si T es EquipmentHardware, la tabla de equipos).
        private readonly DbSet<T> _dbSet;

        public RepositoryGeneric(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // FindAsync de EF busca por clave primaria (el Id). Devuelve null si no existe.
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // AsNoTracking: como solo se va a leer, EF no necesita vigilar los objetos y la consulta es más rápida.
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        // Where traduce la condición a SQL y la base devuelve solo los registros que cumplen.
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        // AnyAsync genera un EXISTS en SQL: responde si hay al menos un registro, sin traer datos.
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);      // Marca la entidad como nueva en memoria.
            await _context.SaveChangesAsync();  // Ejecuta el INSERT; acá la base genera el Id.
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);              // Marca la entidad como modificada en memoria.
            await _context.SaveChangesAsync();  // Ejecuta el UPDATE; el await espera a que la base termine.
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);              // Marca la entidad para eliminar en memoria.
            await _context.SaveChangesAsync();  // Ejecuta el DELETE.
        }
    }
}
