using Core.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Media;
using static Data_Logger_1._3.ViewModels.Dialogs.PostItViewModel;

namespace Data_Logger_1._3.Commands.PostItCommands.ToolCommands
{
    internal class HighlighterCommand : CommandBase
    {
        private readonly PostItViewModel _createPostItViewModel;

        public HighlighterCommand(PostItViewModel createPostItViewModel)
        {
            _createPostItViewModel = createPostItViewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var editor = _createPostItViewModel.ActiveEditor;
                if (editor != null)
                {
                    switch (_createPostItViewModel.FieldToEdit)
                    {
                        case PostItField.Error:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Salmon);
                                break;
                            }
                        case PostItField.Solution:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGreen);
                                break;
                            }
                        case PostItField.Suggestion:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Pink);
                                break;
                            }
                        case PostItField.Comment:
                            {
                                editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.PaleGoldenrod);
                                break;
                            }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} HighlighterCommand.Execute: {ex.Message}");
            }
        }
    }
}
