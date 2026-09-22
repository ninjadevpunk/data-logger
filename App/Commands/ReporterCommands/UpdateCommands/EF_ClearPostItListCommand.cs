using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_ClearPostItListCommand : CommandBase
    {
        private readonly ReporterUpdaterViewModel _viewModel;

        public EF_ClearPostItListCommand(ReporterUpdaterViewModel viewModel)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred at EF_ClearPostItListCommand(viewModel): {ex.Message}");
            }
        }

        public EF_ClearPostItListCommand()
        {

        }

        public override void Execute(object parameter)
        {
            try
            {
                _viewModel.PostIts.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred at EF_ClearPostItListCommand.Execute(): {ex.Message}");
            }
        }
    }
}
