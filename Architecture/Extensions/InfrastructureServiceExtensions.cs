using HardwareTech.Application.Interfaces;
using HardwareTech.Infraestructure.Repositories;
using HardwareTech.Infraestructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareTech.Infraestructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}

