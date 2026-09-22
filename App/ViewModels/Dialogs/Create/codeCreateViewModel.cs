using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    public class codeCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override CacheContext Context => CacheContext.Coding;
        public override string LogType => "CODING LOG";

        protected readonly CodingViewModel _viewModel;
        private readonly CodingQtViewModel? _QtviewModel;

        private const string Qt = "Qt Creator";
        protected const string AndroidStudio = "Android Studio Meerkat 2024.3.1";
        protected const string VisualStudio = "Visual Studio Community 2022";

        private bool QtOnly = false;
        protected bool ASOnly = false;

        public codeCreateViewModel()
        {

        }

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService)
            : base(navigationService, logCacheViewModel, dataService)
        {
            _viewModel = (CodingViewModel)logCacheViewModel;
            InitializeCommonFields(logCacheViewModel);


            BugsFound = 0;
            ApplicationOpened = false;

            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _viewModel, _dataService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
        }



        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string application, IDataService dataService)
            : base(navigationService, logCacheViewModel, dataService)
        {

            if (application is Qt || application is "Qt")
            {
                _QtviewModel = (CodingQtViewModel)logCacheViewModel;
            }
            else
            {
                _viewModel = (CodingViewModel)logCacheViewModel;
                InitializeCommonFields(logCacheViewModel);

                AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _viewModel, _dataService);
            }

            SaveCommand = new SaveCommand(this, _dataService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
        }











        public virtual async Task AutoStartAsync()
        {
            await _dataService.InitialiseProjectsLISTAsync(Category);
            await _dataService.InitialiseApplicationsLISTAsync(Category);
            LoadProjectsAndApplications();

            await LoadDefaultOutputs();
            await LoadDefaultTypes();

            await UpdateLogCount();
        }

        public virtual async Task AutoStartAsync(bool qtOnly)
        {
            await _dataService.InitialiseProjectsLISTAsync(Category);

            if (qtOnly && _QtviewModel != null)
            {
                await InitializeQtFields(_QtviewModel);
            }
            else
            {
                LoadProjectsAndApplications();

                await LoadDefaultOutputs();
                await LoadDefaultTypes();
            }

            await UpdateLogCount();
        }

        private void InitializeCommonFields(LogCacheViewModel logCacheViewModel)
        {
            AppFieldEnabled = true;
            ApplicationName = VisualStudio;
        }

        private async Task InitializeQtFields(LogCacheViewModel logCacheViewModel)
        {
            AppFieldEnabled = false;
            ApplicationToolTip = "This field cannot be changed. Only Qt logs are allowed in this tab.";
            ApplicationName = Qt;
            QtOnly = true;

            LoadQtProjects();
            await LoadQtOutputs();
            await LoadQtTypes();

            AnnotateCommand = new AnnotateCommand(CacheContext.Qt, _navigationService, this, _QtviewModel, _dataService);
        }

        private void LoadProjectsAndApplications()
        {
            var items = _dataService.SQLITE_PROJECTS;
            var apps = _dataService.SQLITE_APPLICATIONS;
            _projects.Clear();
            _applications.Clear();

            foreach (ProjectClass project in items)
            {
                _projects.Add(project.Name);
            }

            foreach (ApplicationClass app in apps)
            {
                _applications.Add(app.Name);
            }
        }

        private void LoadQtProjects()
        {
            var items = _dataService.SQLITE_PROJECTS;

            foreach (ProjectClass project in items)
            {
                if (project.Application.Name == Qt)
                    _projects.Add(project.Name);
            }
        }

        private async Task LoadDefaultOutputs()
        {
            Output = "C# Application (*.exe)";

            foreach (var output in await _dataService.ListOutputs(Category))
            {
                _outputs.Add(output.Name);
            }

        }

        private async Task LoadQtOutputs()
        {
            Output = "Widgets Application";

            foreach (var output in await _dataService.ListQtOutputs())
            {
                _outputs.Add(output.Name);
            }
        }

        private async Task LoadDefaultTypes()
        {
            Type = "Exception";

            foreach (var type in await _dataService.ListTypes(Category))
            {
                _types.Add(type.Name);
            }

        }

        private async Task LoadQtTypes()
        {
            Type = "Build";

            foreach (var type in await _dataService.ListQtTypes())
            {
                _types.Add(type.Name);
            }
        }

        public string ApplicationToolTip { get; set; } = "The application's name.";
        public string TimeStartToolTip { get; set; } = "Updates the current start time AND end time. Does not update automatically!";
        public string TimeEndToolTip { get; set; } = "Updates the END TIME ONLY. Does not update automatically.";
        public string OutputToolTip { get; set; } = "Please select from this list. Options cannot be modified.";
        public string TypeToolTip { get; set; } = "Please select from this list. Options cannot be modified.";



        private int bugsFound;
        public int BugsFound
        {
            get => bugsFound;
            set
            {
                bugsFound = value;
                OnPropertyChanged(nameof(BugsFound));
            }
        }

        private bool applicationOpened;
        public bool ApplicationOpened
        {
            get => applicationOpened;
            set
            {
                applicationOpened = value;
                OnPropertyChanged(nameof(ApplicationOpened));
            }
        }

        #region Member Functions

        public virtual async Task UpdateLogCount()
        {
            if (QtOnly && _QtviewModel is not null)
            {
                var count = await _dataService.QtLogCount();
                LogCount = $"{_QtviewModel.CacheItems.Count} Logs Cached";
                _QtviewModel.LogCount = $"{_QtviewModel.CacheItems.Count} qt logs cached | {count} total logs";
            }
            else if (_viewModel is not null)
            {
                var count = await _dataService.LogCount(Category);
                LogCount = $"{_viewModel.CacheItems.Count} Logs Cached";
                _viewModel.LogCount = $"{_viewModel.CacheItems.Count} coding logs cached | {count} total logs";
            }
        }

        #endregion
    }
}
