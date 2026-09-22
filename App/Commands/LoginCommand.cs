using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class LoginCommand : AsyncCommandBase
    {

        private readonly LoginViewModel _login;
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;


        public LoginCommand(LoginViewModel loginViewModel, AuthService authService, NavigationService navigationService)
        {
            _login = loginViewModel;
            _authService = authService;
            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                bool signInSuccessful = await _authService.SignIn(_login.Username, _login.Password);

                if (signInSuccessful)
                {
                    _login.StatusMessage = "Login Successful";
                    _login.StatusMessageColour = _login.MessageGood;
                    await _navigationService.NavigateToMainWindow();
                }
                else
                {
                    _login.StatusMessage = "Your username/password is incorrect";
                    _login.StatusMessageColour = _login.MessageBad;
                }

            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
