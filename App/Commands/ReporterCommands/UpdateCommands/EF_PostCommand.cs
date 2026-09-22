using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_PostCommand : CommandBase
    {
        public enum ActionType { Create, Edit }

        private readonly NavigationService _navigationService;
        private readonly ReporterUpdaterViewModel _reporterUpdaterViewModel;
        private readonly EF_EditPostItViewModel _efEditPostItViewModel;
        private readonly EF_PostItViewModel _efPostItViewModel;
        private ActionType _actionType = ActionType.Create;


        public EF_PostCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
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


        public EF_PostCommand(ActionType actionType, NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel, EF_EditPostItViewModel efEditPostItViewModel, EF_PostItViewModel efPostItViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
                _efEditPostItViewModel = efEditPostItViewModel ?? throw new ArgumentNullException(nameof(efEditPostItViewModel));
                _efPostItViewModel = efPostItViewModel ?? throw new ArgumentNullException(nameof(efPostItViewModel));
                _actionType = actionType;
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

        public EF_PostCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel, EF_EditPostItViewModel efEditPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
                _efEditPostItViewModel = efEditPostItViewModel ?? throw new ArgumentNullException(nameof(efEditPostItViewModel));
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

        public override void Execute(object parameter)
        {
            try
            {
                switch (_actionType)
                {
                    case ActionType.Create:
                        {
                            if (_efEditPostItViewModel.Subject == string.Empty)
                            {
                                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                if ((_efEditPostItViewModel.Error.Length == 0 || _efEditPostItViewModel.Error.Length == 353) &&
                                (_efEditPostItViewModel.Solution.Length == 0 || _efEditPostItViewModel.Solution.Length == 353) &&
                                (_efEditPostItViewModel.Suggestion.Length == 0 || _efEditPostItViewModel.Suggestion.Length == 353) &&
                                (_efEditPostItViewModel.Comment.Length == 0 || _efEditPostItViewModel.Comment.Length == 353)
                                )
                                {
                                    MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    _reporterUpdaterViewModel.PostIts.Add(_efEditPostItViewModel);
                                    _navigationService.GoBack(true);
                                }
                            }
                            break;
                        }
                    case ActionType.Edit:
                        {
                            if (_efEditPostItViewModel.Subject == string.Empty)
                            {
                                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                if ((_efEditPostItViewModel.Error.Length == 0 || _efEditPostItViewModel.Error.Length == 353) &&
                                (_efEditPostItViewModel.Solution.Length == 0 || _efEditPostItViewModel.Solution.Length == 353) &&
                                (_efEditPostItViewModel.Suggestion.Length == 0 || _efEditPostItViewModel.Suggestion.Length == 353) &&
                                (_efEditPostItViewModel.Comment.Length == 0 || _efEditPostItViewModel.Comment.Length == 353)
                                )
                                {
                                    MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    var index = _reporterUpdaterViewModel.PostIts.IndexOf(_efEditPostItViewModel);
                                    _reporterUpdaterViewModel.PostIts[index] = _efEditPostItViewModel;
                                    _navigationService.GoBack(true);
                                }
                            }
                            break;
                        }
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
