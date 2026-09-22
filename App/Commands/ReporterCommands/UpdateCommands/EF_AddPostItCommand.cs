using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_AddPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly ReporterUpdaterViewModel _reporterUpdaterViewModel;

        public EF_AddPostItCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel)
        {


            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
            }
            catch (Exception)
            {
                //
            }


        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.NavigateToPostItCreator(_reporterUpdaterViewModel);
        }
    }
}
