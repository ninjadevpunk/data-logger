using Core.Models;
using Data_Logger_1._3.Services;
using System.Windows.Input;
using System.Windows;
using MVVMEssentials.ViewModels;
using System.Windows.Media.Imaging;
using Core.Enums;
using Data_Logger_1._3.ViewModels.ViewerViewModels.PostIt;
using Core.DTOs.Identity;
using Core.DTOs.Viewer;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    internal abstract class ViewerViewModel : ViewModelBase
    {
        protected readonly NavigationService _navigationService;
        private readonly IReadOnlyList<ViewStickyPostItViewModel> _postIts = [];

        public abstract LOG.CATEGORY Category { get; }
        public abstract LogContext Context { get; }

        protected ViewerViewModel(NavigationService navigationService, UserDTO userDTO, ViewDTO viewDTO)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "ViewerViewModel");
            _postIts = [.. viewDTO.PostIts.Select(postIt => new ViewStickyPostItViewModel(postIt))];
            PostIts = _postIts;

            Author = $"{userDTO.Name} {userDTO.Surname}";
            SignUpImage = BitmapService.LoadImage(userDTO.ProfilePic);
            var appName = viewDTO.ApplicationName;
            ApplicationName = string.IsNullOrEmpty(appName) ? "Unknown Application" : appName;
            var projectName = viewDTO.ProjectName;
            ProjectName = string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(projectName) ? "Unnamed Project" : projectName;
            Date = viewDTO.Date;
            Output = viewDTO.OutputName;
            Type = viewDTO.TypeName;
        }



        public Visibility DisplayPicVisibility => SignUpImage != null ? Visibility.Collapsed : Visibility.Visible;

        public string Author { get; }


        public BitmapImage? SignUpImage { get; }
        public string ProjectName { get; }
        public string ApplicationName { get; }
        public string Date { get; }



        public string Output { get; }
        public string Type { get; }
        public IReadOnlyList<ViewStickyPostItViewModel> PostIts { get; }
        public abstract ICommand OKCommand { get; }
    }
}
