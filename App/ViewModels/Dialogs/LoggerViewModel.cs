using Core.DTOs.Identity;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Commands.CacheCommands;
using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    /// <summary>
    /// Viewmodel base class for all Logger page UI state.
    /// </summary>
    internal abstract class LoggerViewModel : ViewModelBase
    {
        public abstract LogContext Context { get; }
        public abstract LOG.CATEGORY Category { get; }
        public abstract string LogType { get; }

        protected readonly IAuthService _authService;
        protected readonly UserDTO _userDTO;
        protected readonly IDataService _dataService;
        protected readonly NavigationService _navigationService;
        protected readonly LogCacheViewModel _logCacheViewModel;





        protected LoggerViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IAuthService authService,
            IDataService dataService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "LoggerViewModel");
            _logCacheViewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel), "LoggerViewModel");
            _authService = authService ?? throw new ArgumentNullException(nameof(authService), "LoggerViewModel");
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService), "LoggerViewModel");

            _userDTO = authService.GetUser();

            ProjectName = "";
            _projects = [];

            ApplicationName = "";
            _applications = [];

            StartHours = DateTime.Now.Hour;
            StartMinutes = DateTime.Now.Minute;
            StartSeconds = DateTime.Now.Second;
            StartMilliseconds = DateTime.Now.Millisecond;

            StartDate = DateTime.Now;

            EndHours = DateTime.Now.Hour;
            EndMinutes = DateTime.Now.Minute;
            EndSeconds = DateTime.Now.Second;
            EndMilliseconds = DateTime.Now.Millisecond;

            EndDate = DateTime.Now;

            Output = "";
            _outputs = [];
            Type = "";
            _types = [];


            PostIts = [];

            CurrentStartDateCommand = new CurrentStartDateCommand(this);
            CurrentEndDateCommand = new CurrentEndDateCommand(this);
            AddPostItCommand = new AddPostItCommand(_navigationService, this);
            ClearPostItListCommand = new ClearPostItListCommand(this);
        }


        protected readonly ObservableCollection<string> _projects = [];
        protected readonly ObservableCollection<string> _applications = [];
        protected readonly HashSet<string> _outputs = [];
        protected readonly HashSet<string> _types = [];

        public IEnumerable<string> Projects => _projects;

        public IEnumerable<string> Applications => _applications;

        public IEnumerable<string> Outputs => _outputs;

        public IEnumerable<string> Types => _types;






        public Visibility DisplayPicVisibility => SignUpImage == null ? Visibility.Collapsed : Visibility.Visible;

        public string Author => $"{_userDTO.Name ?? string.Empty} {_userDTO.Surname ?? string.Empty}";


        private BitmapImage? signupImage;
        public BitmapImage? SignUpImage => BitmapService.LoadImage(_userDTO.ProfilePic);

        private string projectName = string.Empty;
        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string applicationName = string.Empty;
        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
                OnPropertyChanged(nameof(ApplicationName));
            }
        }

        protected abstract bool AppFieldEnabled { get; }


        #region START



        private DateTime startDate;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        private int startHours;
        public int StartHours
        {
            get
            {
                return startHours;
            }
            set
            {
                startHours = value;
                UpdateDateControl();
                OnPropertyChanged(nameof(StartHours));
            }
        }

        private int startMinutes;
        public int StartMinutes
        {
            get
            {
                return startMinutes;
            }
            set
            {
                startMinutes = value;
                UpdateDateControl();
                OnPropertyChanged(nameof(StartMinutes));
            }
        }

        private int startSeconds;
        public int StartSeconds
        {
            get
            {
                return startSeconds;
            }
            set
            {
                startSeconds = value;
                UpdateDateControl();
                OnPropertyChanged(nameof(StartSeconds));
            }
        }

        private int startMilliseconds;
        public int StartMilliseconds
        {
            get
            {
                return startMilliseconds;
            }
            set
            {
                startMilliseconds = value;
                UpdateDateControl();
                OnPropertyChanged(nameof(StartMilliseconds));
            }
        }


        #endregion




        #region END




        private DateTime endDate;
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        private int endHours;
        public int EndHours
        {
            get
            {
                return endHours;
            }
            set
            {
                endHours = value;
                UpdateEndDateControl();
                OnPropertyChanged(nameof(EndHours));
            }
        }

        private int endMinutes;
        public int EndMinutes
        {
            get
            {
                return endMinutes;
            }
            set
            {
                endMinutes = value;
                UpdateEndDateControl();
                OnPropertyChanged(nameof(EndMinutes));
            }
        }

        private int endSeconds;
        public int EndSeconds
        {
            get
            {
                return endSeconds;
            }
            set
            {
                endSeconds = value;
                UpdateEndDateControl();
                OnPropertyChanged(nameof(EndSeconds));
            }
        }

        private int endMilliseconds;
        public int EndMilliseconds
        {
            get
            {
                return endMilliseconds;
            }
            set
            {
                endMilliseconds = value;
                UpdateEndDateControl();
                OnPropertyChanged(nameof(EndMilliseconds));
            }
        }



        #endregion




        private string output = string.Empty;
        public string Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

        private string type = string.Empty;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }


        private ObservableCollection<PostItViewModel> postIts = [];
        public ObservableCollection<PostItViewModel> PostIts
        {
            get
            {
                return postIts;
            }
            set
            {
                postIts = value;
                OnPropertyChanged(nameof(PostIts));
            }
        }

        private bool isDatabaseOnline;
        public bool IsDatabaseOnline
        {
            get
            {
                return isDatabaseOnline;
            }
            set
            {
                isDatabaseOnline = value;
                DatabaseStatusFill = IsDatabaseOnline ? Brushes.Green : Brushes.Red;
                DatabaseStatus = IsDatabaseOnline ? "Connected" : "Not Connected";
                OnPropertyChanged(nameof(IsDatabaseOnline));
            }
        }



        private string logCount = string.Empty;
        public string LogCount
        {
            get
            {
                return logCount;
            }
            set
            {
                logCount = value;
                OnPropertyChanged(nameof(LogCount));
            }
        }


        private Brush? databaseStatusFill;
        public Brush? DatabaseStatusFill
        {
            get
            {
                return databaseStatusFill;
            }
            set
            {
                databaseStatusFill = value;
                OnPropertyChanged(nameof(DatabaseStatusFill));
            }
        }

        private string databaseStatus = string.Empty;
        public string DatabaseStatus
        {
            get
            {
                return databaseStatus;
            }
            set
            {
                databaseStatus = value;
                OnPropertyChanged(nameof(DatabaseStatus));
            }
        }


        public ICommand CurrentStartDateCommand { get; set; } = null!;

        public ICommand CurrentEndDateCommand { get; set; } = null!;


        /// <summary>
        /// Saves a log as a .dls file
        /// </summary>
        public abstract ICommand SaveCommand { get; }


        /// <summary>
        /// Edit a log in log cache.
        /// </summary>
        public ICommand CacheEditCommand { get; set; } = null!;

        public ICommand UpdateLogCommand { get; set; } = null!;


        // Clears the PostIt list
        public ICommand ClearPostItListCommand { get; set; } = null!;
        // Clears all fields in the logger
        public ICommand ClearLoggerCommand { get; set; } = null!;
        // Adds a PostIt to the PostIt list
        public ICommand AddPostItCommand { get; set; } = null!;




        #region Member Functions



        public abstract Task UpdateLogCount();

        public void UpdateDateControl()
        {
            StartDate = DateTime.Parse(StartDate.Date.ToLongDateString() + " " + StartHours + ":" + StartMinutes + ":" + StartSeconds + "." + StartMilliseconds);
        }

        public void UpdateEndDateControl()
        {
            EndDate = DateTime.Parse(EndDate.Date.ToLongDateString() + " " + EndHours + ":" + EndMinutes + ":" + EndSeconds + "." + EndMilliseconds);
        }

        public void TimeNow(bool updateStart)
        {
            if (updateStart)
            {
                StartHours = DateTime.Now.Hour;
                StartMinutes = DateTime.Now.Minute;
                StartSeconds = DateTime.Now.Second;
                StartMilliseconds = DateTime.Now.Millisecond;
            }

            EndHours = DateTime.Now.Hour;
            EndMinutes = DateTime.Now.Minute;
            EndSeconds = DateTime.Now.Second;
            EndMilliseconds = DateTime.Now.Millisecond;

        }

        #endregion
    }
}
