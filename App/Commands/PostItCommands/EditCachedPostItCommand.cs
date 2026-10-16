using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    /// <summary>
    /// Edits a PostIt in a cached log.
    /// </summary>
    internal class EditCachedPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly EditPostItViewModel _editPostItViewModel;
        private readonly CacheStickyPostItViewModel _cacheStickyPostItViewModel;


        public EditCachedPostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel,
            EditPostItViewModel editPostItViewModel, CacheStickyPostItViewModel cacheStickyPostItViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _editPostItViewModel = editPostItViewModel;
            _cacheStickyPostItViewModel = cacheStickyPostItViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (!Extensions.ValidatePostIt(_editPostItViewModel.Subject, _editPostItViewModel.Error, _editPostItViewModel.Solution,
                    _editPostItViewModel.Suggestion, _editPostItViewModel.Comment))
                    return;

                var dto = Extensions.BuildPostItCacheDTO(_editPostItViewModel.Subject, _editPostItViewModel.Error, _editPostItViewModel.Solution,
                    _editPostItViewModel.Suggestion, _editPostItViewModel.Comment);

                var cachePostIt = new CacheStickyPostItViewModel(_navigationService, _dataService, _loggerCreateViewModel, dto);
                var index = _loggerCreateViewModel.PostIts.IndexOf(_cacheStickyPostItViewModel);
                if (index > -1)
                    _loggerCreateViewModel.PostIts[index] = cachePostIt;

                _navigationService.GoBack(true);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
