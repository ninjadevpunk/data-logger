using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    internal class CreateCheckListViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;

        public CreateCheckListViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService, 
            NOTESDashboardViewModel notesViewModel)
        {
            _navigationService = navigationService;

            ChecklistItems = new();

            CreationDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm");
            StartandEndDate();
            NoteSubject = "No Subject";

            AddChecklistItem = new AddChecklistItemCommand(dataService, this);
            SaveChecklistCommand = new SaveChecklistCommand(_navigationService, authService, dataService, this, notesViewModel);
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

        public string CreationDate { get; set; } = string.Empty;

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

        private ObservableCollection<CreateChecklistItemViewModel> checklistItems = [];
        public ObservableCollection<CreateChecklistItemViewModel> ChecklistItems
        {
            get
            {
                return checklistItems;
            }
            set
            {
                checklistItems = value;
                StartandEndDate();
                OnPropertyChanged(nameof(ChecklistItems));
            }
        }

        public ICommand? AddChecklistItem { get; set; }

        public ICommand? SaveChecklistCommand { get; set; }



        #region Member Functions





        private string UpdateLastModified() => ModifiedDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm");

        private void StartandEndDate() => Date = $"Created on {CreationDate}. Last modified on {UpdateLastModified()}";



        #endregion

    }
}
