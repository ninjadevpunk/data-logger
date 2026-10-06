using Core.DTOs.Cache;
using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class EditCommand : AsyncCommandBase
    {
        private readonly LogCacheDTO _logCacheDTO;
        private readonly NavigationService _navigationService;
        private readonly Timer _timer;

        public EditCommand(NavigationService navigationService, LogCacheDTO logCacheDTO, Timer timer)
        {
            _logCacheDTO = logCacheDTO;
            _navigationService = navigationService;
            _timer = timer;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (!_timer.Change(Timeout.Infinite, Timeout.Infinite))
                    throw new InvalidOperationException("Timer not paused!");

                if(parameter is LOGViewModel logViewModel)
                    await _navigationService.NavigateToLoggerEditor(logViewModel, _logCacheDTO, _timer, (int)logViewModel.TimeRemaining);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} EditCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
