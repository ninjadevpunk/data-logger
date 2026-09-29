using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class ClearPostItListCommand : CommandBase
    {
        private readonly LoggerViewModel _viewModel;

        public ClearPostItListCommand(LoggerViewModel viewModel)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred at ClearPostItListCommand(viewModel): {ex.Message}");
            }
        }

        public ClearPostItListCommand()
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
                Debug.WriteLine($"An exception occurred at ClearPostItListCommand.Execute(): {ex.Message}");
            }
        }
    }
}
