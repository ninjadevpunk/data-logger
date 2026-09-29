using Core.Models;
using Data_Logger_1._3.Services;
using System.Windows.Input;
using System.Windows;
using MVVMEssentials.ViewModels;
using System.Windows.Media.Imaging;
using Core.Enums;
using Data_Logger_1._3.ViewModels.ViewerViewModels.PostIt;
using Core.DTOs.Identity;
using Core.DTOs;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    internal abstract class ViewViewModel : ViewModelBase
    {
        protected readonly NavigationService _navigationService;
        private readonly IReadOnlyList<ViewStickyPostItViewModel> _postIts = [];

        public abstract LOG.CATEGORY Category { get; }
        public abstract LogContext Context { get; }

        protected ViewViewModel(NavigationService navigationService, UserDTO userDTO, ViewDTO viewDTO)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "ViewViewModel");
            _postIts = viewDTO.PostIts.Select(postIt => new ViewStickyPostItViewModel(postIt)).ToList();

            Author = $"{userDTO.Name} {userDTO.Surname}";
            SignUpImage = BitmapService.LoadImage(userDTO.ProfilePic);
            var appName = viewDTO.ApplicationName;
            ApplicationName = string.IsNullOrEmpty(appName) ? "Unknown Application" : appName;
            var projectName = viewDTO.ProjectName;
            ProjectName = string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(projectName) ? "Unnamed Project" : projectName;
            Date = viewDTO.Date;
            Output = viewDTO.OutputName;
            Type = viewDTO.TypeName;
            //OKCommand
        }



        public Visibility DisplayPicVisibility => SignUpImage != null ? Visibility.Collapsed : Visibility.Visible;

        public string Author { get; init; }


        public BitmapImage? SignUpImage { get; init; }
        public string ProjectName { get; init; }
        public string ApplicationName { get; init; }
        public string Date { get; init; }



        public string Output { get; init; }
        public string Type { get; init; }
        public IReadOnlyList<ViewStickyPostItViewModel> PostIts => _postIts;
        public ICommand OKCommand { get; init; } = null!;
    }
}
