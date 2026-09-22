using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class RequestForgotPasswordCommand : AsyncCommandBase
    {
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public RequestForgotPasswordCommand(AuthService authService, NavigationService navigationService)
        {
            try
            {
                _authService = authService;
                _navigationService = navigationService;
            }
            catch (Exception)
            {
                //
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                _authService.ForgotPasswordRequest();

                await _navigationService.NavigateToLogin();
            }
            catch (Exception)
            {
                //
            }

        }


    }
}
