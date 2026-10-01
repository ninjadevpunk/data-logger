using Core.DTOs;
using Core.Utilities;
using Data_Logger_1._3.Utilities;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels.PostIt
{

    /// <summary>
    /// Represents a sticky PostIt that is viewed in the Viewer page.
    /// </summary>
    internal class ViewStickyPostItViewModel
    {
        public ViewStickyPostItViewModel(PostItDTO dto)
        {
            DisplaySubject = dto.Subject;
            DisplayError = RtfConverter.ToPlainText(dto.Error).Text;
            DisplaySolution = RtfConverter.ToPlainText(dto.Solution).Text;
            DisplaySuggestion = RtfConverter.ToPlainText(dto.Suggestion).Text;
            DisplayComment = RtfConverter.ToPlainText(dto.Comment).Text;
        }


        public string DisplaySubject { get; init; }
        public string DisplayError { get; set; }
        public string DisplaySolution { get; set; }
        public string DisplaySuggestion { get; set; }
        public string DisplayComment { get; set; }

        public Visibility ErrorVisibile => Formatters.FieldVisible(DisplayError);
        public Visibility SolutionVisible => Formatters.FieldVisible(DisplaySolution);
        public Visibility SuggestionVisible => Formatters.FieldVisible(DisplaySuggestion);
        public Visibility CommentVisibile => Formatters.FieldVisible(DisplayComment);

        
    }
}
