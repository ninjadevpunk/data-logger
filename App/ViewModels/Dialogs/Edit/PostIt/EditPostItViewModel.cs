using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Views.Dialogs;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt
{
    /// <summary>
    /// Represnts UI logic for the <see cref="PostItPage"/>. For existing PostIts ONLY.
    /// </summary>
    internal sealed class EditPostItViewModel : PostItViewModel
    {
        public override ICommand OKCommand { get; }
        public override ICommand PostItCommand { get; }


        public EditPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel,
            CacheStickyPostItViewModel cacheStickyPostItViewModel) :
            base(navigationService, dataService, loggerCreateViewModel)
        {
            var postItCacheDTO = cacheStickyPostItViewModel.RtfPostIt;
            Subject = postItCacheDTO.Subject.SubjectName;
            PlaceholderText = string.IsNullOrWhiteSpace(Subject) ? "Subject" : string.Empty;

            Error = postItCacheDTO.Error;
            Solution = postItCacheDTO.Solution;
            Suggestion = postItCacheDTO.Suggestion;
            Comment = postItCacheDTO.Comment;

            OKCommand = new OKPostItCommand();
            PostItCommand = new EditCachedPostItCommand(_navigationService, _dataService, _loggerCreateViewModel, this, cacheStickyPostItViewModel);
        }


    }
}
