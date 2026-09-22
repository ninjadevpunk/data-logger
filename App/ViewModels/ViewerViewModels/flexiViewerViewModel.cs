using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public class flexiViewerViewModel : LoggerViewViewModel
    {

        public override LOG.CATEGORY Category => LOG.CATEGORY.NOTES;

        public override CacheContext Context => CacheContext.Flexi;

        public flexiViewerViewModel(NavigationService navigationService) : base(navigationService)
        {
            OKCommand = new ViewerOKCommand(navigationService, Context);
        }

        public string FlexiNoteCategory { get; set; }

        public string Medium { get; set; }

        public string Format { get; set; }

        public string Bitrate { get; set; }

        public string Duration { get; set; }

        public string IsCompleted { get; set; }

        public string Source { get; set; }
    }
}
