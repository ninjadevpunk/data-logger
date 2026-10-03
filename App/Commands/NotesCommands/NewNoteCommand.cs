using Core.Utilities;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    internal class NewNoteCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;

        public NewNoteCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToCreateNotesPage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NewNoteCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
