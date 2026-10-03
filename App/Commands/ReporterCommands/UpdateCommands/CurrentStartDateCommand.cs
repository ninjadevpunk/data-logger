using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    internal class CurrentStartDateCommand : CommandBase
    {
        private readonly CodingLoggerUpdateViewModel _codingLoggerUpdateViewModel;

        public CurrentStartDateCommand(CodingLoggerUpdateViewModel codingLoggerUpdateViewModel)
        {
            _codingLoggerUpdateViewModel = codingLoggerUpdateViewModel;
        }


        public override void Execute(object parameter)
        {
            _codingLoggerUpdateViewModel.StartDate = DateTime.Now;
            _codingLoggerUpdateViewModel.StartHours = DateTime.Now.Hour;
            _codingLoggerUpdateViewModel.StartMinutes = DateTime.Now.Minute;
            _codingLoggerUpdateViewModel.StartSeconds = DateTime.Now.Second;
            _codingLoggerUpdateViewModel.StartMilliseconds = DateTime.Now.Millisecond;

            _codingLoggerUpdateViewModel.EndDate = DateTime.Now;
            _codingLoggerUpdateViewModel.EndHours = DateTime.Now.Hour;
            _codingLoggerUpdateViewModel.EndMinutes = DateTime.Now.Minute;
            _codingLoggerUpdateViewModel.EndSeconds = DateTime.Now.Second;
            _codingLoggerUpdateViewModel.EndMilliseconds = DateTime.Now.Millisecond;
        }
    }
}
