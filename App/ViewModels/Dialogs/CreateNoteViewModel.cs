using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreateNoteViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly NavigationService _navigationService;




        public CreateNoteViewModel(NavigationService navigationService, IDataService dataService, NOTESViewModel notesViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            InitialiseCommonFields();

            SaveNoteCommand = new SaveNoteCommand(_navigationService, _dataService, this, notesViewModel);
        }

        public CreateNoteViewModel(NavigationService navigationService, IDataService dataService, NOTESViewModel notesViewModel, NoteLOGViewModel noteLOGViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            InitialiseCommonFields();


            SaveNoteCommand = new SaveNoteCommand(_navigationService, _dataService, this, notesViewModel);

        }

        public void InitialiseCommonFields()
        {
            CreationDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm");
            StartandEndDate();
            NoteSubject = "No Subject";
            NoteContent = "";
        }

        private string noteSubject;
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

        public string CreationDate { get; set; }

        private string modifiedDate;
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


        private string date;
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

        private string noteContent;
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
