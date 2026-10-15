using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt
{
    internal class UpdatePostItViewModel : PostItViewModel
    {
        // TODO: Implement PostIt replacer command
        public override ICommand OKCommand { get; }
        public override ICommand PostItCommand { get; }

        public UpdatePostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerUpdateViewModel,
            DataStickyPostItViewModel dataStickyPostItViewModel)
            : base(navigationService, dataService, loggerUpdateViewModel)
        {
            var postItCacheDTO = dataStickyPostItViewModel.RtfPostIt;
            Subject = postItCacheDTO.Subject.SubjectName;
            PlaceholderText = string.IsNullOrWhiteSpace(Subject) ? "Subject" : string.Empty;

            Error = postItCacheDTO.Error;
            Solution = postItCacheDTO.Solution;
            Suggestion = postItCacheDTO.Suggestion;
            Comment = postItCacheDTO.Comment;

            OKCommand = new OKPostItCommand();
            PostItCommand = new EditPermanentPostItCommand(_navigationService, _dataService, _loggerCreateViewModel, this, dataStickyPostItViewModel);
        }
    }
}
