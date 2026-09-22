using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class DisplayPicCommand : CommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService? _authService;

        public DisplayPicCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public DisplayPicCommand(SignUpViewModel signUpViewModel, AuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
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

                    if (_authService?.Account != null)
                        _authService.Account.ProfilePic = tempProfilePicPath;

                }


            }
            catch (Exception)
            {
                _signUpViewModel.ShowDefault = Visibility.Visible;

                
                _signUpViewModel.SignUpImage = BitmapService.LoadImage("/Assets/login/user.png");

                if (_authService?.Account != null)
                    _authService.Account.ProfilePic = "";

                BitmapService.DeleteTempProfilePics();
            }

        }
    }
}
