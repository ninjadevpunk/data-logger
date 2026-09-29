using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.CacheCommands
{
    internal class CurrentEndDateCommand : CommandBase
    {
        private readonly LoggerViewModel _loggerCreateViewModel;

        public CurrentEndDateCommand(LoggerViewModel viewModel)
        {

            _loggerCreateViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public override void Execute(object parameter)
        {
            _loggerCreateViewModel.EndDate = DateTime.Now;
            _loggerCreateViewModel.EndHours = DateTime.Now.Hour;
            _loggerCreateViewModel.EndMinutes = DateTime.Now.Minute;
            _loggerCreateViewModel.EndSeconds = DateTime.Now.Second;
            _loggerCreateViewModel.EndMilliseconds = DateTime.Now.Millisecond;
        }
    }
}
