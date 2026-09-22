using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using System.Windows;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public class AScodeViewerViewModel : codeViewerViewModel
    {
        public override LOG.CATEGORY Category { get => LOG.CATEGORY.CODING; }
        public override CacheContext Context { get => CacheContext.AndroidStudio; }

        public AScodeViewerViewModel(NavigationService navigationService, bool isSimple) : base(navigationService)
        {
            OKCommand = new ViewerOKCommand(navigationService, Context);

            GradleDaemonVisibility = isSimple ? Visibility.Hidden : Visibility.Visible;
            RunBuildVisibility = isSimple ? Visibility.Hidden : Visibility.Visible;
            LoadBuildVisibility = isSimple ? Visibility.Hidden : Visibility.Visible;
            ConfigureBuildVisibility = isSimple ? Visibility.Hidden : Visibility.Visible;
            AllProjectsVisibility = isSimple ? Visibility.Hidden : Visibility.Visible;
        }

        public Visibility GradleDaemonVisibility { get; set; }
        public Visibility RunBuildVisibility { get; set; }
        public Visibility LoadBuildVisibility { get; set; }
        public Visibility ConfigureBuildVisibility { get; set; }
        public Visibility AllProjectsVisibility { get; set; }

        public string SyncTime { get; set; }

        public string GradleDaemonTime { get; set; }

        public string RunBuildTime { get; set; }
        public string LoadBuildTime { get; set; }
        public string ConfigureBuildTime { get; set; }
        public string AllProjectsTime { get; set; }
    }
}
