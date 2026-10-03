using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    public class SettingsService
    {
#if DEBUG
        public static readonly string BasePath = Path.Combine(AppContext.BaseDirectory, "Data Logger");
#else
        public static readonly string BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger");
#endif
        private readonly int _userId;

        public SettingsService(IAuthService authService)
        {
            _userId = authService.GetUser().UserId;
        }

        /// <summary>
        /// Creates a settings file. Throws an <see cref="InvalidOperationException"/> if the settings path is invalid.
        /// </summary>
        /// <param name="userId">The logged-in user's ID.</param>
        /// <returns>Returns whether the operation was successful or not.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool CreateSettingsFile()
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

        public Settings? Load(int userId)
        {
            try
            {
                var path = GetUserFilePath();

                if (!File.Exists(path))
                    return Save(userId, new Settings());

                var json = File.ReadAllText(path);

                return JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} Load(userId): {ex.Message}");
                return null;
            }
        }

        public Settings? Save(int userId, Settings settings)
        {
            try
            {
                if (Load(userId) == null)
                {
                    throw new InvalidOperationException("No settings!");
                }

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

        public Settings? Save(int userId, Settings settings, bool isNewAccount)
        {
            try
            {
                if (Load(userId) == null)
                {
                    throw new InvalidOperationException("No settings!");
                }

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

        public bool Delete(int userId)
        {
            try
            {
                var userPath = Path.Combine(BasePath, userId.ToString());

                if (!Directory.Exists(userPath))
                    return true;

                Directory.Delete(userPath, recursive: true);

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
