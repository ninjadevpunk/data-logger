using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class GoogleSignInCommand : AsyncCommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService _authService;

        public GoogleSignInCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public GoogleSignInCommand(SignUpViewModel signUpViewModel, AuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                //_authService.GoogleSignIn().Wait();
            }
            catch (Exception e)
            {
                //
            }
        }


    }
}
