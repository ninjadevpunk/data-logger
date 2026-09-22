using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class CreateLogCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;

        public CreateLogCommand(NavigationService navigationService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near CreateLogCommand: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                await _navigationService.NavigateToLoggerCreator();
            }
            catch (Exception)
            {

                //
            }

        }
    }
}
