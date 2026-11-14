using HardwareTech.Application.Interfaces;
using HardwareTech.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareTech.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IRepairService, RepairService>();
            services.AddScoped<IUserService, UserServices>();

            return services;
        }
    }
}
