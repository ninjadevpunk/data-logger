using System.Collections;

namespace Core.DTOs.Notes
{
    /// <summary>
    /// DTO representing a checklist, which is a collection of checklist item DTOs.
    /// </summary>
    public record CheckListDTO : IReadOnlyList<CheckListItemDTO>
    {
        public int UserId { get; init; }
        public string ListSubject { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public string ModifiedDate { get; init; } = string.Empty;
        public CheckListItemDTO this[int index] => Items[index];
        public IReadOnlyList<CheckListItemDTO> Items { get; init; } = [];

        public int Count => Items.Count;

        public IEnumerator<CheckListItemDTO> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
