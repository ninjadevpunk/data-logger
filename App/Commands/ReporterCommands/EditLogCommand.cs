using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Diagnostics;
using Data_Logger_1._3.Models;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Allows the user to edit a log retrieved from the database.
    /// </summary>
    public class EditLogCommand : CommandBase
    {
        private readonly REPORTViewModel _viewModel;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly NavigationService _navigationService;
        private readonly LOG _log;

        public EditLogCommand()
        {

        }

        public EditLogCommand(REPORTViewModel reportViewModel, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService)
        {
            _reportDeskViewModel = reportDeskViewModel;
            _navigationService = navigationService;
        }

        public EditLogCommand(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService)
        {
            _log = log;
            _reportDeskViewModel = reportDeskViewModel;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            try
            {
                _navigationService.NavigateToUpdater(_log, _reportDeskViewModel);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred near EditCommand.Execute(): {e.Message}");
            }
        }
    }

}
