using Data_Logger_1._3.Models.App_Models;
using Microsoft.Extensions.Configuration;

namespace Data_Logger_1._3.Services
{
    public class AppSettingsService
    {
        public AppSettings Settings { get; }

        public AppSettingsService(IConfiguration configuration)
        {
            Settings = new AppSettings
            {
                Version = configuration["App:Version"] ?? string.Empty
            };
        }
    }
}
