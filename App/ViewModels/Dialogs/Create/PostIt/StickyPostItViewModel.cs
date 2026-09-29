using Data_Logger_1._3.ViewModels.Utilities;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt
{
    /// <summary>
    /// UI representation of a sticky note on the logger page.
    /// </summary>
    /// <typeparam name="T">A PostItDataDTO type</typeparam>
    internal abstract class StickyPostItViewModel<T>
    {
        /// <summary>
        /// Snapshot of a saved PostIt
        /// </summary>
        protected readonly T _postItDTO;
        protected ICommand EditPostItCommand { get; set; } = null!;
        protected ICommand DeletePostItCommand { get; set; } = null!;

        protected StickyPostItViewModel(T postItDTO)
        {
            _postItDTO = postItDTO ?? throw new ArgumentNullException(nameof(postItDTO), "StickyPostItViewModel");
        }

        public string DisplayError { get; protected init; } = string.Empty;
        public Visibility ErrorVisible => Formatters.FieldVisible(DisplayError);

        public string DisplaySolution { get; protected init; } = string.Empty;
        public Visibility SolutionVisible => Formatters.FieldVisible(DisplaySolution);

        public string DisplaySuggestion { get; protected init; } = string.Empty;
        public Visibility SuggestionVisible => Formatters.FieldVisible(DisplaySuggestion);

        public string DisplayComment { get; protected init; } = string.Empty;
        public Visibility CommentVisible => Formatters.FieldVisible(DisplayComment);


        public double ErrorTextBlockHeight => Formatters.BlockHeight(DisplayError);
        public double SolutionTextBlockHeight => Formatters.BlockHeight(DisplaySolution);
        public double SuggestionTextBlockHeight => Formatters.BlockHeight(DisplaySuggestion);
        public double CommentTextBlockHeight => Formatters.BlockHeight(DisplayComment);

    }


}
