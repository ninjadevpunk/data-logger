using Core.DTOs.Cache;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class PostItCommand : AsyncCommandBase
    {

        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly CreatePostItViewModel _createPostItViewModel;

        public PostItCommand(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, CreatePostItViewModel createPostItViewModel)
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
                if (_createPostItViewModel.Subject == string.Empty)
                {
                    MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    bool allAreEmpty = IsEmptyField(_createPostItViewModel.Error) && IsEmptyField(_createPostItViewModel.Solution) &&
                        IsEmptyField(_createPostItViewModel.Suggestion) && IsEmptyField(_createPostItViewModel.Comment);


                    if (allAreEmpty)
                    {
                        MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", 
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        (string errorPlaintext, Exception? errorException) = RtfConverter.ToPlainText(_createPostItViewModel.Error);
                        (string solutionPlaintext, Exception? solutionException) = RtfConverter.ToPlainText(_createPostItViewModel.Solution);

                        if (errorException != null)
                            throw errorException;
                        if (solutionException != null)
                            throw solutionException;

                        var cachePostIt = new CacheStickyPostItViewModel(_navigationService, _dataService, _loggerCreateViewModel, new PostItCacheDTO
                        {
                            Subject = _createPostItViewModel.Subject,
                            Error = _createPostItViewModel.Error,
                            Solution = _createPostItViewModel.Solution,
                            Suggestion = _createPostItViewModel.Suggestion,
                            Comment = _createPostItViewModel.Comment,
                            ERCaptureTime = string.IsNullOrWhiteSpace(errorPlaintext) ? default : DateTime.Now,
                            SOCaptureTime = string.IsNullOrWhiteSpace(solutionPlaintext) ? default : DateTime.Now
                        });
                        _loggerCreateViewModel.PostIts.Add(cachePostIt);
                        _navigationService.GoBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

        }

        private bool IsEmptyField(string postItText) => string.IsNullOrWhiteSpace(RtfConverter.ToPlainText(postItText).Text);

    }
}
