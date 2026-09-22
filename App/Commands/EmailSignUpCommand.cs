using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class EmailSignUpCommand : AsyncCommandBase
    {


        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public EmailSignUpCommand(SignUpViewModel signUpViewModel, AuthService authService, NavigationService navigationService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (_authService == null)
                    throw new InvalidOperationException("AuthService cannot be null");

                // Retrieve user input from the view model
                var displayPic = _signUpViewModel.ProfilePicPath;
                string email = _signUpViewModel.Email;
                string password = _signUpViewModel.Password;
                string displayName = _signUpViewModel.Name;
                string surname = _signUpViewModel.Surname;
                var IsHired = _signUpViewModel.YesBox;
                var company = _signUpViewModel.CompanyName;
                var address = _signUpViewModel.CompanyAddress;
                var logo = _signUpViewModel.CompanyLogo;

                

                // Call the SignUp method in AuthService to handle user registration
                var isSignedUp = await _authService.SignUp(displayPic, email, password, displayName, surname, IsHired, company, address, logo);

                if (isSignedUp)
                {
                    _signUpViewModel.StatusMessage = "Sign up Successful";
                    _signUpViewModel.StatusMessageColour = _signUpViewModel.MessageGood;
                    await _navigationService.NavigateToMainWindow();
                    _signUpViewModel.CloseSignUp();
                }
                else
                {
                    _signUpViewModel.StatusMessage = "An error occurred";
                    _signUpViewModel.StatusMessageColour = _signUpViewModel.MessageBad;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred on our side. We apologise for any inconvenience. Feedback will be automatically sent to our support desk.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
