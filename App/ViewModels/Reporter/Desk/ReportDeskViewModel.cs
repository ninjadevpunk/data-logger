using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    /// <summary>
    /// The base class for all reporter dashboards.
    /// </summary>
    public abstract class ReportDeskViewModel : ViewModelBase
    {
        public NavigationService _navigationService;
        protected readonly IDataService _dataService;
        protected readonly PDFService _pdfService;

        public abstract CacheContext Context { get; }

        protected bool AwaitCall = true;


        public ReportDeskViewModel(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        public ReportDeskViewModel(NavigationService navigationService, IDataService dataService, PDFService pdfService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _pdfService = pdfService;
        }


        #region Properties




        #region Collections




        private ObservableCollection<SearchResultViewModel> searchBarItems = new();
        public ObservableCollection<SearchResultViewModel> SearchBarItems
        {
            get
            {
                return searchBarItems;
            }
            set
            {
                searchBarItems = value;
                OnPropertyChanged(nameof(SearchBarItems));
            }
        }



        private ObservableCollection<REPORTViewModel> logs = new();
        public ObservableCollection<REPORTViewModel> Logs
        {
            get
            {
                return logs;
            }
            set
            {
                logs = value;

                SetNoLogsMessageVisibility();
                OnPropertyChanged(nameof(Logs));
            }
        }

        private Visibility noLogsMessageVisibility;
        public Visibility NoLogsMessageVisibility
        {
            get
            {
                return noLogsMessageVisibility;
            }
            set
            {
                noLogsMessageVisibility = value;
                OnPropertyChanged(nameof(NoLogsMessageVisibility));
            }
        }



        #endregion




        private bool deskSet = false;
        public bool DeskSet
        {
            get
            {
                return deskSet;
            }
            set
            {
                deskSet = value;
                OnPropertyChanged(nameof(DeskSet));
            }
        }

        private string query = string.Empty;
        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                if (query != value)
                {
                    query = value;
                    OnPropertyChanged(nameof(Query));
                }
            }
        }


        private string project = string.Empty;
        public string Project
        {
            get
            {
                return project;
            }
            set
            {
                project = value;
                OnPropertyChanged(nameof(Project));
            }
        }

        private ObservableCollection<string> projects = new();
        public ObservableCollection<string> Projects
        {
            get
            {
                return projects;
            }
            set
            {
                projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        private string application = string.Empty;
        public string Application
        {
            get
            {
                return application;
            }
            set
            {
                application = value;
                OnPropertyChanged(nameof(Application));
            }
        }

        private ObservableCollection<string> applications = new();
        public ObservableCollection<string> Applications
        {
            get
            {
                return applications;
            }
            set
            {
                applications = value;
                OnPropertyChanged(nameof(Applications));
            }
        }

        public bool ApplicationEnabled { get; set; } = true;

        public string ApplicationToolTip { get; set; } = "Enter the application used for the project.";



        #endregion



        #region Commands



        public ICommand SearchCommand { get; set; }

        public ICommand ExportCommand { get; set; }

        public ICommand ReturnToDashboard { get; set; }




        #endregion



        #region Methods




        public abstract Task UpdateLogsListAsync();
        public abstract Task InitialiseAppsAsync();
        public abstract Task InitialiseProjectsAsync();

        protected void SetNoLogsMessageVisibility()
        {
            NoLogsMessageVisibility = Logs.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }














        #endregion
    }
}
