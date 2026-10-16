using Core.DTOs;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    internal class OpenViewerCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogDTO _logDTO;


        public OpenViewerCommand(NavigationService navigationService, LogDTO logDTO)
        {
            _navigationService = navigationService;
            _logDTO = logDTO;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.NavigateToViewer(_logDTO);
        }
    }
}
