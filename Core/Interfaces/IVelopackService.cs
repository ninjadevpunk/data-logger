using Velopack;

namespace Core.Interfaces
{
    public interface IVelopackService
    {
        SemanticVersion? GetCurrentVersion();

        Task<UpdateInfo?> CheckForUpdatesAsync();
        Task DownloadUpdateAsync(UpdateInfo updateInfo);
        void ApplyUpdateAndRestart(UpdateInfo updateInfo);
    }
}
