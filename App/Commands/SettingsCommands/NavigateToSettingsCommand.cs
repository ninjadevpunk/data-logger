using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class NavigateToSettingsCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;

        public NavigateToSettingsCommand(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToSettings();
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in NavigateToSettingsCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
