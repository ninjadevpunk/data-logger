using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    internal class DeleteNoteItemCommand : AsyncCommandBase
    {
        private readonly NOTESDashboardViewModel _notesViewModel;
        private readonly IDataService _dataService;
        private readonly int _noteId;

        public DeleteNoteItemCommand(NOTESDashboardViewModel notesViewModel, IDataService dataService, int noteId)
        {
            _notesViewModel = notesViewModel;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (parameter is NoteLOGViewModel noteToDelete)
                {
                    // Delete from database first
                    var noteDeleted = await _dataService.DeleteNoteAsync(_noteId);

                    // Remove the note from the ObservableCollection
                    if(noteDeleted)
                        _notesViewModel.NoteItems.Remove(noteToDelete);

                    _notesViewModel.StartUpVisibilitySet(_notesViewModel.NoteItems.Count > 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
