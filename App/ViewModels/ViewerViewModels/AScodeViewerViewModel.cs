using Data_Logger_1._3.Commands.LogCacheCommands;
using Core.Models;
using Data_Logger_1._3.Services;
using System.Windows;
using Core.Enums;
using System.Windows.Input;
using Core.DTOs.Identity;
using Core.DTOs.Viewer;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    internal class AScodeViewerViewModel : ViewerViewModel
    {
        public override LOG.CATEGORY Category { get => LOG.CATEGORY.CODING; }
        public override LogContext Context { get => LogContext.AndroidStudio; }

        public override ICommand OKCommand { get; }

        public AScodeViewerViewModel(NavigationService navigationService, UserDTO userDTO, ViewDTO viewDTO, bool isSimple) 
            : base(navigationService, userDTO, viewDTO)
        {
            OKCommand = new ViewerOKCommand(navigationService);

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

        public string SyncTime { get; set; } = string.Empty;
        public string GradleDaemonTime { get; set; } = string.Empty;
        public string RunBuildTime { get; set; } = string.Empty;
        public string LoadBuildTime { get; set; } = string.Empty;
        public string ConfigureBuildTime { get; set; } = string.Empty;
        public string AllProjectsTime { get; set; } = string.Empty;
    }
}
