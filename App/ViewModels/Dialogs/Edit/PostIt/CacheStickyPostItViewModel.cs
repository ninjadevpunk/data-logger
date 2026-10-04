using Core.DTOs.Cache;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt
{
    internal sealed class CacheStickyPostItViewModel : StickyPostItViewModel
    {
        /// <summary>
        /// Snapshot of a cached PostIt in a cached log.
        /// </summary>
        private readonly PostItCacheDTO _postItCacheDTO;

        public override ICommand EditPostItCommand { get; }
        public override ICommand DeletePostItCommand { get; }

        public CacheStickyPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, 
            PostItCacheDTO postItCacheDTO)
            : base(navigationService, dataService, loggerCreateViewModel)
        {
            _postItCacheDTO = postItCacheDTO;
            RtfPostIt = postItCacheDTO;

            Subject = _postItCacheDTO.Subject;
            DisplayError = RtfConverter.ToPlainText(_postItCacheDTO.Error).Text;
            DisplaySolution = RtfConverter.ToPlainText(_postItCacheDTO.Solution).Text;
            DisplaySuggestion = RtfConverter.ToPlainText(_postItCacheDTO.Suggestion).Text;
            DisplayComment = RtfConverter.ToPlainText(_postItCacheDTO.Comment).Text;

            EditPostItCommand = new EditPostItCommand(_navigationService, _loggerCreateViewModel, this);
            DeletePostItCommand = new DeletePostItCommand(_loggerCreateViewModel);
        }

        public PostItCacheDTO RtfPostIt { get; }
    }
}
