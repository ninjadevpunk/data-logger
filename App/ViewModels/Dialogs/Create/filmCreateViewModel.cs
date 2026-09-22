using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    public class filmCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.FILM;
        public override CacheContext Context => CacheContext.Film;
        public override string LogType => "FILM LOG";

        private readonly FilmViewModel _filmViewModel;

        public filmCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService)
            : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "Blender";

            _filmViewModel = (FilmViewModel)logCacheViewModel;

            InitializeDefaults();

            BrowseCommand = new BrowseCommand(Context, this);
            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _filmViewModel, _dataService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
        }

        public async Task AutoStartAsync()
        {
            await InitializeProjectsAndApplications();

            await UpdateLogCount();
        }

        private async Task InitializeProjectsAndApplications()
        {
            await _dataService.InitialiseProjectsLISTAsync(Category);
            var items = _dataService.SQLITE_PROJECTS;

            var apps = _dataService.SQLITE_APPLICATIONS;

            foreach (ProjectClass project in items)
            {
                _projects.Add(project.Name);
            }

            await _dataService.InitialiseApplicationsLISTAsync(Category);
            foreach (ApplicationClass app in apps)
            {
                _applications.Add(app.Name);
            }
        }

        private void InitializeDefaults()
        {
            Output = "MP4";
            _outputs.Add("MP4");
            _outputs.Add("AVI");
            _outputs.Add("MKV");
            _outputs.Add("TS");
            _outputs.Add("WEBM");

            Type = "Film";
            _types.Add("Film");
            _types.Add("NONE");
            _types.Add("Doodle");
            _types.Add("Assignment");

            Height = string.Empty;
            Width = string.Empty;
            Resolution = string.Empty;
            Length = string.Empty;
            IsCompleted = false;
            Source = string.Empty;
        }













        #region Properties




        private string height;
        public string Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string width;
        public string Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private string resolution;
        public string Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                OnPropertyChanged(nameof(Resolution));
            }
        }

        private string length;
        public string Length
        {
            get => length;
            set
            {
                length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        private bool isCompleted;
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        private string source;
        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }



        #endregion







        #region Member Functions




        public async Task UpdateLogCount()
        {
            if (_filmViewModel is not null)
            {
                var count = await _dataService.LogCount(Category);
                LogCount = $"{_filmViewModel.CacheItems.Count} Logs Cached";
                _filmViewModel.LogCount = $"{_filmViewModel.CacheItems.Count} film logs cached | {count} total logs";
            }
        }

        #endregion




    }
}
