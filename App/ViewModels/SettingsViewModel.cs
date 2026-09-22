using Data_Logger_1._3.Commands.SettingsCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Services.CommandLogic;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public enum PasswordResetStage
        {
            RequestReset,
            EnterVerificationCode,
            ChangePassword,
            PasswordChanged
        }

        private readonly SettingsService _settingsService;
        private Settings _settings;
        public ICommand OpenImageCommand { get; set; }
        public ICommand ResetImageCommand { get; set; }
        public ICommand PasswordResetCommand { get; set; }
        public ICommand SubmitVerificationCodeCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
        public ICommand ReturnToDashboardCommand { get; set; }

        public SettingsViewModel(NavigationService navigationService, AuthService authService, IDataService dataService, SettingsService settingsService,
            MainWindowViewModel mainWindowViewModel, PasswordResetService passwordResetService)
        {
            var id = dataService.GetUser().accountID;

            _settingsService = settingsService;
            _settings = _settingsService.Load(id);

            var author = _settings.User;
            author.Id = id;

            SignUpImage = BitmapService.LoadImage(author.ProfilePic) ?? null;
            DisplayPicPath = author.ProfilePic;
            DefaultPicVisibility = SignUpImage == null ? Visibility.Visible : Visibility.Collapsed;

            BitmapService.DeleteTempProfilePics();

            Name = author.Name;
            Surname = author.Surname;
            Email = author.Email;
            YesBox = author.IsCompanyEmployee;

            if (YesBox)
            {
                CompanyName = author.CompanyName;
                CompanyAddress = author.CompanyAddress;
            }

            Theme = _settings.AppTheme.ToString();

            OpenImageCommand = new SaveSettingsProfilePicCommand(this);
            ResetImageCommand = new ResetImageCommand(this, _settings);
            PasswordResetCommand = new PasswordResetCommand(dataService, this, passwordResetService);
            SubmitVerificationCodeCommand = new SubmitVerificationCodeCommand(this, passwordResetService, dataService);
            ChangePasswordCommand = new ChangePasswordCommand(this, authService, dataService);
            DeleteAccountCommand = new DeleteAccountCommand(authService, dataService, settingsService);
            SaveSettingsCommand = new SaveSettingsCommand(authService, dataService, settingsService, _settings, mainWindowViewModel, this);
            SaveIsEnabled = SettingsService.FieldsAcceptable(Email, YesBox, CompanyName);
            ReturnToDashboardCommand = new ReturnToDashboardCommand(mainWindowViewModel, dataService);
        }

        private BitmapImage? signUpImage;
        public BitmapImage? SignUpImage
        {
            get
            {
                return signUpImage;
            }
            set
            {
                signUpImage = value;
                DefaultPicVisibility = value == null ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(SignUpImage));
            }
        }

        private string displayPicPath;
        public string DisplayPicPath
        {
            get
            {
                return displayPicPath;
            }
            set
            {
                displayPicPath = value;
                _settings.User.ProfilePic = value;
                SignUpImage = BitmapService.LoadImage(displayPicPath) ?? null;
                OnPropertyChanged(nameof(DisplayPicPath));
            }
        }

        private Visibility defaultPicVisibility;
        public Visibility DefaultPicVisibility
        {
            get
            {
                return defaultPicVisibility;
            }
            set
            {
                defaultPicVisibility = value;
                OnPropertyChanged(nameof(DefaultPicVisibility));
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                _settings.User.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string surname;
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
                _settings.User.Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                _settings.User.Email = value;
                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, YesBox, CompanyName);
                OnPropertyChanged(nameof(Email));
            }
        }

        private bool yesBox;
        public bool YesBox
        {
            get
            {
                return yesBox;
            }
            set
            {
                yesBox = value;


                if (NoBox == YesBox)
                    NoBox = !YesBox;


                _settings.User.IsCompanyEmployee = value;
                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, YesBox, CompanyName);
                OnPropertyChanged(nameof(YesBox));
            }
        }

        private bool noBox;
        public bool NoBox
        {
            get
            {
                return noBox;
            }
            set
            {
                noBox = value;

                if (YesBox == NoBox)
                    YesBox = !NoBox;

                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, YesBox, CompanyName);
                OnPropertyChanged(nameof(NoBox));
            }
        }

        private string? companyName;
        public string? CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
                _settings.User.CompanyName = value;
                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, YesBox, CompanyName);
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private string? companyAddress;
        public string? CompanyAddress
        {
            get
            {
                return companyAddress;
            }
            set
            {
                companyAddress = value;
                _settings.User.CompanyAddress = value;
                OnPropertyChanged(nameof(CompanyAddress));
            }
        }

        private string newPassword;
        public string NewPassword
        {
            get
            {
                return newPassword;
            }
            set
            {
                newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        // VERIFICATION CODE
        private string? verificationCode;
        public string? VerificationCode
        {
            get
            {
                return verificationCode;
            }
            set
            {
                verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }

        private PasswordResetStage resetStage = PasswordResetStage.RequestReset;
        public PasswordResetStage ResetStage
        {
            get
            {
                return resetStage;
            }
            set
            {
                resetStage = value;
                OnPropertyChanged(nameof(ResetStage));
            }
        }

        private bool passwordChanged = false;
        public bool PasswordChanged
        {
            get
            {
                return passwordChanged;
            }
            set
            {
                ResetStage = PasswordResetStage.PasswordChanged;
                passwordChanged = value;
                OnPropertyChanged(nameof(PasswordChanged));
            }
        }

        // THEME

        private string theme;
        public string Theme
        {
            get
            {
                return theme;
            }
            set
            {
                if (Enum.TryParse<Settings.Theme>(value, true, out var result))
                {
                    _settings.AppTheme = result;
                }
                theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        private bool saveIsEnabled;
        public bool SaveIsEnabled
        {
            get
            {
                return saveIsEnabled;
            }
            set
            {
                saveIsEnabled = value;
                OnPropertyChanged(nameof(SaveIsEnabled));
            }
        }


    }
}
