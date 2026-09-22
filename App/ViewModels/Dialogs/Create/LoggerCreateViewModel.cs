using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    public abstract class LoggerCreateViewModel : ViewModelBase
    {

        protected readonly IDataService _dataService;
        protected readonly NavigationService _navigationService;
        protected readonly LogCacheViewModel _logCacheViewModel;


        public abstract LOG.CATEGORY Category { get; }

        public abstract CacheContext Context { get; }

        public abstract string LogType { get; }

        protected LoggerCreateViewModel()
        {
            
        }

        protected LoggerCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService)
        {
            _navigationService = navigationService;
            _logCacheViewModel = logCacheViewModel;
            _dataService = dataService;

            Author = _dataService.GetAuthorName();

            DisplayPicVisibility = Visibility.Visible;

            AppFieldEnabled = true;

            ProjectName = "";
            _projects = new();

            ApplicationName = "";
            _applications = new();

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
            _outputs = new();
            Type = "";
            _types = new();


            PostIts = new ObservableCollection<PostItViewModel>();

            CurrentStartDateCommand = new CurrentStartDateCommand(this);
            CurrentEndDateCommand = new CurrentEndDateCommand(this);
            AddPostItCommand = new AddPostItCommand(_navigationService, this);
            ClearPostItListCommand = new ClearPostItListCommand(this);
        }


        protected readonly ObservableCollection<string> _projects;
        protected readonly ObservableCollection<string> _applications;
        protected readonly HashSet<string> _outputs;
        protected readonly HashSet<string> _types;

        public IEnumerable<string> Projects => _projects;

        public IEnumerable<string> Applications => _applications;

        public IEnumerable<string> Outputs => _outputs;

        public IEnumerable<string> Types => _types;






        public Visibility DisplayPicVisibility { get; set; }

        public string Author { get; set; }


        private BitmapImage? signupImage;
        public BitmapImage? SignUpImage
        {
            get
            {
                return signupImage;
            }
            set
            {
                signupImage = value;

                if (SignUpImage != null)
                    DisplayPicVisibility = Visibility.Collapsed;
                else
                    DisplayPicVisibility = Visibility.Visible;

                OnPropertyChanged(nameof(SignUpImage));
            }
        }

        private string projectName;
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

        private string applicationName;
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

        private bool appFieldEnabled;
        public bool AppFieldEnabled
        {
            get
            {
                return appFieldEnabled;
            }
            set
            {
                appFieldEnabled = value;
                OnPropertyChanged(nameof(AppFieldEnabled));
            }
        }


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




        private string output;
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

        private string type;
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


        private ObservableCollection<PostItViewModel> postIts;
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



        private string logCount;
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


        private Brush databaseStatusFill;
        public Brush DatabaseStatusFill
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

        private string databaseStatus;
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


        public ICommand CurrentStartDateCommand { get; set; }

        public ICommand CurrentEndDateCommand { get; set; }


        // For log saving (from the logger). Saves as .log file
        public ICommand SaveCommand { get; set; }

        // Save log to log cache for 20 mins before database store
        public ICommand? AnnotateCommand { get; set; }

        // Edit log through log cache
        public ICommand EditCommand { get; set; }


        // Clears the PostIt list
        public ICommand ClearPostItListCommand { get; set; }
        // Clears all fields in the logger
        public ICommand ClearLoggerCommand { get; set; }
        // Adds a PostIt to the PostIt list
        public ICommand AddPostItCommand { get; set; }

        // For gfx, film and flexi logs only
        public ICommand BrowseCommand { get; set; }



        #region Member Functions



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
