using Core.Models;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    internal class ResetImageCommand : AsyncCommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        private readonly Settings _settings;


        public ResetImageCommand(SettingsViewModel settingsViewModel, Settings settings)
        {
            _settingsViewModel = settingsViewModel;
            _settings = settings;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            _settingsViewModel.SignUpImage = null;
            _settingsViewModel.DisplayPicPath = string.Empty;
            _settings.User.ProfilePic = string.Empty;
        }
    }
}
