using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class PostCommand : AsyncCommandBase
    {
        public enum ActionType { Create, Edit }
        public enum PostItContext { POSTIT, EDITPOSTIT }

        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;
        private readonly PostItViewModel _createPostItViewModel;
        private readonly EditPostItViewModel _editPostItViewModel;
        private readonly ActionType _actionType = ActionType.Create;
        private readonly PostItContext _postItContext = PostItContext.POSTIT;

        public PostCommand(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel, PostItViewModel createPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));

            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");

            }
        }

        public PostCommand(ActionType actionType, PostItContext postItContext, NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel, PostItViewModel createPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));
                _actionType = actionType;
                _postItContext = postItContext;
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }

        public PostCommand(ActionType actionType, PostItContext postItContext, NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel, EditPostItViewModel editPostItViewModel, PostItViewModel createPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));
                _editPostItViewModel = editPostItViewModel ?? throw new ArgumentNullException(nameof(editPostItViewModel));
                _actionType = actionType;
                _postItContext = postItContext;
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                switch (_postItContext)
                {
                    case PostItContext.POSTIT:
                        switch (_actionType)
                        {
                            case ActionType.Create:
                                {
                                    if (_createPostItViewModel.Subject == string.Empty)
                                    {
                                        MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                    else
                                    {
                                        if ((_createPostItViewModel.Error.Length == 0 || _createPostItViewModel.Error.Length == 353) &&
                                        (_createPostItViewModel.Solution.Length == 0 || _createPostItViewModel.Solution.Length == 353) &&
                                        (_createPostItViewModel.Suggestion.Length == 0 || _createPostItViewModel.Suggestion.Length == 353) &&
                                        (_createPostItViewModel.Comment.Length == 0 || _createPostItViewModel.Comment.Length == 353)
                                        )
                                        {
                                            MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                        else
                                        {
                                            _loggerCreateViewModel.PostIts.Add(_createPostItViewModel);
                                            _navigationService.GoBack(true);
                                        }
                                    }
                                    break;
                                }
                            case ActionType.Edit:
                                {
                                    if ((_createPostItViewModel.Error.Length == 0 || _createPostItViewModel.Error.Length == 353) &&
                                        (_createPostItViewModel.Solution.Length == 0 || _createPostItViewModel.Solution.Length == 353) &&
                                        (_createPostItViewModel.Suggestion.Length == 0 || _createPostItViewModel.Suggestion.Length == 353) &&
                                        (_createPostItViewModel.Comment.Length == 0 || _createPostItViewModel.Comment.Length == 353)
                                       )
                                    {
                                        MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                    else
                                    {
                                        var index = _loggerCreateViewModel.PostIts.IndexOf(_createPostItViewModel);

                                        if (index != -1)
                                            _loggerCreateViewModel.PostIts[index] = _createPostItViewModel;

                                        _navigationService.GoBack(true);
                                    }
                                    break;
                                }
                        }

                        break;
                    case PostItContext.EDITPOSTIT:
                        switch (_actionType)
                        {
                            case ActionType.Create:
                                break;
                            case ActionType.Edit:
                                {
                                    if (_editPostItViewModel.Subject == string.Empty)
                                    {
                                        MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                    else
                                    {
                                        if ((_editPostItViewModel.Error.Length == 0 || _editPostItViewModel.Error.Length == 353) &&
                                            (_editPostItViewModel.Solution.Length == 0 || _editPostItViewModel.Solution.Length == 353) &&
                                            (_editPostItViewModel.Suggestion.Length == 0 || _editPostItViewModel.Suggestion.Length == 353) &&
                                            (_editPostItViewModel.Comment.Length == 0 || _editPostItViewModel.Comment.Length == 353)
                                           )
                                        {
                                            MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                        else
                                        {
                                            var index = _loggerCreateViewModel.PostIts.IndexOf(_editPostItViewModel);

                                            if (index != -1)
                                                _loggerCreateViewModel.PostIts[index] = _editPostItViewModel;

                                            _navigationService.GoBack(true);
                                        }
                                    }
                                    break;
                                    break;
                                }
                        }

                        break;
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
