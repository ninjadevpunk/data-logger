using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class DeletePostItCommand : CommandBase
    {
        private readonly LoggerCreateViewModel _viewModel;

        public DeletePostItCommand(LoggerCreateViewModel viewModel)
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
                var item = parameter as PostItViewModel;

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
