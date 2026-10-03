using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    internal class ChangePasswordCommand : AsyncCommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;


        public ChangePasswordCommand(SettingsViewModel settingsViewModel, IAuthService authService, IDataService dataService)
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
                Result passwordChanged = await _authService.ChangePasswordAsync(_settingsViewModel.NewPassword, _settingsViewModel.Email);

                if (passwordChanged.Success)
                {
                    _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.PasswordChanged;
                    _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.RequestReset;
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"{Constants.ExceptionPrefix} ChangePasswordCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
