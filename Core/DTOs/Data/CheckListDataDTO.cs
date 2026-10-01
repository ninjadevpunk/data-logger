using Core.DTOs.Notes;

namespace Core.DTOs.Data
{
    /// <summary>
    /// DTO representing a checklist from the database.
    /// </summary>
    public sealed record CheckListDataDTO : CheckListDTO
    {
        public int CheckListId { get; init; }
    }
}
