using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

/// <summary>
/// A command to send the user to the login page.
/// </summary>
public class NavigateToLoginCommand : AsyncCommandBase
{
    private readonly NavigationService _navigationService;
    private readonly SignUpViewModel _signUpViewModel;

    public NavigateToLoginCommand(NavigationService navigationService, SignUpViewModel signUpViewModel)
    {
        _navigationService = navigationService;
        _signUpViewModel = signUpViewModel;
    }

    protected override async Task ExecuteAsync(object parameter)
    {
        try
        {
            await _navigationService.NavigateToLogin();
            _signUpViewModel.CloseSignUp();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Exception occurred near NavigateToLoginCommand.Execute(): {e.Message}");
        }
    }
}
