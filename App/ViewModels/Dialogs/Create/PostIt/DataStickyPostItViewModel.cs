using Core.DTOs.Data;
using Core.Utilities;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt
{
    internal sealed class DataStickyPostItViewModel : StickyPostItViewModel<PostItDataDTO>
    {
        public DataStickyPostItViewModel(PostItDataDTO dto)
            : base(dto)
        {
            //EditPostItCommand = new EditPostItCommand(dto);
            //DeletePostItCommand = new DeletePostItCommand(dto);

            DisplayError = RtfConverter.ToPlainText(dto.Error).Text;
            DisplaySolution = RtfConverter.ToPlainText(dto.Solution).Text;
            DisplaySuggestion = RtfConverter.ToPlainText(dto.Suggestion).Text;
            DisplayComment = RtfConverter.ToPlainText(dto.Comment).Text;
        }
    }

}
