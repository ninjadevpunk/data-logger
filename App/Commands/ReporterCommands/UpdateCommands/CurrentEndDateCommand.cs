using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class CurrentEndDateCommand : CommandBase
    {
        private readonly ReporterUpdaterViewModel _viewModel;

        public CurrentEndDateCommand(ReporterUpdaterViewModel viewModel)
        {


            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            }
            catch (Exception)
            {
                //
            }


        }



        public override void Execute(object parameter)
        {
            _viewModel.EndDate = DateTime.Now;
            _viewModel.EndHours = DateTime.Now.Hour;
            _viewModel.EndMinutes = DateTime.Now.Minute;
            _viewModel.EndSeconds = DateTime.Now.Second;
            _viewModel.EndMilliseconds = DateTime.Now.Millisecond;
        }
    }
}
