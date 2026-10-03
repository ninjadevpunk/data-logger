using Core.Utilities;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class ReportCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;

        public ReportCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
			try
			{
                await _navigationService.NavigateToReporter();
			}
			catch (Exception ex)
			{
                Debug.WriteLine($"{Constants.ExceptionPrefix} ReportCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
