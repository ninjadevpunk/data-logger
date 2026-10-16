using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data.Services
{
    public class EntityMasterFactory : IDesignTimeDbContextFactory<EntityMaster>
    {
        public EntityMaster CreateDbContext(string[] args)
        {
            var appPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "App"));
#if DEBUG
            var envPath = Path.Combine(appPath, "bin", "Debug", "net8.0-windows7.0", ".env");
            DotNetEnv.Env.Load(envPath);
            var key = Environment.GetEnvironmentVariable("DevMode_SQLCipher_Key");
#else
            var key = DataDependencies.RetrieveEncryptionKey();
#endif
            var configuration = new ConfigurationBuilder()
             .SetBasePath(appPath)
#if DEBUG
             .AddJsonFile("appsettings.DevMode.json")
#else
             .AddJsonFile("appsettings.json")
#endif
             .Build();

            

            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("SQLCipher key is missing.");

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = configuration.GetConnectionString("DefaultConnection"),
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = key
            }.ToString();

            var options = new DbContextOptionsBuilder<EntityMaster>()
                .UseSqlite(connectionString)
                .Options;

            return new EntityMaster(options);
        }
    }
}
