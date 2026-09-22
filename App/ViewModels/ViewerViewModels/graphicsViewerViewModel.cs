using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public class graphicsViewerViewModel : LoggerViewViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.GRAPHICS;

        public override CacheContext Context => CacheContext.Graphics;

        public graphicsViewerViewModel(NavigationService navigationService) : base(navigationService)
        {
            OKCommand = new ViewerOKCommand(navigationService, Context);
        }

        public string HxW {  get; set; }

        public string Medium { get; set; }

        public string Format { get; set; }

        public string Brush { get; set; } 

        public string MeasuringUnit { get; set; }

        public string Size { get; set; }

        public string DPI { get; set; }

        public string ColourDepth { get; set; }

        public string IsCompleted { get; set; }

        public string Source { get; set; }
    }
}
