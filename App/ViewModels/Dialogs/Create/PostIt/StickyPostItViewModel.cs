using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt
{
    /// <summary>
    /// UI representation of a sticky note on the logger page.
    /// </summary>
    /// <typeparam name="T">A PostItDataDTO type</typeparam>
    internal abstract class StickyPostItViewModel
    {
        protected readonly NavigationService _navigationService;
        protected readonly IDataService _dataService;
        protected readonly LoggerViewModel _loggerCreateViewModel;

        /// <summary>
        /// Edits a PostIt in cacheed log.
        /// </summary>
        public abstract ICommand EditPostItCommand { get; }

        /// <summary>
        /// Deletes a PostIt from cacheed log.
        /// </summary>
        public abstract ICommand DeletePostItCommand { get; }

        public StickyPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _loggerCreateViewModel = loggerCreateViewModel;
        }


        public string Subject { get; set; } = string.Empty;

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
