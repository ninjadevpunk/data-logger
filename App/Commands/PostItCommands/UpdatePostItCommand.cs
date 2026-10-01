using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class UpdatePostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;

        private readonly PostItDataDTO _postItDataDTO;

        public UpdatePostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel,
            PostItDataDTO postItDataDTO)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _postItDataDTO = postItDataDTO;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToPostItUpdater(_loggerCreateViewModel, _postItDataDTO);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
