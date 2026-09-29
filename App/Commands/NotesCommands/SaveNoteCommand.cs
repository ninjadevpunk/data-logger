using System.Windows;
using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class SaveNoteCommand : AsyncCommandBase
    {
        protected readonly NavigationService _navigationService;
        protected readonly IDataService _dataService;
        protected readonly CreateNoteViewModel _createNoteViewModel;
        protected readonly NOTESViewModel _notesViewModel;


        public SaveNoteCommand(NavigationService navigationService, IDataService dataService, CreateNoteViewModel createNoteViewModel, NOTESViewModel notesViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _createNoteViewModel = createNoteViewModel ?? throw new ArgumentNullException(nameof(createNoteViewModel));
                _notesViewModel = notesViewModel ?? throw new ArgumentNullException(nameof(notesViewModel));

            }
            catch (Exception)
            {
                //
            }
        }

        public SaveNoteCommand(NavigationService navigationService, IDataService dataService, CreateNoteViewModel createNoteViewModel, NOTESViewModel notesViewModel, NoteLOGViewModel noteLOGViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _createNoteViewModel = createNoteViewModel ?? throw new ArgumentNullException(nameof(createNoteViewModel));
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
                if (string.IsNullOrEmpty(_createNoteViewModel.NoteContent))
                {
                    MessageBox.Show("Your note is empty. It will NOT be stored.", "Note Empty", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    NoteItem noteItem = new();

                    ACCOUNT account = _dataService.GetUser();
                    ApplicationClass? dataLoggerNotesApp = await _dataService.FindApplicationByIDAsync(15);

                    noteItem.Author = account;
                    noteItem.Project = await _dataService.FindProjectByIDAsync(1);

                    if (dataLoggerNotesApp != null)
                    {
                        noteItem.Application = dataLoggerNotesApp;
                        noteItem.Start = DateTime.Parse(_createNoteViewModel.CreationDate);
                        noteItem.End = DateTime.Parse(_createNoteViewModel.ModifiedDate);
                        noteItem.Output = await _dataService.FindOutputByIDAsync(37);
                        noteItem.Type = await _dataService.FindTypeByIDAsync(39);

                        noteItem.Subject = _createNoteViewModel.NoteSubject;
                        noteItem.NoteContent = _createNoteViewModel.NoteContent;




                        // Send to database
                        await _dataService.CreateLOGAsync(noteItem);

                        var list = _notesViewModel.NoteItems;
                        list.Add(new NoteLOGViewModel(_dataService, _notesViewModel, noteItem));

                        _notesViewModel.NoteItems = list;
                    }
                }

                await _navigationService.NavigateToNotesDashboard();
                _navigationService.SetGenericNotesChecked(false);
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
