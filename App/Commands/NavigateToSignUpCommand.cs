
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

/// <summary>
/// Command for sending user to the sign up page.
/// </summary>
public class NavigateToSignUpCommand : CommandBase
{
    private readonly NavigationService _navigationService;

    public NavigateToSignUpCommand(NavigationService navigationService, LoginViewModel loginViewModel)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        try
        {
            _navigationService.NavigateToSignUp();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Exception occurred near NavigateToSignUpCommand.Execute(): {e.Message}");
        }
    }
}
