using Core.DTOs.Cache;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    /// <summary>
    /// Sends user to PostIt editor.
    /// </summary>
    internal class EditPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly CacheStickyPostItViewModel _cacheStickyPostItViewModel;

        public EditPostItCommand(NavigationService navigationService, LoggerViewModel loggerViewModel,
            CacheStickyPostItViewModel cacheStickyPostItViewModel)
        {
            _navigationService = navigationService;
            _loggerCreateViewModel = loggerViewModel;
            _cacheStickyPostItViewModel = cacheStickyPostItViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.NavigateToPostItEditor(_loggerCreateViewModel, _cacheStickyPostItViewModel);
        }
    }
}
