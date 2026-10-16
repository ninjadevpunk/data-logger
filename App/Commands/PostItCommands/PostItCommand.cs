using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class PostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly CreatePostItViewModel _createPostItViewModel;

        public PostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, 
            CreatePostItViewModel createPostItViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _createPostItViewModel = createPostItViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (!Extensions.ValidatePostIt(_createPostItViewModel.Subject, _createPostItViewModel.Error, _createPostItViewModel.Solution, 
                    _createPostItViewModel.Suggestion, _createPostItViewModel.Comment))
                    return;

                var dto = Extensions.BuildPostItCacheDTO(_createPostItViewModel.Subject, _createPostItViewModel.Error, _createPostItViewModel.Solution,
                    _createPostItViewModel.Suggestion, _createPostItViewModel.Comment);

                var cachePostIt = new CacheStickyPostItViewModel(_navigationService, _dataService, _loggerCreateViewModel, dto);
                _loggerCreateViewModel.PostIts.Add(cachePostIt);
                _navigationService.GoBack(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

        }

    }
}
