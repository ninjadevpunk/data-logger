using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public class codeViewerViewModel : LoggerViewViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override CacheContext Context => CacheContext.Coding;

        public codeViewerViewModel(NavigationService navigationService) : base(navigationService)
        {
            OKCommand = new ViewerOKCommand(navigationService, Context);
        }

        public codeViewerViewModel(NavigationService navigationService, string application) : base(navigationService)
        {
            if (application == "Qt")
                OKCommand = new ViewerOKCommand(navigationService, CacheContext.Qt);
            else
                OKCommand = new ViewerOKCommand(navigationService, Context);
        }

        public string BugsFound { get; set; } = "0 Bugs Found";

        public string ApplicationOpened { get; set; } = "Unsuccessful Launch";
    }
}
