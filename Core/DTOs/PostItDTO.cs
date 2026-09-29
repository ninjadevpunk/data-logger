namespace Core.DTOs
{
    public record PostItDTO
    {
        public string Subject { get; init; } = string.Empty;
        public string Error { get; init; } = string.Empty;
        public string Solution { get; init; } = string.Empty;
        public string Suggestion { get; init; } = string.Empty;
        public string Comment { get; init; } = string.Empty;
    }
}
