using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class SaveChecklistCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly CreateCheckListViewModel _createCheckListViewModel;
        private readonly NOTESViewModel _notesViewModel;


        public SaveChecklistCommand(NavigationService navigationService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            }
            catch (Exception)
            {
                //
            }
        }

        public SaveChecklistCommand(NavigationService navigationService, IDataService dataService, CreateCheckListViewModel createCheckListViewModel, NOTESViewModel notesViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _createCheckListViewModel = createCheckListViewModel ?? throw new ArgumentNullException(nameof(createCheckListViewModel));
                _notesViewModel = notesViewModel ?? throw new ArgumentNullException(nameof(notesViewModel));

            }
            catch (Exception)
            {
                //
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                NoteItem noteItem = new();


                ACCOUNT account = _dataService.GetUser();
                ApplicationClass DataLoggerNotesApp = await _dataService.FindApplicationByID(15);

                noteItem.Author = account;
                noteItem.Project = new(-1, account, "", DataLoggerNotesApp,
                    LOG.CATEGORY.NOTES, false);

                noteItem.Application = DataLoggerNotesApp;
                noteItem.Start = DateTime.Parse(_createCheckListViewModel.CreationDate);
                noteItem.End = DateTime.Parse(_createCheckListViewModel.ModifiedDate);
                noteItem.Output = await _dataService.FindOutputByID(37);
                noteItem.Type = await _dataService.FindTypeByID(39);

                noteItem.Subject = _createCheckListViewModel.NoteSubject;

                noteItem.Checklist = new();
                
                foreach(var item in _createCheckListViewModel.ChecklistItems)
                {
                    CheckListItem checkListItem = new CheckListItem(-1, item.IsDone, item.Item);
                    noteItem.Checklist.Items.Add(checkListItem);
                }




                // Send to database
                await _dataService.CreateLOG(noteItem);

                var list = _notesViewModel.NoteItems;
                list.Add(new NoteLOGViewModel(_dataService, _notesViewModel, noteItem));

                _notesViewModel.NoteItems = list;

                await _navigationService.NavigateToNotesDashboard();
                _navigationService.SetChecklistNotesChecked(false);
            }
            catch (Exception)
            {

                //
            }
        }
    }
}
