using Core.Enums;
using Core.Utilities;

namespace Core.DTOs.Cache
{
    public sealed record AndroidCacheDTO : CodingCacheDTO
    {
        public override string ApplicationName => Constants.Android;
        public override LogContext Context => LogContext.AndroidStudio;
        public bool Scope { get; init; }
        public bool IsConsidered { get; init; }
        public DateTime Sync { get; init; }
        public DateTime StartingGradleDaemon { get; init; }
        public DateTime RunBuild { get; init; }
        public DateTime LoadBuild { get; init; }
        public DateTime ConfigureBuild { get; init; }
        public DateTime AllProjects { get; init; }
    }
}
