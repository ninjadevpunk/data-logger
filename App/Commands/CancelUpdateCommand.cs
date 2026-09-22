using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class CancelUpdateCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;

        public CancelUpdateCommand(IDataService dataService)
        {
            _dataService = dataService;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Close Updater Window
                if (parameter is Window window)
                    window.DialogResult = false;
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "Exception occurred in CancelCommand.ExecuteAsync");
            }
        }

    }
}
