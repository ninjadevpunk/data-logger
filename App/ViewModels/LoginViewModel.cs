
using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Data_Logger_1._3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {



        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;
        private readonly UIFactory _factory;





        public LoginViewModel(AuthService authService, NavigationService navigationService, UIFactory uiFactory)
        {

            _authService = authService;
            _navigationService = navigationService;
            _factory = uiFactory;

            StatusMessage = "";
            StatusMessageColour = _factory.MessageBad;

            MessageGood = _factory.MessageGood;
            MessageBad = _factory.MessageBad;

            IsButtonBackEnabled = true;
            IsButtonForwardEnabled = false;

            LoginCommand = new LoginCommand(this, _authService, _navigationService);
            ForgotPasswordCommand = new ForgotPasswordCommand();
            GoogleLoginCommand = new GoogleLoginCommand(this, _authService);
            NavigateToSignUpCommand = new NavigateToSignUpCommand(_navigationService, this);
        }


        public void CloseLogin() => RequestClose?.Invoke();



        /* Member Variables */

        private string username;
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



        private static readonly Brush EnabledColor = UIFactory.TryParseBrush("iconColourAccent02");
        private static readonly Brush DisabledColor = UIFactory.TryParseBrush("AccentColour");
        public Brush MessageGood { get; set; }
        public Brush MessageBad { get; set; }

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



    }
}
