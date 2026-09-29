using Core.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    public class RequestForgotPasswordCommand : AsyncCommandBase
    {
        private readonly IAuthService? _authService;
        private readonly NavigationService? _navigationService;
        private readonly ForgotPasswordViewModel? _forgotPasswordViewModel;

        public RequestForgotPasswordCommand(IAuthService authService, NavigationService navigationService, ForgotPasswordViewModel forgotPasswordViewModel)
        {
            try
            {
                _authService = authService;
                _navigationService = navigationService;
                _forgotPasswordViewModel = forgotPasswordViewModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in RequestForgotPasswordCommand constructor: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                if (_authService == null || _navigationService == null || _forgotPasswordViewModel == null)
                    throw new InvalidOperationException("Auth, NavigationService and viewmodel can't be null!");

                await _authService.ForgotPasswordRequestAsync(_forgotPasswordViewModel.Email);
                
                await _navigationService.NavigateToLogin();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in RequestForgotPasswordCommand.ExecuteAsync: {ex.Message}");
            }

        }


    }
}
