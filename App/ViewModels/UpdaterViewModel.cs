using Data_Logger_1._3.Commands;
using Core.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using Velopack;
using Data.Interfaces;
using Core.Interfaces;

namespace Data_Logger_1._3.ViewModels
{
    public class UpdaterViewModel : ViewModelBase
    {
        private readonly IAuthService? _authService;
        private readonly IDataService _dataService;
        private readonly SettingsService? _settingsService;
        private readonly Settings? _settings;
        private readonly IVelopackService? _velopackService;
        private readonly UpdateInfo _updateInfo;
        public ICommand? UpdateNowCommand { get; set; } = null;
        public ICommand? CancelUpdateCommand { get; set; } = null;
        public ICommand? ManualUpdateNowCommand { get; set; } = null;

        public UpdaterViewModel(IAuthService authService, IDataService dataService, SettingsService settingsService, IVelopackService velopackService,
            UpdateInfo updateInfo)
        {
            UpdateStatus = "Updating...";
            _authService = authService;
            _dataService = dataService;
            _updateInfo = updateInfo;

            _settingsService = settingsService;
            _settings = _settingsService.Load(UserID ?? throw new InvalidOperationException("UserID is null!")) ?? new Settings();

            UpdateNowCommand = new UpdateNowCommand(dataService, velopackService, updateInfo);
            CancelUpdateCommand = new CancelUpdateCommand(dataService);
        }

        public UpdaterViewModel(IDataService dataService, IVelopackService velopackService, UpdateInfo updateInfo)
        {
            UpdateStatus = "Downloading Update...";
            _dataService = dataService;

            _velopackService = velopackService;
            _updateInfo = updateInfo;


            ManualUpdateNowCommand = new UpdateNowCommand(dataService, velopackService, updateInfo, true);
            BackgroundDownloadUpdateAsync();
        }




        #region Properties



        private int? UserID => _authService?.GetUser().UserId;

        public string UpdateVersion => $"Latest: {_updateInfo.TargetFullRelease.Version}";

        private bool showUpdatePopup;
        public bool ShowUpdatePopup
        {
            get => showUpdatePopup;
            private set
            {
                if (showUpdatePopup == value)
                    return;

                showUpdatePopup = value;

                if (_settings != null)
                {
                    _settings.ShowUpdatePopup = value;
                    SaveSettings();
                }

                OnPropertyChanged(nameof(ShowUpdatePopup));
            }
        }

        private string updateStatus = string.Empty;
        public string UpdateStatus
        {
            get => updateStatus;
            set
            {
                updateStatus = value;
                OnPropertyChanged(nameof(UpdateStatus));
            }
        }

        private bool barVisible = true;
        public bool BarVisible
        {
            get => barVisible;
            set
            {
                barVisible = value;
                OnPropertyChanged(nameof(BarVisible));
                BarVisibility = value ? Visibility.Visible : Visibility.Hidden;
                InstallButtonVisible = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private bool isDownloadComplete = false;
        public bool IsDownloadComplete
        {
            get => isDownloadComplete;
            set
            {
                isDownloadComplete = value;
                OnPropertyChanged(nameof(IsDownloadComplete));
            }
        }

        private Visibility barVisibility;
        public Visibility BarVisibility
        {
            get => barVisibility;
            set
            {
                barVisibility = value;
                OnPropertyChanged(nameof(BarVisibility));
            }
        }

        private Visibility installButtonVisible;
        public Visibility InstallButtonVisible
        {
            get => installButtonVisible;
            set
            {
                installButtonVisible = value;
                OnPropertyChanged(nameof(InstallButtonVisible));
            }
        }





        #endregion







        #region Methods




        private void SaveSettings()
        {
            try
            {
                if (_settings != null)
                    _settingsService!.Save(UserID ?? throw new InvalidOperationException("UserID is null!"), _settings);
            }
            catch (Exception ex)
            {
                _dataService.HandleExceptionAsync(ex, "Exception occurred in SaveSettings()");
            }
        }

        private async void BackgroundDownloadUpdateAsync()
        {
            try
            {
                if (_velopackService != null)
                {
                    await _velopackService.DownloadUpdateAsync(_updateInfo);
                    IsDownloadComplete = true;
                    BarVisible = false;

                    if (IsDownloadComplete)
                        UpdateStatus = "Update Ready";
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "Exception occurred in BackgroundDownloadUpdateAsync()");
                MessageBox.Show("An unexpected error occurred while trying to download the update. We apologise for any inconvenience caused. Please try again later.",
                    "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        #endregion
    }
}
