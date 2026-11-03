using HardwareTech.Application.Interfaces;
using HardwareTech.Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareTech.Infraestructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        // Registra los repositorios de la capa Infraestructure en el contenedor de dependencias.
        // Se llama desde Program.cs: builder.Services.AddRepositories();
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Una sola línea registra el repositorio genérico para TODAS las entidades:
            // cuando un servicio pida IRepositoryGeneric<EquipmentHardware>, el contenedor crea RepositoryGeneric<EquipmentHardware>.
            // Scoped = una instancia por request HTTP, igual que el DbContext.
            services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            

            return services;
        }
    }
}

