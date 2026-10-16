using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    internal class DeleteAccountCommand : AsyncCommandBase
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly SettingsService _settingsService;


        public DeleteAccountCommand(IAuthService authService, IDataService dataService, SettingsService settingsService)
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
                    var user = _authService.GetUser();
                    if(File.Exists(user.ProfilePic))
                        File.Delete(user.ProfilePic);

                    _settingsService.Delete();

                    var accountDeleted = await _authService.DeleteAccountAsync();

                    if(accountDeleted.Success)
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
                await _dataService.HandleExceptionAsync(ex, $"{Constants.ExceptionPrefix} DeleteAccountCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
