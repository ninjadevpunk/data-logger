using Data_Logger_1._3.Services;
using Data_Logger_1._3.Services.CommandLogic;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class SubmitVerificationCodeCommand : AsyncCommandBase
    {
        SettingsViewModel _settingsViewModel;
        PasswordResetService _passwordResetService;
        IDataService _dataService;


        public SubmitVerificationCodeCommand(SettingsViewModel settingsViewModel, PasswordResetService passwordResetService, IDataService dataService)
        {
            _settingsViewModel = settingsViewModel;
            _passwordResetService = passwordResetService;
            _dataService = dataService;
        }


        protected override async Task ExecuteAsync(object? parameter)
        {
            // Implement the logic to submit the verification code entered here.

            try
            {
                bool success = await _passwordResetService.VerifyCodeAsync(_settingsViewModel.Email, _settingsViewModel.VerificationCode);

                if(success)
                {
                    _settingsViewModel.ResetStage = SettingsViewModel.PasswordResetStage.ChangePassword;
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in SubmitVerificationCodeCommand.ExecuteAsync(): {ex.Message}");
            }
        }

    }
}
