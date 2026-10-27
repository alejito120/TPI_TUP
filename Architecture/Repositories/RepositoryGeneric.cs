using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HardwareTech.Application.Interfaces;
using HardwareTech.Infraestructure.Data;

namespace HardwareTech.Infraestructure.Repositories
{
    internal class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class //Volvemos a definir que T asume la reresentacion de una Clase
    {
        private readonly ApplicationDbContext _context; // EL CONTEXT NOS PERMITE LA INTERVENCION SOBRE LA BDD
        private readonly DbSet<T> _dbSet; // EF ASUME UNA REPRESENTACION DE LA ENTIDAD SOBRE LA QUE QUEREMOS TRABAJAR

        public RepositoryGeneric(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); //Busca por la pk de la entidad, si T es Products, buscaria por Id.
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); //Todos los registros de la entidad referenciada, si es Products, trae todos los productos.
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); //Agrega la entidad

            await _context.SaveChangesAsync(); //Ejecuta el insert a la BDD
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);

            await _context.SaveChangesAsync(); //Como la interfaz define el metodo void y no task, el impacto sobre la bdd necesitamos resolverlo asincronamente
                                               //por eso añadimos el await
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);

            await _context.SaveChangesAsync(); //Exactamente lo mismo que en el Update
        }
    }
}
