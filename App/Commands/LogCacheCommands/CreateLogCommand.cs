using Data.Interfaces;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class CreateLogCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;


        public CreateLogCommand(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToLoggerCreator();
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred in CreateLogCommand.ExecuteAsync: {ex.Message}");
            }

        }
    }
}
