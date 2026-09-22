using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class DeleteNoteItemCommand : AsyncCommandBase
    {
        private readonly NOTESViewModel _notesViewModel;
        private readonly IDataService _dataService;

        public DeleteNoteItemCommand(NOTESViewModel notesViewModel, IDataService dataService)
        {
            try
            {
                _notesViewModel = notesViewModel ?? throw new ArgumentNullException(nameof(notesViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred near DeleteNoteItemCommand(notesViewModel,dataService) CONSTRUCTOR: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (parameter is NoteLOGViewModel noteToDelete)
                {
                    // Delete from database first
                    var noteDeleted = await _dataService.DeleteNote(noteToDelete.ViewModelID);

                    // Remove the note from the ObservableCollection
                    if(noteDeleted)
                        _notesViewModel.NoteItems.Remove(noteToDelete);

                    _notesViewModel.StartUpVisibilitySet(_notesViewModel.NoteItems.Count > 0);
                }
            }
            catch (Exception ex)
            {
                // Send to Feedback

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                await _dataService.HandleExceptionAsync(ex, "DeleteNoteItemCommand.Execute()");
            }
        }
    }
}
