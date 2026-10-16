namespace Core.DTOs.Viewer
{
    public sealed record AndroidViewDTO : CodingViewDTO
    {
        public DateTime Sync { get; init; }
        public DateTime StartingGradleDaemon { get; init; }
        public DateTime RunBuild { get; init; }
        public DateTime LoadBuild { get; init; }
        public DateTime ConfigureBuild { get; init; }
        public DateTime AllProjects { get; init; }
    }
}
