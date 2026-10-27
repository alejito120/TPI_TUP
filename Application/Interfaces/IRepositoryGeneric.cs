using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Application.Interfaces
{
    public interface IRepositoryGeneric<T> where T : class // T recibe una clase
    {
        //Buscamos producto por id
        Task<T?> GetByIdAsync(int id);

        //Obtenemos todos los productos, sin la necesidad de generar una lista (solo los queremos mostrar)
        Task<IEnumerable<T>> GetAllAsync();

        //Añadimos un nuevo producto, el ID ya se pasa dentro de las propiedades de la clase
        Task AddAsync(T entity);

        //No se usa Task ya que no es una operacio asincrona, lo que hacemos es guardar el nuevo valor a actualizar o borrar,
        //la implementacion se hace en Infraestructure
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
