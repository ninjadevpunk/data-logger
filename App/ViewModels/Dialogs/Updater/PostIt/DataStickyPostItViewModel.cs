using Core.DTOs.Data;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt
{
    internal sealed class DataStickyPostItViewModel : StickyPostItViewModel
    {
        /// <summary>
        /// Snapshot of a PostIt in a saved log.
        /// </summary>
        private readonly PostItDataDTO _postItDataDTO;

        public override ICommand EditPostItCommand { get; }
        public override ICommand DeletePostItCommand => throw new NotImplementedException();


        public DataStickyPostItViewModel(NavigationService navigationService, IDataService dataService, LoggerViewModel loggerCreateViewModel, 
            PostItDataDTO postItDataDTO)
            : base(navigationService, dataService, loggerCreateViewModel)
        {
            _postItDataDTO = postItDataDTO;

            DisplayError = RtfConverter.ToPlainText(postItDataDTO.Error).Text;
            DisplaySolution = RtfConverter.ToPlainText(postItDataDTO.Solution).Text;
            DisplaySuggestion = RtfConverter.ToPlainText(postItDataDTO.Suggestion).Text;
            DisplayComment = RtfConverter.ToPlainText(postItDataDTO.Comment).Text;

            EditPostItCommand = new UpdatePostItCommand(_navigationService, _dataService, _loggerCreateViewModel,
            _postItDataDTO);
        }

        public PostItDataDTO RtfPostIt => _postItDataDTO;
    }

}
