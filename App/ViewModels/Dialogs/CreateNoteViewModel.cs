using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    internal class CreateNoteViewModel : ViewModelBase
    {
        public CreateNoteViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService, 
            NOTESDashboardViewModel notesViewModel)
        {
            NoteSubject = "No Subject";
            NoteContent = string.Empty;

            SaveNoteCommand = new SaveNoteCommand(navigationService, authService, dataService, this, notesViewModel);
        }


        private string noteSubject = string.Empty;
        public string NoteSubject
        {
            get
            {
                return noteSubject;
            }
            set
            {
                noteSubject = value;
                UpdateDates();
                OnPropertyChanged(nameof(NoteSubject));
            }
        }

        public string CreationDate { get; } = Extensions.NoteDateTimeNow();
        public string ModifiedDate => Extensions.NoteDateTimeNow();


        private string date = string.Empty;
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }

        }

        private string noteContent = string.Empty;
        public string NoteContent
        {
            get
            {
                return noteContent;
            }
            set
            {
                noteContent = value;
                UpdateDates();
                OnPropertyChanged(nameof(NoteContent));
            }
        }



        public ICommand SaveNoteCommand { get; set; }






        #region Member Functions


        private void UpdateDates() => Date = Extensions.CreatedAndModifiedDate(CreationDate, ModifiedDate);



        #endregion
    }
}
