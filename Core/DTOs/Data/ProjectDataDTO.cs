namespace Core.DTOs.Data
{
    public sealed record ProjectDataDTO : ProjectDTO
    {
        public int ProjectId { get; init; }
        public int AppId { get; init; }
    }
}
