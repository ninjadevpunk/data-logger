using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class DeleteAccountCommand : AsyncCommandBase
    {
        private readonly AuthService _authService;
        private readonly IDataService _dataService;
        private readonly SettingsService _settingsService;


        public DeleteAccountCommand(AuthService authService, IDataService dataService, SettingsService settingsService)
        {
            _authService = authService;
            _dataService = dataService;
            _settingsService = settingsService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you would like to delete your account? This action cannot be undone and you will be logged out automatically.", 
                    "Confirm Account Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (_authService.Account == null)
                        throw new InvalidOperationException("Account cant be null.");

                    if(File.Exists(_authService.Account.ProfilePic))
                        File.Delete(_authService.Account.ProfilePic);

                    _settingsService.Delete(_authService.Account.accountID);

                    var accountDeleted = await _authService.DeleteAccountAsync();

                    if(accountDeleted)
                    {
                        Application.Current.Shutdown();
                        return;
                    }

                    MessageBox.Show("An unexpected error occurred and we were unable to delete your account. Please try again later.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in DeleteAccountCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
