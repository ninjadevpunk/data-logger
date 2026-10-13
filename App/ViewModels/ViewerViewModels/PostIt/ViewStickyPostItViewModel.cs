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
            DisplaySubject = dto.Subject.SubjectName;
            DisplayError = RtfConverter.ToPlainText(dto.Error).Text;
            DisplaySolution = RtfConverter.ToPlainText(dto.Solution).Text;
            DisplaySuggestion = RtfConverter.ToPlainText(dto.Suggestion).Text;
            DisplayComment = RtfConverter.ToPlainText(dto.Comment).Text;
        }


        public string DisplaySubject { get; }
        public string DisplayError { get; }
        public string DisplaySolution { get; }
        public string DisplaySuggestion { get; }
        public string DisplayComment { get; }

        public Visibility ErrorVisibile => Formatters.FieldVisible(DisplayError);
        public Visibility SolutionVisible => Formatters.FieldVisible(DisplaySolution);
        public Visibility SuggestionVisible => Formatters.FieldVisible(DisplaySuggestion);
        public Visibility CommentVisibile => Formatters.FieldVisible(DisplayComment);

        
    }
}
