using Core.Interfaces;
using System.Diagnostics;
using Velopack;
using Velopack.Sources;

namespace Core.Services
{
    public class VelopackService : IVelopackService
    {
        private readonly UpdateManager _updateManager;

        public VelopackService()
        {
            _updateManager = new UpdateManager(new GithubSource("https://github.com/ninjadevpunk/DataLoggerSLine", accessToken: null, prerelease: true));
        }

        public async Task<UpdateInfo?> CheckForUpdatesAsync()
        {
            try
            {
                return await _updateManager.CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to check for updates: {ex.Message}");
                return null;
            }
        }

        public async Task DownloadUpdateAsync(UpdateInfo updateInfo)
        {
            await _updateManager.DownloadUpdatesAsync(updateInfo);
        }

        public void ApplyUpdateAndRestart(UpdateInfo updateInfo)
        {
            _updateManager.ApplyUpdatesAndRestart(updateInfo);
        }

        public SemanticVersion? GetCurrentVersion()
        {
            try
            {
                return _updateManager.CurrentVersion;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Getting current version failed: {ex.Message}.");
                return null;
            }
        }
    }
}
