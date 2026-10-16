namespace Core.DTOs
{
    public record SubjectDTO
    {
        public int UserId { get; set; }
        public string SubjectName { get; init; } = string.Empty;
    }
}
