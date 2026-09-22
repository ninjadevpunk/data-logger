using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class OpenUpdaterCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;

        public OpenUpdaterCommand(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToUpdaterWindowAsync();
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "Exception occurred in OpenUpdaterCommand.ExecuteAsync()");
            }
        }



    }
}
