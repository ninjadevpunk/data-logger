using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt
{
    internal sealed class EditPostItViewModel : PostItViewModel
    {
        public override ICommand OKCommand => throw new NotImplementedException();
        public override ICommand PostItCommand { get; }


        public EditPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel,
            PostItCacheDTO postItCacheDTO) :
            base(navigationService, dataService, loggerCreateViewModel)
        {
            Subject = postItCacheDTO.Subject;
            PlaceholderText = Subject;

            Error = postItCacheDTO.Error;
            Solution = postItCacheDTO.Solution;
            Suggestion = postItCacheDTO.Suggestion;
            Comment = postItCacheDTO.Comment;

            PostItCommand = new EditCachedPostItCommand(_navigationService, _dataService, _loggerCreateViewModel, postItCacheDTO);
        }


    }
}
