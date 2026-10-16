namespace Core.DTOs
{
    public record ProjectDTO
    {
        public string ProjectName { get; init; } = string.Empty;
        public string ApplicationName { get; init; } = string.Empty;
    }
}
