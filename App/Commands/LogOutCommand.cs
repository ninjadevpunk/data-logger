
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

/// <summary>
/// Logs out a user and takes them to the login page.
/// </summary>
public class LogOutCommand : AsyncCommandBase
{
    private readonly NavigationService _navigationService;

    public LogOutCommand(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Cancel);

            if (result == MessageBoxResult.Yes)
            {
                await _navigationService.NavigateToLogin(true);
            }

            
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Exception occurred near LogOutCommand.Execute(): {e.Message}");
        }
    }
}
