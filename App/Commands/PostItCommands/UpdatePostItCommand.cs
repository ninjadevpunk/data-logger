using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class UpdatePostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly DataStickyPostItViewModel _dataStickyPostItViewModel;

        public UpdatePostItCommand(NavigationService navigationService, LoggerViewModel loggerCreateViewModel,
            DataStickyPostItViewModel dataStickyPostItViewModel)
        {
            _navigationService = navigationService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _dataStickyPostItViewModel = dataStickyPostItViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.NavigateToPostItUpdater(_loggerCreateViewModel, _dataStickyPostItViewModel);
        }
    }
}
