using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.ViewModels.Reporter.Updater.EF_PostItViewModel;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands.ToolCommands
{
    public class EF_EraserCommand : CommandBase
    {
        private readonly EF_PostItViewModel _postItViewModel;

        public EF_EraserCommand(EF_PostItViewModel efPostItViewModel)
        {
            try
            {
                _postItViewModel = efPostItViewModel ?? throw new ArgumentNullException(nameof(efPostItViewModel));
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex.Message}");
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                switch (_postItViewModel.FieldToEdit)
                {
                    case PostItField.Error:
                        {
                            _postItViewModel.Error = string.Empty;
                            break;
                        }
                    case PostItField.Solution:
                        {
                            _postItViewModel.Solution = string.Empty;
                            break;
                        }
                    case PostItField.Suggestion:
                        {
                            _postItViewModel.Suggestion = string.Empty;
                            break;
                        }
                    case PostItField.Comment:
                        {
                            _postItViewModel.Comment = string.Empty;
                            break;
                        }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
