namespace Core.DTOs.Data
{
    public class NoteDTO
    {
        public int NoteItemId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string NoteContent { get; set; } = string.Empty;
    }
}
