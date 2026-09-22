using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_EditPostItCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly ReporterUpdaterViewModel _viewModel;

        public EF_EditPostItCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterEditorViewModel)
        {


            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = reporterEditorViewModel ?? throw new ArgumentNullException(nameof(reporterEditorViewModel));
            }
            catch (Exception)
            {
                //
            }


        }



        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _navigationService.NavigateToPostItCreator(_viewModel, parameter as EF_EditPostItViewModel);
            }
            catch (ArgumentNullException nullex)
            {
                //
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
