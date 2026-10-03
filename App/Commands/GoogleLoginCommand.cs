using Core.Interfaces;
using Core.Utilities;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    internal class GoogleLoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly IAuthService _authService;

        public GoogleLoginCommand(LoginViewModel loginViewModel, IAuthService authService)
        {
            _viewModel = loginViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // TODO
                //_authService.GoogleSignInAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} GoogleLoginCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
