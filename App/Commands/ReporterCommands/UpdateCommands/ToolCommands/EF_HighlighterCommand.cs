using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using static Data_Logger_1._3.ViewModels.Reporter.Updater.EF_PostItViewModel;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands.ToolCommands
{
    public class EF_HighlighterCommand : CommandBase
    {
        private readonly EF_PostItViewModel _postItViewModel;

        public EF_HighlighterCommand(EF_PostItViewModel efPostItViewModel)
        {
            try
            {
                _postItViewModel = efPostItViewModel ?? throw new ArgumentNullException(nameof(efPostItViewModel));
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred: {ex.Message}");
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                var editor = _postItViewModel.ActiveEditor;
                if (editor != null)
                {
                    switch (_postItViewModel.FieldToEdit)
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
            catch (Exception)
            {
                //
            }
        }

        private static Brush? TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return Brushes.Transparent;
            }
        }
    }
}
