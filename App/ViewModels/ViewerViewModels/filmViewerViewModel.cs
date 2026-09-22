using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public class filmViewerViewModel : LoggerViewViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.FILM;

        public override CacheContext Context => CacheContext.Film;
        public filmViewerViewModel(NavigationService navigationService) : base(navigationService)
        {
            OKCommand = new ViewerOKCommand(navigationService, Context);
        }

        public string HxW { get; set; }

        public string Length { get; set; }

        public string IsCompleted { get; set; }

        public string Source { get; set; }

    }
}
