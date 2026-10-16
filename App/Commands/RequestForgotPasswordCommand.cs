using Core.Interfaces;
using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    internal class RequestForgotPasswordCommand : AsyncCommandBase
    {
        private readonly IAuthService _authService;
        private readonly NavigationService _navigationService;
        private readonly ForgotPasswordViewModel _forgotPasswordViewModel;

        public RequestForgotPasswordCommand(IAuthService authService, NavigationService navigationService, ForgotPasswordViewModel forgotPasswordViewModel)
        {
            _authService = authService;
            _navigationService = navigationService;
            _forgotPasswordViewModel = forgotPasswordViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                await _authService.ForgotPasswordRequestAsync(_forgotPasswordViewModel.Email);
                
                await _navigationService.NavigateToLogin();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} RequestForgotPasswordCommand.ExecuteAsync: {ex.Message}");
            }

        }


    }
}
