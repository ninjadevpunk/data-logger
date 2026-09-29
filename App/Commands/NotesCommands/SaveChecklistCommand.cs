using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class SaveChecklistCommand : AsyncCommandBase
    {
        private readonly NavigationService? _navigationService;
        private readonly IDataService? _dataService;
        private readonly CreateCheckListViewModel? _createCheckListViewModel;
        private readonly NOTESViewModel? _notesViewModel;


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

        public SaveChecklistCommand(NavigationService? navigationService, IDataService? dataService, CreateCheckListViewModel createCheckListViewModel, NOTESViewModel notesViewModel)
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

                if (_dataService == null || _navigationService == null)
                    throw new InvalidOperationException("Navigation and DataService is null!");

                var user = _dataService?.GetUser();

                ACCOUNT? account = user;

                if (account == null)
                    throw new InvalidOperationException("Account is null!");

                ApplicationClass? DataLoggerNotesApp = await _dataService!.FindApplicationByIDAsync(15);

                noteItem.Author = account;
                noteItem.Project = new(-1, account, "", DataLoggerNotesApp,
                    LOG.CATEGORY.NOTES, false);

                noteItem.Application = DataLoggerNotesApp;

                if (_createCheckListViewModel == null)
                    throw new InvalidOperationException("Checklist viewmodel is null!");

                noteItem.Start = DateTime.Parse(_createCheckListViewModel.CreationDate);
                noteItem.End = DateTime.Parse(_createCheckListViewModel.ModifiedDate);
                noteItem.Output = await _dataService.FindOutputByIDAsync(37);
                noteItem.Type = await _dataService.FindTypeByIDAsync(39);

                noteItem.Subject = _createCheckListViewModel.NoteSubject;

                noteItem.Checklist = new();

                foreach (var item in _createCheckListViewModel.ChecklistItems)
                {
                    CheckListItem checkListItem = new CheckListItem(-1, item.IsDone, item.Item);
                    noteItem.Checklist.Items.Add(checkListItem);
                }




                // Send to database
                await _dataService.CreateLOGAsync(noteItem);

                var list = _notesViewModel?.NoteItems ?? throw new InvalidOperationException("Notes viewmodel is null!");
                list.Add(new NoteLOGViewModel(_dataService, _notesViewModel, noteItem));

                _notesViewModel.NoteItems = list;

                await _navigationService.NavigateToNotesDashboard();
                _navigationService.SetChecklistNotesChecked(false);
            }
            catch (Exception ex)
            {
                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred in SaveChecklistCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
