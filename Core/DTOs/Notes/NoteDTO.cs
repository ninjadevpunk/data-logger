namespace Core.DTOs.Notes
{
    public record NoteDTO
    {
        public int UserId { get; set; }
        public string Subject { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public string ModifiedDate { get; init; } = string.Empty;
        public string NoteContent { get; init; } = string.Empty;
    }
}
