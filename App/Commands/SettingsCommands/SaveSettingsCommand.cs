using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Exceptions;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    internal class SaveSettingsCommand : AsyncCommandBase
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly SettingsService _settingsService;
        private readonly Settings _settings;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly SettingsViewModel _settingsViewModel;

        public SaveSettingsCommand(IAuthService authService, IDataService dataService, ICacheService cacheService, SettingsService settingsService,
            Settings settings, MainWindowViewModel mainWindowViewModel, SettingsViewModel settingsViewModel)
        {
            _authService = authService;
            _dataService = dataService;
            _cacheService = cacheService;
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
            Settings? savedSettings = _settingsService.Load();
            bool IsNewProfilePic = false;

            try
            {
                // Keep Old Settings

                if (savedSettings == null)
                    throw new Exception("Failed to load saved settings for the user.");

                oldProfilePicPath = savedSettings.User.ProfilePic;
                oldUserSettings = savedSettings.User;
                UserSettings? currentUserSettings = _settings.User;
                currentProfilePic = currentUserSettings.ProfilePic;

                if (!string.Equals(oldUserSettings.Email, currentUserSettings.Email, StringComparison.OrdinalIgnoreCase))
                {
                    if (await _dataService.EmailExistsAsync(currentUserSettings.Email))
                        throw new EmailConflictException("An account with this email already exists.");
                }

                if (!currentUserSettings.IsCompanyEmployee)
                {
                    currentUserSettings.CompanyName = string.Empty;
                    currentUserSettings.CompanyAddress = string.Empty;
                    _settingsViewModel.CompanyName = string.Empty;
                    _settingsViewModel.CompanyAddress = string.Empty;
                }

                IsNewProfilePic = _cacheService.IsTemporaryProfilePic(currentProfilePic);

                if (IsNewProfilePic)
                {
                    // Copy Selected Pic to AppData Folder
                    newProfilePicPath = _cacheService.SaveProfilePicture(currentProfilePic);
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

                    var user = _authService.GetUser();

                    if (user != null)
                        user.ProfilePic = newProfilePicPath;

                    // Update MainWindow profile picture
                    if (!string.IsNullOrEmpty(newProfilePicPath))
                    {
                        _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(newProfilePicPath);
                        _settingsViewModel.SignUpImage = BitmapService.LoadImage(newProfilePicPath);
                    }
                    else
                    {
                        _mainWindowViewModel.SignUpImage = null;
                        _settingsViewModel.SignUpImage = null;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_settingsViewModel.DisplayPicPath))
                    {
                        _mainWindowViewModel.SignUpImage = null;

                        // Profile Pic Update
                        if (!await _dataService.UpdateProfilePicAsync(currentUserSettings.Id, string.Empty))
                            throw new Exception("Failed to update profile picture.");
                    }
                }
                _settingsViewModel.DisplayPicPath = currentProfilePic;

                // User Settings Update
                if (!await _dataService.UpdateUserAsync(currentUserSettings))
                    throw new Exception("Failed to update user information.");

                await _authService.UpdateUser();


                // Save new settings to JSON Permanently
                savedSettings = _settingsService.Save(_settings) ?? throw new Exception("Failed to save settings.");

                // Update the startup theme
                try
                {
                    Extensions.PersistTheme(_settings.AppTheme);
                }
                catch (Exception ex)
                {
                    await _dataService.HandleExceptionAsync(ex);
                    Debug.WriteLine($"{Constants.ExceptionPrefix} SaveSettingsCommand.PersistTheme(): {ex.Message}");
                }


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
                            await _dataService.HandleExceptionAsync(ex);
                        }
                    }
                }

                // Delete all temp profile pics
                _cacheService.DeleteTempProfilePics();




            }
            catch (EmailConflictException mailex)
            {
                await _dataService.HandleExceptionAsync(mailex);
                MessageBox.Show("This email has been taken.", "Email Already Exists", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);

                // Profile Pic
                if (!string.IsNullOrEmpty(currentProfilePic) && IsNewProfilePic)
                    _cacheService.DeleteTempProfilePics();

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
                            await _dataService.HandleExceptionAsync(delex);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(oldProfilePicPath) && File.Exists(oldProfilePicPath))
                {
                    await _dataService.UpdateProfilePicAsync(_settings.User.Id, oldProfilePicPath);

                    _settings.User.ProfilePic = oldProfilePicPath;

                    var user = _authService.GetUser();

                    if (user != null && user.ProfilePic != oldProfilePicPath)
                        user.ProfilePic = oldProfilePicPath;

                    _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);
                    _settingsViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);
                    _settingsViewModel.DisplayPicPath = oldProfilePicPath;
                }
                else
                {
                    oldProfilePicPath = string.Empty;

                    await _dataService.UpdateProfilePicAsync(_settings.User.Id, oldProfilePicPath);
                    _settings.User.ProfilePic = oldProfilePicPath;
                    _mainWindowViewModel.SignUpImage = null;
                    _settingsViewModel.SignUpImage = null;
                }


                // Theme
                if (savedSettings != null)
                {
                    _settingsViewModel.Theme = savedSettings.AppTheme.ToString();
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
