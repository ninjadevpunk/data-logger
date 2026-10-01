using Core.DTOs.Notes;

namespace Core.DTOs.Data
{
    public record NoteDataDTO : NoteDTO
    {
        public int NoteItemId { get; init; }
    }
}
