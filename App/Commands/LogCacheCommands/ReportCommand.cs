using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class ReportCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;

        public ReportCommand(NavigationService navigationService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            }
            catch (ArgumentNullException nullex)
            {
                Debug.WriteLine($"ArgumentNullException found near ReportCommand: {nullex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near ReportCommand: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
			try
			{
                await _navigationService.NavigateToReporter();
			}
			catch (Exception ex)
			{
                //
            }
        }
    }
}
