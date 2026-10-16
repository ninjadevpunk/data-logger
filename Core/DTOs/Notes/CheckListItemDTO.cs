namespace Core.DTOs.Notes
{
    /// <summary>
    /// DTO representing a checklist item.
    /// </summary>
    public record CheckListItemDTO
    {
        public int UserId { get; init; }
        public bool IsChecked { get; set; }
        public string Item { get; init; } = string.Empty;
    }
}
