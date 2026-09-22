using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.ViewModels.Dialogs.PostItViewModel;

namespace Data_Logger_1._3.Commands.PostItCommands.ToolCommands
{
    public class EraserCommand : CommandBase
    {
        private readonly PostItViewModel _createPostItViewModel;

        public EraserCommand(PostItViewModel createPostItViewModel)
        {
            try
            {
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));
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
                switch (_createPostItViewModel.FieldToEdit)
                {
                    case PostItField.Error:
                        {
                            _createPostItViewModel.Error = string.Empty;
                            break;
                        }
                    case PostItField.Solution:
                        {
                            _createPostItViewModel.Solution = string.Empty;
                            break;
                        }
                    case PostItField.Suggestion:
                        {
                            _createPostItViewModel.Suggestion = string.Empty;
                            break;
                        }
                    case PostItField.Comment:
                        {
                            _createPostItViewModel.Comment = string.Empty;
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
