using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    internal class SettingsService
    {
        public static readonly string BasePath = Constants.SettingsBasePath;
        private readonly int _userId;

        public SettingsService(IAuthService authService)
        {
            _userId = authService.GetUser().Id;
        }

        /// <summary>
        /// Creates a settings directory.
        /// </summary>
        /// <param name="userId">The logged-in user's ID.</param>
        /// <returns>Returns whether the operation was successful or not.</returns>
        public bool CreateSettingsDir()
        {
            try
            {
                var userFolder = Path.Combine(BasePath, _userId.ToString());

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                return true;
            }
            catch (UnauthorizedAccessException unex)
            {
                Debug.WriteLine($"UnauthorizedAccessException error occurred in CreateSettingsFile(userId): {unex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} CreateSettingsFile(userId): {ex.Message}");
                return false;
            }
        }

        public string GetUserFilePath()
        {
            try
            {
                return Path.Combine(BasePath, _userId.ToString(), "settings.json");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} GetUserFilePath(userId): {ex.Message}");
                return string.Empty;
            }
        }

        public Settings? Load()
        {
            try
            {
                var path = GetUserFilePath();

                if (!File.Exists(path))
                    return null;

                var json = File.ReadAllText(path);

                return JsonConvert.DeserializeObject<Settings>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} Load(userId): {ex.Message}");
                return null;
            }
        }

        public Settings? Save(Settings settings)
        {
            try
            {
                var path = GetUserFilePath();
                
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(path, json);

                MessageBox.Show("Settings saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                return settings;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} Load(userId): {ex.Message}");
                MessageBox.Show($"An unexpected error occurred. We apologise for any inconvenience caused.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public Settings? Save(Settings settings, bool isNewAccount)
        {
            try
            {
                var path = GetUserFilePath();

                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(path, json);

                if(!isNewAccount)
                {
                    MessageBox.Show("Settings saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return settings;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} Load(userId): {ex.Message}");
                MessageBox.Show($"An unexpected error occurred. We apologise for any inconvenience caused.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public bool Delete()
        {
            try
            {
                var userPath = Path.Combine(BasePath, _userId.ToString());

                if (!Directory.Exists(userPath))
                    return true;

                Directory.Delete(userPath, true);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} Delete(userId): {ex.Message}");
                return false;
            }
        }

        public static bool FieldsAcceptable(string email, bool isCompanyEmployee, string? companyName)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            if (isCompanyEmployee)
            {
                if (string.IsNullOrEmpty(companyName))
                    return false;
            }

            return true;
        }
    }
}
