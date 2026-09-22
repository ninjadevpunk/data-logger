using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class ChangePasswordCommand : AsyncCommandBase
    {
        SettingsViewModel _settingsViewModel;
        AuthService _authService;
        private readonly IDataService _dataService;


        public ChangePasswordCommand(SettingsViewModel settingsViewModel, AuthService authService, IDataService dataService)
        {
            _settingsViewModel = settingsViewModel;
            _authService = authService;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                // Implement the logic to change the password here.
                var passwordChanged = await _authService.ChangePassword(_settingsViewModel.NewPassword, _settingsViewModel.Email);

                if (passwordChanged)
                {
                    _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.PasswordChanged;
                    _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.RequestReset;
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in ChangePasswordCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
