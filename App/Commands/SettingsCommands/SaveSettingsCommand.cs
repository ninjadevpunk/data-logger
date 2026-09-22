using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;
using static Data_Logger_1._3.Services.EntityReader;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class SaveSettingsCommand : AsyncCommandBase
    {
        private readonly AuthService _authService;
        private readonly IDataService _dataService;
        private readonly SettingsService _settingsService;
        private readonly Settings _settings;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly SettingsViewModel _settingsViewModel;

        public SaveSettingsCommand(AuthService authService, IDataService dataService, SettingsService settingsService, Settings settings, MainWindowViewModel mainWindowViewModel,
            SettingsViewModel settingsViewModel)
        {
            _authService = authService;
            _dataService = dataService;
            _settingsService = settingsService;
            _settings = settings;
            _mainWindowViewModel = mainWindowViewModel;
            _settingsViewModel = settingsViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            string oldProfilePicPath = string.Empty;
            string newProfilePicPath = string.Empty;
            UserSettings? oldUserSettings = null;
            string currentProfilePic = string.Empty;
            UserSettings? currentUserSettings = null;
            Settings? savedSettings = _settingsService.Load(_settings.User.Id);
            bool IsNewProfilePic = false;

            try
            {
                // Keep Old Settings

                if (savedSettings == null)
                    throw new Exception("Failed to load saved settings for the user.");

                oldProfilePicPath = savedSettings.User.ProfilePic;
                oldUserSettings = savedSettings.User;
                currentUserSettings = _settings.User;
                currentProfilePic = currentUserSettings.ProfilePic;

                if (!string.Equals(oldUserSettings.Email, currentUserSettings.Email, StringComparison.OrdinalIgnoreCase))
                {
                    if (await _dataService.EmailExists(currentUserSettings.Email))
                        throw new EmailConflictException("An account with this email already exists.");
                }

                if(!currentUserSettings.IsCompanyEmployee)
                {
                    currentUserSettings.CompanyName = string.Empty;
                    currentUserSettings.CompanyAddress = string.Empty;
                    _settingsViewModel.CompanyName = string.Empty;
                    _settingsViewModel.CompanyAddress = string.Empty;
                }

                IsNewProfilePic = BitmapService.IsTemporaryProfilePic(currentProfilePic);

                if (IsNewProfilePic)
                {
                    // Copy Selected Pic to AppData Folder
                    newProfilePicPath = BitmapService.SaveProfilePicture(currentProfilePic);
                    currentProfilePic = newProfilePicPath;

                    // Update new profile picture if profile pic has changed
                    if (string.IsNullOrEmpty(newProfilePicPath))
                        throw new InvalidOperationException("Profile pic cannot be null/empty when saving settings.");


                    // Update database
                    // Profile Pic Update
                    if (!await _dataService.UpdateProfilePicAsync(currentUserSettings.Id, currentProfilePic))
                        throw new Exception("Failed to update profile picture.");


                    // Update new profile picture to settings object
                    _settings.User.ProfilePic = newProfilePicPath;


                    currentUserSettings.ProfilePic = newProfilePicPath;

                    if (_authService?.Account != null)
                        _authService.Account.ProfilePic = newProfilePicPath;

                    // Update MainWindow profile picture
                    _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(newProfilePicPath);
                    _settingsViewModel.SignUpImage = BitmapService.LoadImage(newProfilePicPath);
                }

                // User Settings Update
                if (!await _dataService.UpdateUserAsync(currentUserSettings))
                    throw new Exception("Failed to update user information.");



                // Save new settings to JSON Permanently
                _settingsService.Save(_settings.User.Id, _settings);


                if (IsNewProfilePic)
                {   
                    // Delete old profile pic in AppData if it exists
                    if (File.Exists(oldProfilePicPath))
                    {
                        try
                        {
                            File.Delete(oldProfilePicPath);
                        }
                        catch (Exception ex)
                        {
                            await _dataService.HandleExceptionAsync(ex, $"Exception occurred while deleting old profile picture: {ex.Message}");
                        }
                    }
                }

                // Delete all temp profile pics
                BitmapService.DeleteTempProfilePics();


            }
            catch (EmailConflictException mailex)
            {
                await _dataService.HandleExceptionAsync(mailex, $"Email conflict exception occurred in SaveSettingsCommand.ExecuteAsync(): {mailex.Message}");
                MessageBox.Show("This email has been taken.", "Email Already Exists", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in SaveSettingsCommand.ExecuteAsync(): {ex.Message}");

                // Profile Pic
                if (!string.IsNullOrEmpty(currentProfilePic) && IsNewProfilePic)
                    BitmapService.DeleteTempProfilePics();

                if (!string.IsNullOrEmpty(newProfilePicPath))
                {
                    // Delete new profile pic in AppData if it exists
                    if (File.Exists(newProfilePicPath))
                    {
                        try
                        {
                            File.Delete(newProfilePicPath);
                        }
                        catch (Exception delex)
                        {
                            await _dataService.HandleExceptionAsync(delex, $"Exception occurred while deleting new profile picture: {ex.Message}");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(oldProfilePicPath) && File.Exists(oldProfilePicPath))
                {
                    await _dataService.UpdateProfilePicAsync(_settings.User.Id, oldProfilePicPath);

                    _settings.User.ProfilePic = oldProfilePicPath;

                    if (_authService?.Account != null && _authService.Account.ProfilePic != oldProfilePicPath)
                        _authService.Account.ProfilePic = oldProfilePicPath;

                    _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);
                    _settingsViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);

                }
                else
                {
                    oldProfilePicPath = "/Assets/login/user.png";

                    await _dataService.UpdateProfilePicAsync(_settings.User.Id, oldProfilePicPath);
                    _settings.User.ProfilePic = oldProfilePicPath;
                    _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);
                    _settingsViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);
                }


                // Theme
                if (savedSettings != null)
                {
                    _settings.AppTheme = savedSettings.AppTheme;
                }

                // User Settings
                if (oldUserSettings != null)
                {
                    await _dataService.UpdateUserAsync(oldUserSettings);

                    _settings.User = oldUserSettings;
                }


                MessageBox.Show("An unexpected error occurred. Changes have been reverted", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
