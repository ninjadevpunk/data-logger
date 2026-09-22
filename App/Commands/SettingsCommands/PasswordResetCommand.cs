using Data_Logger_1._3.Services;
using Data_Logger_1._3.Services.CommandLogic;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class PasswordResetCommand : AsyncCommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        private readonly IDataService _dataService;
        private readonly PasswordResetService _passwordResetService;


        public PasswordResetCommand(IDataService dataService, SettingsViewModel settingsViewModel, PasswordResetService passwordResetService)
        {
            _settingsViewModel = settingsViewModel;
            _dataService = dataService;
            _passwordResetService = passwordResetService;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.RequestReset;

            try
            {
                bool success = await _passwordResetService.RequestPasswordResetAsync(_settingsViewModel.Email);

                if (!success)
                {
                    MessageBox.Show("We couldn't process your request right now. Please try again later.", "Password Reset", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.EnterVerificationCode;

                MessageBox.Show("Password reset request submitted. Please check your email for the verification code.", "Password Reset", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in PasswordResetCommand.ExecuteAsync(): {ex.Message}");
                MessageBox.Show("We couldn't process your request right now. Please try again later.", "Password Reset", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


    }
}
