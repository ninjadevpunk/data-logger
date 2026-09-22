using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class CurrentStartDateCommand : CommandBase
    {
        private readonly ReporterUpdaterViewModel _viewModel;

        public CurrentStartDateCommand(ReporterUpdaterViewModel viewModel)
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
            _viewModel.StartDate = DateTime.Now;
            _viewModel.StartHours = DateTime.Now.Hour;
            _viewModel.StartMinutes = DateTime.Now.Minute;
            _viewModel.StartSeconds = DateTime.Now.Second;
            _viewModel.StartMilliseconds = DateTime.Now.Millisecond;

            _viewModel.EndDate = DateTime.Now;
            _viewModel.EndHours = DateTime.Now.Hour;
            _viewModel.EndMinutes = DateTime.Now.Minute;
            _viewModel.EndSeconds = DateTime.Now.Second;
            _viewModel.EndMilliseconds = DateTime.Now.Millisecond;
        }
    }
}
