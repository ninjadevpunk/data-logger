using Core.Interfaces;
using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using FileSys.Interfaces;
using Microsoft.Win32;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    internal class DisplayPicCommand : CommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly ICacheService _cacheService;

        public DisplayPicCommand(SignUpViewModel signUpViewModel, ICacheService cacheService)
        {
            _signUpViewModel = signUpViewModel;
            _cacheService = cacheService;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var dialog = new OpenFileDialog
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

                    // Save resized image to Temp
                    string tempProfilePicPath = BitmapService.TempSaveResizedImage(dialog.FileName);

                    _signUpViewModel.ShowDefault = Visibility.Collapsed;
                    _signUpViewModel.ProfilePicPath = tempProfilePicPath;
                    _signUpViewModel.SignUpImage = BitmapService.LoadImage(tempProfilePicPath);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} in DisplayPicCommand Execute: {ex.Message}");
                _signUpViewModel.ShowDefault = Visibility.Visible;

                
                _signUpViewModel.SignUpImage = null;
                _cacheService.DeleteTempProfilePics();
            }

        }
    }
}
