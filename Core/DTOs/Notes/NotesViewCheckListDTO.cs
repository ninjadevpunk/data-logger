using Core.DTOs.Data;
using System.Collections;

namespace Core.DTOs.Notes
{
    public record NotesViewCheckListDTO : NotesViewDTO, IReadOnlyList<CheckListItemDTO>
    {
        public CheckListDataDTO NoteDTO { get; init; } = null!;
        public CheckListItemDTO this[int index] => Items[index];

        public IReadOnlyList<CheckListItemDTO> Items { get; set; } = [];

        public int Count => Items.Count;

        public IEnumerator<CheckListItemDTO> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
