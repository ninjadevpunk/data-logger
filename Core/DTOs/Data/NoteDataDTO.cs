using Core.DTOs.Notes;

namespace Core.DTOs.Data
{
    public sealed record NoteDataDTO : NoteDTO
    {
        public int NoteItemId { get; init; }
    }
}
