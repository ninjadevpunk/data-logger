using Core.Interfaces;
using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels
{
    internal class ForgotPasswordViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly NavigationService _navigationService;

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand RequestForgotPasswordCommand { get; }

        public ForgotPasswordViewModel(IAuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            RequestForgotPasswordCommand = new RequestForgotPasswordCommand(_authService, _navigationService, this);
        }
    }
}
