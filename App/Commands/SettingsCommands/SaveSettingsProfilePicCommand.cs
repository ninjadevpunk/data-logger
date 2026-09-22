using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class SaveSettingsProfilePicCommand : AsyncCommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;

        public SaveSettingsProfilePicCommand(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            string oldProfilePicPath = string.Empty;

            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    DefaultExt = ".png",
                    Filter = "Portable Network Graphics (.png)|*.png|JPEG Images (.jpg)|*.jpg;"
                };

                // Show open file dialog box
                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    var fileInfo = new FileInfo(dialog.FileName);

                    // Limit to 2MB
                    if (fileInfo.Length > 2 * 1024 * 1024)
                    {
                        MessageBox.Show("Image is too large.");
                        return;
                    }

                    // Save resized image to AppData
                    string tempProfilePic = BitmapService.TempSaveResizedImage(dialog.FileName);

                    _settingsViewModel.DisplayPicPath = tempProfilePic;
                    _settingsViewModel.SignUpImage = BitmapService.LoadImage(tempProfilePic);

                }
            }
            catch (Exception)
            {

                if (string.IsNullOrEmpty(oldProfilePicPath))
                {
                    _settingsViewModel.DisplayPicPath = string.Empty;
                    _settingsViewModel.SignUpImage = null;
                    oldProfilePicPath = "/Assets/login/user.png";
                    _settingsViewModel.DefaultPicVisibility = Visibility.Visible;
                }
                else
                {
                    _settingsViewModel.DisplayPicPath = oldProfilePicPath;
                    _settingsViewModel.SignUpImage = BitmapService.LoadImage(oldProfilePicPath);
                }
            }
        }
    }
}
