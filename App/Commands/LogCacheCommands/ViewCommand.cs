using Core.DTOs;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{

    internal class ViewCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogDTO _logDTO;
        private readonly Timer _timer;


        public ViewCommand(NavigationService navigationService, LogDTO logDTO, Timer timer)
        {
            _navigationService = navigationService;
            _logDTO = logDTO;
            _timer = timer;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (!_timer.Change(Timeout.Infinite, Timeout.Infinite))
                    throw new InvalidOperationException("Timer not paused!");

                await _navigationService.NavigateToViewer(_logDTO, _timer);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
