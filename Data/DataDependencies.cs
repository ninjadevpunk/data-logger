using Core.Interfaces;
using Data.Interfaces;
using Data.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
#if RELEASE
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
#endif

namespace Data
{
    public static class DataDependencies
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // EntityMaster
            var connectionString = BuildConnectionString(configuration);

            services.AddDbContext<EntityMaster>(options =>
            {
                options.UseSqlite(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information);

#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            // Internal implementation details
            services.AddScoped<EntityReader>();
            services.AddScoped<EntityWriter>();
            services.AddScoped<EntityHandler>();


            // AuthService
            services.AddSingleton<IAuthService>((services) => new AuthService(services));

            // DataService
            services.AddSingleton<IDataService>(services => new DataService(services.GetRequiredService<IAuthService>(), services));

            return services;
        }

        public static string BuildConnectionString(IConfiguration configuration)
        {
            string key = string.Empty;

#if DEBUG
            DotNetEnv.Env.Load();
            key = Environment.GetEnvironmentVariable("DevMode_SQLCipher_Key")!;
#else
            key = RetrieveEncryptionKey();
#endif

            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("SQLCipher key is missing!");

            return new SqliteConnectionStringBuilder
            {
                DataSource = configuration.GetConnectionString("DefaultConnection"),
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = key
            }.ToString();
        }

#if RELEASE
        public static string RetrieveEncryptionKey()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");
            string keyFilePath = Path.Combine(folder, "secret.bin");

            try
            {
                if (!File.Exists(keyFilePath))
                {
                    var newKey = Guid.NewGuid().ToString("N");
#pragma warning disable CA1416 // Validate platform compatibility
                    var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(newKey), null, DataProtectionScope.LocalMachine);

                    File.WriteAllBytes(keyFilePath, encrypted);

                    return newKey;
                }

                var protectedData = File.ReadAllBytes(keyFilePath);
                var decryptedBytes = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.LocalMachine);
#pragma warning restore CA1416 // Validate platform compatibility
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near RetrieveEncryptionKey: {ex.Message}");

                return string.Empty;
            }
        }
#endif
    }
}
