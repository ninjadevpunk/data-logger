using Core.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    internal class DeletePostItCommand : CommandBase
    {
        private readonly LoggerViewModel _loggerViewModel;

        public DeletePostItCommand(LoggerViewModel loggerViewModel)
        {
            _loggerViewModel = loggerViewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                if (parameter is StickyPostItViewModel item)
                    _loggerViewModel.PostIts.Remove(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred on our end. We apologise for any inconvenience. Feedback will be automatically sent to us but feel " +
                    "free to contact us if you need any assistance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Debug.WriteLine($"{Constants.ExceptionPrefix} DeletePostItCommand.Execute: {ex.Message}");
            }
        }
    }
}
