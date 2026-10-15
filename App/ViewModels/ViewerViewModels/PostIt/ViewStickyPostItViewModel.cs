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
            ErrorVisible = Formatters.FieldVisible(DisplayError);
            SolutionVisible = Formatters.FieldVisible(DisplaySolution);
            SuggestionVisible = Formatters.FieldVisible(DisplaySuggestion);
            CommentVisible = Formatters.FieldVisible(DisplayComment);
            ErrorTextBlockHeight = Formatters.BlockHeight(DisplayError);
            SolutionTextBlockHeight = Formatters.BlockHeight(DisplaySolution);
            SuggestionTextBlockHeight = Formatters.BlockHeight(DisplaySuggestion);
            CommentTextBlockHeight = Formatters.BlockHeight(DisplayComment);
        }


        public string DisplaySubject { get; }
        public string DisplayError { get; }
        public string DisplaySolution { get; }
        public string DisplaySuggestion { get; }
        public string DisplayComment { get; }

        public Visibility ErrorVisible { get; }
        public Visibility SolutionVisible { get; }
        public Visibility SuggestionVisible { get; }
        public Visibility CommentVisible { get; }


        public double ErrorTextBlockHeight { get; }
        public double SolutionTextBlockHeight { get; }
        public double SuggestionTextBlockHeight { get; }
        public double CommentTextBlockHeight { get; }


    }
}
