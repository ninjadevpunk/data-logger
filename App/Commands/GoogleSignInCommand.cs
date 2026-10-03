using Core.Interfaces;
using Core.Utilities;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands
{
    internal class GoogleSignInCommand : AsyncCommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly IAuthService _authService;


        public GoogleSignInCommand(SignUpViewModel signUpViewModel, IAuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // TODO
                //await _authService.GoogleSignInAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} GoogleSignInCommand.ExecuteAsync: {ex.Message}");
            }
        }


    }
}
