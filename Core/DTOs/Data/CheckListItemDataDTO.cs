using Core.DTOs.Notes;

namespace Core.DTOs.Data
{
    /// <summary>
    /// DTO representing a checklist item from the database.
    /// </summary>
    public record CheckListItemDataDTO : CheckListItemDTO
    {
        public int CheckListItemId { get; init; }
        public int LogId { get; init; }
    }
}
