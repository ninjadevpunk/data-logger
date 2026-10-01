using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class EditCachedPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly PostItCacheDTO _postItCacheDTO;


        public EditCachedPostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, 
            PostItCacheDTO postItCacheDTO)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            _postItCacheDTO = postItCacheDTO ?? throw new ArgumentNullException(nameof(postItCacheDTO));
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToPostItEditor(_loggerCreateViewModel, _postItCacheDTO);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
