using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.EditCacheCommands
{
    /// <summary>
    /// Caching an edited cached log.
    /// </summary>
    internal class EditAnnotateCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly LogCacheDTO _logCacheDTO;


        public EditAnnotateCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel,
            LogCacheDTO logCacheDTO)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _logCacheDTO = logCacheDTO;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // TODO: Implement editing a cached log. CreateViewerViewModel a DTO from the logger UI and replace the old DTO with it by creating a
                // new LOGViewModel containing the new edited DTO.

                await _navigationService.NavigateToLogCachePage(_logCacheDTO.Context);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }

    }
}
