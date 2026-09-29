using Core.DTOs.Cache;
using Core.Utilities;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt
{
    internal sealed class CacheStickyPostItViewModel : StickyPostItViewModel<PostItCacheDTO>
    {
        public CacheStickyPostItViewModel(PostItCacheDTO dto)
            : base(dto)
        {
            //EditPostItCommand = new EditCachedPostItCommand(dto);
            //DeletePostItCommand = new DeleteCachedPostItCommand(dto);

            DisplayError = RtfConverter.ToPlainText(dto.Error).Text;
            DisplaySolution = RtfConverter.ToPlainText(dto.Solution).Text;
            DisplaySuggestion = RtfConverter.ToPlainText(dto.Suggestion).Text;
            DisplayComment = RtfConverter.ToPlainText(dto.Comment).Text;
        }
    }
}
