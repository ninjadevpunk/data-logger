using Core.DTOs.Identity;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Data.Interfaces;
using Data_Logger_1._3.Commands.CacheCommands;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
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




        protected LoggerViewModel(NavigationService navigationService, IAuthService authService, IDataService dataService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "LoggerViewModel");
            _authService = authService ?? throw new ArgumentNullException(nameof(authService), "LoggerViewModel");
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService), "LoggerViewModel");

            _userDTO = authService.GetUser();

            ProjectName = string.Empty;
            ApplicationName = string.Empty;

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

            Output = string.Empty;
            Type = string.Empty;
            PostIts = [];

            CurrentStartDateCommand = new CurrentStartDateCommand(this);
            CurrentEndDateCommand = new CurrentEndDateCommand(this);
            
            ClearPostItListCommand = new ClearPostItListCommand(this);
        }



        public string ApplicationToolTip { get; set; } = "The application's name.";
        public string TimeStartToolTip { get; set; } = "Updates the current start time AND end time. Does not update automatically!";
        public string TimeEndToolTip { get; set; } = "Updates the END TIME ONLY. Does not update automatically.";
        public string OutputToolTip { get; set; } = "Please select from this list. Options cannot be modified.";
        public string TypeToolTip { get; set; } = "Please select from this list. Options cannot be modified.";


        protected HashSet<string> Projects { get; set; } = [];

        protected HashSet<string> Applications { get; set; } = [];

        protected HashSet<string> Outputs { get; set; } = [];

        protected HashSet<string> Types { get; set; } = [];






        public Visibility DisplayPicVisibility => SignUpImage != null ? Visibility.Collapsed : Visibility.Visible;

        public string Author => $"{_userDTO.Name ?? string.Empty} {_userDTO.Surname ?? string.Empty}";


        public BitmapImage? SignUpImage => BitmapService.LoadImage(_userDTO.ProfilePic);

        private string projectName = string.Empty;
        public string ProjectName
        {
            get => projectName;
            set
            {
                projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string applicationName = string.Empty;
        public string ApplicationName
        {
            get => applicationName;
            set
            {
                applicationName = value;
                OnPropertyChanged(nameof(ApplicationName));
            }
        }

        public virtual bool AppFieldEnabled { get; } = true;


        #region START



        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
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
            get => endDate;
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
            get => output;
            set
            {
                output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

        private string type = string.Empty;
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }


        private ObservableCollection<StickyPostItViewModel> postIts = [];
        public ObservableCollection<StickyPostItViewModel> PostIts
        {
            get => postIts;
            set
            {
                postIts = value;
                OnPropertyChanged(nameof(PostIts));
            }
        }

        private bool isDatabaseOnline;
        public bool IsDatabaseOnline
        {
            get => isDatabaseOnline;
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
            get => logCount;
            set
            {
                logCount = value;
                OnPropertyChanged(nameof(LogCount));
            }
        }


        private Brush? databaseStatusFill;
        public Brush? DatabaseStatusFill
        {
            get => databaseStatusFill;
            set
            {
                databaseStatusFill = value;
                OnPropertyChanged(nameof(DatabaseStatusFill));
            }
        }

        private string databaseStatus = string.Empty;
        public string DatabaseStatus
        {
            get => databaseStatus;
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
        /// Clears the PostIt list.
        /// </summary>
        public ICommand ClearPostItListCommand { get; set; } = null!;

        /// <summary>
        /// Clears all fields in the logger.
        /// </summary>
        public ICommand ClearLoggerCommand { get; set; } = null!;
        




        #region Member Functions


        /// <summary>
        /// Loads defaults asynchronously when the logger is opened. This includes loading the project list, application list, output list, and type list.
        /// </summary>
        /// <returns></returns>
        internal abstract Task AutoStartAsync();


        /// <summary>
        /// Loads the list of projects from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        protected abstract Task LoadProjects();


        /// <summary>
        /// Loads the list of applications from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        protected abstract Task LoadApplications();


        /// <summary>
        /// Loads the list of outputs from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        protected abstract Task LoadOutputs();


        /// <summary>
        /// Loads the list of types from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        protected abstract Task LoadTypes();


        /// <summary>
        /// Updates the log count asynchronously. This is used to display the number of logs in the cache/database.
        /// </summary>
        /// <returns></returns>
        public abstract Task UpdateLogCount();



        private void UpdateDateControl()
        {
            StartDate = DateTime.Parse(StartDate.Date.ToLongDateString() + " " + StartHours + ":" + StartMinutes + ":" + StartSeconds + "." + StartMilliseconds);
        }

        private void UpdateEndDateControl()
        {
            EndDate = DateTime.Parse(EndDate.Date.ToLongDateString() + " " + EndHours + ":" + EndMinutes + ":" + EndSeconds + "." + EndMilliseconds);
        }

        

        #endregion
    }
}
