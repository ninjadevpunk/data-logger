using Data.Interfaces;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class CreateLogCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;

        public CreateLogCommand(NavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public CreateLogCommand(NavigationService navigationService, IDataService dataService) : this(navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(_dataService));
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                await _navigationService.NavigateToLoggerCreator();
            }
            catch (Exception ex)
            {
                if(_dataService  != null)
                    await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred in CreateLogCommand.ExecuteAsync: {ex.Message}");
            }

        }
    }
}
