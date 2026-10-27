using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HardwareTech.Domain.Common;

namespace HardwareTech.Application.Interfaces
{
    // Contrato de acceso a datos para cualquier entidad del dominio.
    // Los servicios (Application) solo conocen esta interfaz; la implementación con EF Core vive en Infraestructure.
    // T debe heredar de BaseEntity: así solo se puede usar con entidades del dominio y todas tienen Id.
    public interface IRepositoryGeneric<T> where T : BaseEntity
    {
        // Busca una entidad por su Id. Devuelve null si no existe.
        Task<T?> GetByIdAsync(int id);

        // Devuelve todos los registros de la entidad. Pensado para consultas de solo lectura.
        Task<IEnumerable<T>> GetAllAsync();

        // Devuelve los registros que cumplen la condición (puede devolver 0, 1 o varios).
        // Ejemplo: FindAsync(e => e.ClientId == 5)
        // La condición es una Expression para que EF la traduzca a SQL (WHERE) y filtre en la base de datos.
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Indica si existe al menos un registro que cumple la condición. No trae los datos, solo responde true o false.
        // Ejemplo: ExistsAsync(u => u.Email == "ana@mail.com")
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Agrega la entidad y la guarda en la base.
        // El Id lo genera la base: después de guardar, queda cargado en el mismo objeto que se pasó.
        Task AddAsync(T entity);

        // Actualiza la entidad en la base. Es asíncrona porque impacta en la base de datos.
        Task UpdateAsync(T entity);

        // Elimina la entidad de la base. Es asíncrona por el mismo motivo que UpdateAsync.
        Task DeleteAsync(T entity);
    }
}
