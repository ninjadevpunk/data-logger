using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {


        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;
        private readonly UIFactory _factory;

        public SignUpViewModel(AuthService authService, NavigationService navigationService, UIFactory uiFactory)
        {
            _authService = authService;
            _navigationService = navigationService;
            _factory = uiFactory;

            SignUpImage = null;
            Name = "";
            Surname = "";
            NoBox = true;
            CompanyName = "";
            CompanyAddress = "";
            CompanyLogo = "";

            StatusMessage = "";
            StatusMessageColour = UIFactory.TryParseBrush("RED");

            MessageGood = _factory.MessageGood;
            MessageBad = _factory.MessageBad;

            DisplayPicCommand = new DisplayPicCommand(this, _authService);
            EmailSignUpCommand = new EmailSignUpCommand(this, _authService, _navigationService);
            GoogleSignInCommand = new GoogleSignInCommand(this, _authService);
            NavigateToLoginCommand = new NavigateToLoginCommand(_navigationService, this);

            ShowDefault = Visibility.Visible;
        }


        public void CloseSignUp() => RequestClose?.Invoke();

        /* PROPERTIES */


        public string ProfilePicPath { get; set; } = "/Assets/login/user.png";

        private BitmapImage? signUpImage = null;
        public BitmapImage? SignUpImage
        {
            get
            {
                return signUpImage;
            }
            set
            {
                signUpImage = value;
                OnPropertyChanged(nameof(SignUpImage));
            }
        }

        private Visibility showDefault;
        public Visibility ShowDefault
        {
            get
            {
                return showDefault;
            }
            set
            {
                showDefault = value;
                OnPropertyChanged(nameof(ShowDefault));
            }
        }


        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string surname;
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private bool yesBox;
        public bool YesBox
        {
            get
            {
                return yesBox;
            }
            set
            {
                yesBox = value;

                if (YesBox)
                    FieldEnabled = true;

                if (NoBox == YesBox)
                    NoBox = !YesBox;

                OnPropertyChanged(nameof(YesBox));

            }
        }

        private bool noBox;
        public bool NoBox
        {
            get
            {
                return noBox;
            }
            set
            {
                noBox = value;

                if (NoBox)
                    FieldEnabled = false;

                if (YesBox == NoBox)
                    YesBox = !NoBox;

                OnPropertyChanged(nameof(NoBox));

            }
        }

        private string companyName;
        public string CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private string companyAddress;
        public string CompanyAddress
        {
            get
            {
                return companyAddress;
            }
            set
            {
                companyAddress = value;
                OnPropertyChanged(nameof(CompanyAddress));
            }
        }

        private string companyLogo;
        public string CompanyLogo
        {
            get
            {
                return companyLogo;
            }
            set
            {
                companyLogo = value;
                OnPropertyChanged(nameof(CompanyLogo));
            }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        private Brush statusMessageColour;
        public Brush StatusMessageColour
        {
            get
            {
                return statusMessageColour;
            }
            set
            {
                statusMessageColour = value;
                OnPropertyChanged(nameof(StatusMessageColour));
            }
        }


        public Brush MessageGood { get; set; }
        public Brush MessageBad { get; set; }









        private bool fieldEnabled;
        public bool FieldEnabled
        {
            get
            {
                return fieldEnabled;
            }
            set
            {
                fieldEnabled = value;
                OnPropertyChanged(nameof(FieldEnabled));
            }
        }

        public ICommand DisplayPicCommand { get; set; }

        public ICommand EmailSignUpCommand { get; set; }

        public ICommand GoogleSignInCommand { get; set; }

        public ICommand NavigateToLoginCommand { get; set; }

        public event Action? RequestClose;


    }
}
