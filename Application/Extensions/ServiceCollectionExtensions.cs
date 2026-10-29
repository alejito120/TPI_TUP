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

            return services;
        }
    }
}
