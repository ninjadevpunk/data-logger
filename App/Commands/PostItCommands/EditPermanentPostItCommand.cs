using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class EditPermanentPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerUpdateViewModel;
        private readonly UpdatePostItViewModel _updatePostItViewModel;
        private readonly DataStickyPostItViewModel _dataStickyPostItViewModel;

        public EditPermanentPostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerUpdateViewModel,
            UpdatePostItViewModel updatePostItViewModel, DataStickyPostItViewModel dataStickyPostItViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerUpdateViewModel = loggerUpdateViewModel;
            _updatePostItViewModel = updatePostItViewModel;
            _dataStickyPostItViewModel = dataStickyPostItViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (!Extensions.ValidatePostIt(_updatePostItViewModel.Subject, _updatePostItViewModel.Error, _updatePostItViewModel.Solution,
                    _updatePostItViewModel.Suggestion, _updatePostItViewModel.Comment))
                    return;

                var dto = Extensions.BuildPostItDataDTO(_updatePostItViewModel.Subject, _updatePostItViewModel.Error, _updatePostItViewModel.Solution,
                    _updatePostItViewModel.Suggestion, _updatePostItViewModel.Comment, _dataStickyPostItViewModel.RtfPostIt);

                var dataPostIt = new DataStickyPostItViewModel(_navigationService, _dataService, _loggerUpdateViewModel, dto);
                var index = _loggerUpdateViewModel.PostIts.IndexOf(_dataStickyPostItViewModel);
                if (index > -1)
                    _loggerUpdateViewModel.PostIts[index] = dataPostIt;

                _navigationService.GoBack(true);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
