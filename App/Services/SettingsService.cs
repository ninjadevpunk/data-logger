using Data_Logger_1._3.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    public class SettingsService
    {
        public static readonly string BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger");


        public string GetUserFilePath(int userId)
        {
            try
            {
                return Path.Combine(BasePath, userId.ToString(), "settings.json");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in GetUserFilePath(userId): {ex.Message}");
                return string.Empty;
            }
        }

        public Settings? Load(int userId)
        {
            try
            {
                var path = GetUserFilePath(userId);

                if (!File.Exists(path))
                    return Save(userId, new Settings());

                var json = File.ReadAllText(path);

                return JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in Load(userId): {ex.Message}");
                return null;
            }
        }

        public Settings? Save(int userId, Settings settings)
        {
            try
            {
                if (!CacheMaster.CreateSettingsFile(userId))
                {
                    return null;
                }

                var path = GetUserFilePath(userId);
                
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(path, json);

                MessageBox.Show("Settings saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                return settings;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in Load(userId): {ex.Message}");
                MessageBox.Show($"An unexpected error occurred. We apologise for any inconvenience caused.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public Settings? Save(int userId, Settings settings, bool isNewAccount)
        {
            try
            {
                if (!CacheMaster.CreateSettingsFile(userId))
                {
                    return null;
                }

                var path = GetUserFilePath(userId);

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
                Debug.WriteLine($"Exception occurred in Load(userId): {ex.Message}");
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
                Debug.WriteLine($"Exception occurred in Delete(userId): {ex.Message}");
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
