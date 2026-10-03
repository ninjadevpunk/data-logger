using Core.DTOs.Cache;
using Core.Enums;
using Core.Utilities;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    internal class EditCommand : AsyncCommandBase
    {
        private readonly LogCacheDTO _logCacheDTO;
        private readonly NavigationService _navigationService;

        public EditCommand(LogContext logContext, NavigationService navigationService, LogCacheDTO logCacheDTO)
        {
            _logCacheDTO = logCacheDTO;
            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToLoggerEditor(_logCacheDTO);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} EditCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
