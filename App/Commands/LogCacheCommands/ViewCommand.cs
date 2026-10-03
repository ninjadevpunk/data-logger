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


        public ViewCommand(NavigationService navigationService, LogDTO logDTO)
        {
            _navigationService = navigationService;
            _logDTO = logDTO;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToViewer(_logDTO);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
