namespace Core.DTOs.Notes
{
    public record NotesViewDTO
    {
        public string Subject { get; set; } = string.Empty;
        public string CreationDate { get; set; } = string.Empty;

    }
}
