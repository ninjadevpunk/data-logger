namespace Core.DTOs.Data
{
    public record ProjectDataDTO : ProjectDTO
    {
        public int ProjectId { get; init; }
        public int AppId { get; init; }
    }
}
