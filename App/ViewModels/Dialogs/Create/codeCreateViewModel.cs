using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.CacheCommands;
using Data_Logger_1._3.Commands.DLSFileCommands;
using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    internal class codeCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override LogContext Context => LogContext.Coding;
        public override string LogType => "CODING LOG";

        protected readonly CodingViewModel _codingViewModel;
        protected override bool AppFieldEnabled => true;


        protected bool QtOnly;
        protected bool ASOnly;

        public override ICommand AnnotateCommand => new AnnotateCommand(_navigationService, this, _codingViewModel, _authService, _dataService, 
            _cacheService, Context);
        public override ICommand SaveCommand => new SaveCommand(this, _authService, _dataService, _cacheService);

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IAuthService authService,
            IDataService dataService, ICacheService cacheService)
            : base(navigationService, logCacheViewModel, authService, dataService, cacheService)
        {
            _codingViewModel = (CodingViewModel)logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel),
                    "codeCreateViewModel");
            InitializeCommonFields();

            BugsFound = 0;
            ApplicationOpened = false;

            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
        }










        public virtual async Task AutoStartAsync()
        {
            await LoadProjectsAndApplicationsAsync();

            await LoadDefaultOutputs();
            await LoadDefaultTypes();

            await UpdateLogCount();
        }

        public virtual async Task AutoStartAsync(bool qtOnly)
        {
            await _dataService.InitialiseProjectsLISTAsync(Category);
            await LoadProjectsAndApplicationsAsync();
            await LoadDefaultOutputs();
            await LoadDefaultTypes();
            await UpdateLogCount();
        }

        private void InitializeCommonFields()
        {
            ApplicationName = Constants.VisualStudio;
        }

        private async Task LoadProjectsAndApplicationsAsync()
        {
            var items = await _dataService.InitialiseProjectsLISTAsync(Category);
            var apps = await _dataService.InitialiseApplicationsLISTAsync(Category);
            _projects.Clear();
            _applications.Clear();

            foreach (var project in items)
            {
                _projects.Add(project);
            }

            foreach (var app in apps)
            {
                _applications.Add(app);
            }
        }

        

        private async Task LoadDefaultOutputs()
        {
            Output = "C# Application (*.exe)";

            foreach (var output in await _dataService.ListOutputsAsync(Category))
            {
                _outputs.Add(output);
            }

        }

        private async Task LoadQtOutputs()
        {
            Output = "Widgets Application";

            foreach (var output in await _dataService.ListQtOutputsAsync())
            {
                _outputs.Add(output);
            }
        }

        private async Task LoadDefaultTypes()
        {
            Type = "Exception";

            foreach (var type in await _dataService.ListTypesAsync(Category))
            {
                _types.Add(type);
            }

        }

        private async Task LoadQtTypes()
        {
            Type = "Build";

            foreach (var type in await _dataService.ListQtTypesAsync())
            {
                _types.Add(type);
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

        public override async Task UpdateLogCount()
        {
            if (_codingViewModel != null)
            {
                var count = await _dataService.LogCountAsync(Category);
                LogCount = $"{_codingViewModel.CacheItems.Count} Logs Cached";
                _codingViewModel.LogCount = $"{_codingViewModel.CacheItems.Count} coding logs cached | {count} total logs";
            }
        }

        #endregion
    }
}
