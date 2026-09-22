using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class GoogleLoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly AuthService _authService;

        public GoogleLoginCommand(LoginViewModel loginViewModel, AuthService authService)
        {
            try
            {
                _viewModel = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));
                _authService = authService ?? throw new ArgumentNullException(nameof(authService));
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
                //_authService.GoogleSignIn().Wait();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
