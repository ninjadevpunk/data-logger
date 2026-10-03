using Core.Interfaces;
using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Core.Models.Result;

namespace Data_Logger_1._3.Commands
{
    internal class LoginCommand : AsyncCommandBase
    {

        private readonly LoginViewModel _login;
        private readonly IAuthService _authService;
        private readonly NavigationService _navigationService;


        public LoginCommand(LoginViewModel loginViewModel, IAuthService authService, NavigationService navigationService)
        {
            _login = loginViewModel;
            _authService = authService;
            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {

            try
            {
                var result = await _authService.SignInAsync(_login.Username, _login.Password);

                if(result != null)
                {
                    switch(result.PublicMessageType)
                    {
                        case MessageType.Info:
                            {
                                // TODO: Show message box
                                break;
                            }
                        case MessageType.Warning:
                            {
                                // TODO: Show message box
                                break;
                            }
                        case MessageType.Error:
                            {
                                // TODO: Show message box
                                break;
                            }
                    }

                    if (result.Exception != null)
                        throw result.Exception;

                    bool signInSuccessful = result.Success;

                    if (signInSuccessful)
                    {
                        _login.StatusMessage = "Login Successful";
                        _login.StatusMessageColour = Extensions.MessageGood;
                        await _navigationService.NavigateToMainWindow();
                    }
                    else
                    {
                        _login.StatusMessage = "Your username/password is incorrect";
                        _login.StatusMessageColour = Extensions.MessageBad;
                    }

                    return;
                }

                _login.StatusMessage = "An unexpected error occurred.";
                _login.StatusMessageColour = Extensions.MessageBad;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} LoginCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
