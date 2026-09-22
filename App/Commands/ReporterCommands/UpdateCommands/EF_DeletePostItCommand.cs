using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_DeletePostItCommand : CommandBase
    {
        private readonly ReporterUpdaterViewModel _viewModel;

        public EF_DeletePostItCommand(ReporterUpdaterViewModel viewModel)
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
            try
            {
                var item = parameter as EF_EditPostItViewModel;

                _viewModel.PostIts.Remove(item);
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred on our end. We apologise for any inconvenience. Feedback will be automatically sent to us but feel free to contact us if you need any assistance.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                // TODO
                // feedback sent here.
            }
        }
    }
}
