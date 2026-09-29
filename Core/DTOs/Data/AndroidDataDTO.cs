using Core.Enums;
using Core.Utilities;

namespace Core.DTOs.Data
{
    public record AndroidDataDTO : CodingDataDTO
    {
        public override LogContext Context => LogContext.AndroidStudio;
        public override int AppId => 2;
        public override string ApplicationName => Constants.Android;
        public bool Scope { get; init; }
        public DateTime Sync { get; init; }
        public DateTime StartingGradleDaemon { get; init; }
        public DateTime RunBuild { get; init; }
        public DateTime LoadBuild { get; init; }
        public DateTime ConfigureBuild { get; init; }
        public DateTime AllProjects { get; init; }
    }
}
