using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class CoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IElevationService>(services => new ElevationService());
            services.AddSingleton<IVelopackService>(services => new VelopackService());

            return services;
        }
    }
}
