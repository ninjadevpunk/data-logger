using Core.Interfaces;
using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Data_Logger_1._3.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {

        private readonly IAuthService _authService;
        private readonly NavigationService _navigationService;
        private readonly UIFactory _factory;
        public Brush MessageGood => TryParseBrush("GREEN");
        public Brush MessageBad => TryParseBrush("RED");
        private static readonly Brush EnabledColor = TryParseBrush("iconColourAccent02");
        private static readonly Brush DisabledColor = TryParseBrush("AccentColour");



        public LoginViewModel(IAuthService authService, NavigationService navigationService, UIFactory uiFactory)
        {
            _authService = authService;
            _navigationService = navigationService;
            _factory = uiFactory;

            StatusMessage = "";
            StatusMessageColour = MessageBad;


            IsButtonBackEnabled = true;
            IsButtonForwardEnabled = false;

            LoginCommand = new LoginCommand(this, _authService, _navigationService);
            ForgotPasswordCommand = new ForgotPasswordCommand();
            GoogleLoginCommand = new GoogleLoginCommand(this, _authService);
            NavigateToSignUpCommand = new NavigateToSignUpCommand(_navigationService, this);
        }


        public void CloseLogin() => RequestClose?.Invoke();



        /* Member Variables */

        private string username = string.Empty;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;

                OnPropertyChanged(nameof(Username));
            }
        }

        private string password = string.Empty;
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

        private string statusMessage = string.Empty;
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

        private Brush? statusMessageColour = null;
        public Brush? StatusMessageColour
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



        

        private bool _isButtonBackEnabled;
        public bool IsButtonBackEnabled
        {
            get => _isButtonBackEnabled;
            set
            {
                _isButtonBackEnabled = value;
                OnPropertyChanged(nameof(IsButtonBackEnabled));
            }
        }

        private bool _isButtonForwardEnabled;
        public bool IsButtonForwardEnabled
        {
            get => _isButtonForwardEnabled;
            set
            {
                _isButtonForwardEnabled = value;
                OnPropertyChanged(nameof(IsButtonForwardEnabled));
            }
        }

        public Brush? IconBackFill => IsButtonBackEnabled ? EnabledColor : DisabledColor;
        public Brush? IconForwardFill => IsButtonForwardEnabled ? EnabledColor : DisabledColor;

        public ICommand LoginCommand { get; set; }

        public ICommand GoogleLoginCommand { get; set; }

        public ICommand ForgotPasswordCommand {  get; set; }

        public ICommand NavigateToSignUpCommand {  get; set; }

        public event Action? RequestClose;





        public static Brush TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return Brushes.Transparent;
            }
        }

    }
}
