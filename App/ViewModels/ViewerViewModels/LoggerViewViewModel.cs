using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Windows.Input;
using System.Windows;
using MVVMEssentials.ViewModels;
using static Data_Logger_1._3.Services.CacheMaster;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public abstract class LoggerViewViewModel : ViewModelBase
    {
        protected readonly NavigationService _navigationService;
        private readonly List<PostItViewModel> _postIts;

        public abstract LOG.CATEGORY Category { get; }

        public abstract CacheContext Context { get; }

        protected LoggerViewViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _postIts = new List<PostItViewModel>();
            DisplayPicVisibility = SignUpImage != null ? Visibility.Collapsed : Visibility.Visible;
        }

        public void AddPostIt(PostItViewModel postIt)
        {
            _postIts.Add(postIt);
        }


        public Visibility DisplayPicVisibility { get; set; }

        public string Author { get; set; }


        private BitmapImage? signUpImage;
        public BitmapImage? SignUpImage
        {
            get => signUpImage;
            set
            {
                signUpImage = value;
                OnPropertyChanged(nameof(SignUpImage));

                DisplayPicVisibility = signUpImage != null
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                OnPropertyChanged(nameof(DisplayPicVisibility));
            }
        }

        public string ProjectName { get; set; }

        public string ApplicationName { get; set; }

        public string Date { get; set; }



        private string output;
        public string Output { get; set; }

        public string Type { get; set; }

        public IEnumerable<PostItViewModel> PostIts => _postIts;

        public ICommand OKCommand { get; set; }
    }
}
