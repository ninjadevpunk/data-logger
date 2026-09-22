using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class NoteLOGViewModel : ViewModelBase
    {
        public readonly NoteItem _NoteItem;

        public ICommand DeleteNoteItemCommand { get; set; }

        public ICommand EditNoteItemCommand { get; set; }



        #region Constructors


        public NoteLOGViewModel(IDataService dataService, NOTESViewModel notesViewModel, NoteItem noteItem)
        {
            _NoteItem = noteItem;

            IsCollection = _NoteItem != null && _NoteItem.Checklist != null;


            if(_NoteItem is not null && IsCollection)
            {
                Items = new();

                foreach (var item in _NoteItem.Checklist.Items)
                {
                    if(!item.IsChecked)
                        Items.Add(new CheckListItemViewModel(item));
                }

            }

            NoteContent = PostItViewModel.ConvertRtfToPlainText(_NoteItem.NoteContent);

            DeleteNoteItemCommand = new DeleteNoteItemCommand(notesViewModel, dataService);
        }






        #endregion



        #region Properties



        public int ViewModelID => _NoteItem.ID;

        public bool IsCollection { get; set; }

        public string Subject => _NoteItem.Subject;

        public string NoteContent { get; set; }

        public ObservableCollection<CheckListItemViewModel> Items { get; set; }

        public string Date => $"Created {_NoteItem.Start.ToString("d/M/yyyy HH:mm")}";








        #endregion















        #region Member Functions





        
















        #endregion


    }
}
