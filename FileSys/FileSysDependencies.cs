using FileSys.Interfaces;
using FileSys.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileSys
{
    public static class FileSysDependencies
    {
        public static IServiceCollection AddFileSysDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService>(services => new CacheMaster());
            services.AddSingleton<IInstallationRegistry>(services => new InstallationRegistry());

            return services;
        }
    }
}
