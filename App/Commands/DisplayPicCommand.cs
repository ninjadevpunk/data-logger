using Core.Interfaces;
using Core.Utilities;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    internal class DisplayPicCommand : CommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly IAuthService _authService;
        private readonly ICacheService _cacheService;

        public DisplayPicCommand(SignUpViewModel signUpViewModel, IAuthService authService, ICacheService cacheService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
            _cacheService = cacheService;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.DefaultExt = ".png";
                dialog.Filter = "Portable Network Graphics (.png)|*.png|JPEG Images (.jpg)|*.jpg;";

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

                    _authService.GetUser().ProfilePic = tempProfilePicPath;

                }


            }
            catch (Exception)
            {
                _signUpViewModel.ShowDefault = Visibility.Visible;

                
                _signUpViewModel.SignUpImage = BitmapService.LoadImage(Constants.Avatar);
                _authService.GetUser().ProfilePic = string.Empty;
                _cacheService.DeleteTempProfilePics();
            }

        }
    }
}
