using Core.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    public class GoogleSignInCommand : AsyncCommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly IAuthService? _authService;

        public GoogleSignInCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public GoogleSignInCommand(SignUpViewModel signUpViewModel, IAuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                //await _authService.GoogleSignInAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in GoogleSignInCommand.ExecuteAsync: {ex.Message}");
            }
        }


    }
}
