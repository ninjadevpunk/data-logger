using Core.DTOs.Data;
using Core.DTOs.Notes;
using Data.Interfaces;
using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    internal sealed class NoteLOGViewModel : ViewModelBase
    {
        private readonly CheckListDataDTO? _checkListDataDTO;
        private readonly NoteDataDTO? _noteDataDTO;
        public ICommand DeleteNoteItemCommand { get; set; } = null!;

        public ICommand EditNoteItemCommand { get; set; } = null!;



        #region Constructors


        public NoteLOGViewModel(IDataService dataService, NOTESDashboardViewModel notesViewModel, NotesViewDTO notesViewDTO)
        {
            if(notesViewDTO is NotesViewCheckListDTO checkListDTO)
                _checkListDataDTO = checkListDTO.NoteDTO;
            else
                _noteDataDTO = ((NotesViewItemDTO)notesViewDTO).NoteDTO;

            Subject = notesViewDTO.Subject;

            if(IsCollection)
            {
                var checkList = _checkListDataDTO?.Items
                    .Where(i => i.IsChecked)
                    .Select(i => i.Item)
                    .ToHashSet();

                if(checkList != null)
                    Items = new(checkList);
            }
            Date = notesViewDTO.CreationDate;

            DeleteNoteItemCommand = new DeleteNoteItemCommand(notesViewModel, dataService, _noteDataDTO?.UserId ?? _checkListDataDTO!.UserId);
        }




        #endregion








        #region Properties



        public bool IsCollection => _checkListDataDTO != null;

        public string Subject { get; init; } = string.Empty;

        public string? NoteContent => _noteDataDTO?.NoteContent;

        public ObservableCollection<string>? Items { get; init; } = null;

        public string Date { get; init; } = string.Empty;




        #endregion




    }
}
