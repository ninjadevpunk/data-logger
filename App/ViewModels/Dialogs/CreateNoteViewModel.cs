using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    internal class CreateNoteViewModel : ViewModelBase
    {


        public CreateNoteViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService, NOTESDashboardViewModel notesViewModel)
        {
            StartandEndDate();
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
                StartandEndDate();
                OnPropertyChanged(nameof(NoteSubject));
            }
        }

        public string CreationDate => DateTime.Now.ToString("dd MMMM yyyy HH:mm");

        private string modifiedDate = string.Empty;
        public string ModifiedDate
        {
            get
            {
                return modifiedDate;
            }
            set
            {
                modifiedDate = value;
                OnPropertyChanged(nameof(ModifiedDate));
            }
        }


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
                StartandEndDate() ;
                OnPropertyChanged(nameof(NoteContent));
            }
        }






        public ICommand SaveNoteCommand { get; set; }






        #region Member Functions





        private string UpdateLastModified() => ModifiedDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm");

        private void StartandEndDate() => Date = $"Created on {CreationDate}. Last modified on {UpdateLastModified()}";



        #endregion
    }
}
