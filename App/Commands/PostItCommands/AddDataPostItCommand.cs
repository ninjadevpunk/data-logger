using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class AddDataPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;

        public AddDataPostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToPostItCreator(_loggerCreateViewModel);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
