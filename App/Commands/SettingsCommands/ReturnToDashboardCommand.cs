using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    internal class ReturnToDashboardCommand : AsyncCommandBase
    {

        private readonly IDataService _dataService;
        private readonly MainWindowViewModel _mainWindowViewModel;


        public ReturnToDashboardCommand(MainWindowViewModel mainWindowViewModel, IDataService dataService)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                _mainWindowViewModel.CodingQtChecked = true;
            }
            catch (Exception e)
            {
                await _dataService.HandleExceptionAsync(e, $"{Constants.ExceptionPrefix} ReturnToDashboardCommand.ExecuteAsync");
            }
        }



    }
}
