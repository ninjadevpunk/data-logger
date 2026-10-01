using Core.DTOs.Data;

namespace Core.DTOs.Notes
{
    public record NotesViewItemDTO : NotesViewDTO
    {
        public NoteDataDTO NoteDTO { get; init; } = null!;
        public string Content { get; set; } = string.Empty;
    }
}
