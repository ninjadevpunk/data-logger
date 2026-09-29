using Core.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    public class GoogleLoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel? _viewModel;
        private readonly IAuthService? _authService;

        public GoogleLoginCommand(LoginViewModel loginViewModel, IAuthService authService)
        {
            try
            {
                _viewModel = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));
                _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in GoogleLoginCommand constructor: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                //_authService.GoogleSignInAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in GoogleLoginCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
