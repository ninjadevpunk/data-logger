using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

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

        public ICommand RequestForgotPasswordCommand { get; set; }

        public ForgotPasswordViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            RequestForgotPasswordCommand = new RequestForgotPasswordCommand(_authService, _navigationService);
        }
    }
}
