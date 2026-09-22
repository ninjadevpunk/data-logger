using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class NewNoteCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;

        public NewNoteCommand(NavigationService navigationService)
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

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToCreateNotesPage();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
