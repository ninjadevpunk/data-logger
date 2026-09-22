using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    public class flexiCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.NOTES;
        public override CacheContext Context => CacheContext.Flexi;
        public override string LogType => "FLEXIBLE LOG";



        private readonly FlexiViewModel _flexiViewModel;

        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;
        private readonly ObservableCollection<string> _flexiLogCategories;

        public IEnumerable<string> Mediums => _mediums;
        public IEnumerable<string> Formats => _formats;
        public IEnumerable<string> FlexiLogCategories => _flexiLogCategories;

        public flexiCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "Unity";

            _flexiViewModel = (FlexiViewModel)logCacheViewModel;

            InitializeDefaults();

            _mediums = new ObservableCollection<string>
            {
                "Game Engine",
                "Song",
                "A Cappella",
                "Orchestral"
            };

            _formats = new ObservableCollection<string>
            {
                "Digital Download",
                "Disc",
                "CD",
                "MIDI",
                "Digital",
                "Tape",
                "LP",
                "EP",
                "Gramophone"
            };

            _flexiLogCategories = new ObservableCollection<string>
            {
                "Document",
                "Gaming",
                "Music"
            };


            BrowseCommand = new BrowseCommand(Context, this);
            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _flexiViewModel, _dataService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
        }










        public async Task AutoStartAsync()
        {
            await InitializeProjectsAndApplications(_dataService);

            await UpdateLogCount();
        }

        private async Task InitializeProjectsAndApplications(IDataService dataService)
        {
            await dataService.InitialiseProjectsLISTAsync(Category);
            var items = dataService.SQLITE_PROJECTS;
            var apps = dataService.SQLITE_APPLICATIONS;

            foreach (ProjectClass project in items)
            {
                _projects.Add(project.Name);
            }

            await dataService.InitialiseApplicationsLISTAsync(Category);

            foreach (ApplicationClass app in apps)
            {
                _applications.Add(app.Name);
            }

        }

        private void InitializeDefaults()
        {
            Output = "EXE (*.exe)";
            _outputs.Add("EXE (*.exe)");
            _outputs.Add("PDF");
            _outputs.Add("Joint Pictures Experts Group (*.JPG) | (*.jpg)");
            _outputs.Add("Scalable Vector Graphics (*.svg)");
            _outputs.Add("Microsoft Word (*.docx) | (*.doc)");
            _outputs.Add("Text File (*.txt)");
            _outputs.Add("HTML (*.html)");
            _outputs.Add("XPS (*.xps)");
            _outputs.Add("Microsoft Excel (*.xslx)");
            _outputs.Add("Microsoft Access (*.accdb)");
            _outputs.Add("Motion Picture Experts Group Layer 3 (*.mp3)");
            _outputs.Add("WAV File (*.wav)");
            _outputs.Add("AAC File (*.aac)");
            _outputs.Add("M4A (*.m4a)");
            _outputs.Add("OGG (*.ogg)");

            Type = "Boardgame";
            _types.Add("Document");
            _types.Add("NONE");
            _types.Add("Invitation");
            _types.Add("Mail");
            _types.Add("Doodle");
            _types.Add("Report");
            _types.Add("Novel");
            _types.Add("Questionnaire");
            _types.Add("Curriculum Vitae");
            _types.Add("Website");
            _types.Add("Presentation");
            _types.Add("Spreadsheet");
            _types.Add("Notes");
            _types.Add("Database");
            _types.Add("Boardgame");
            _types.Add("Video Game");
            _types.Add("Card Game");
            _types.Add("Game");
            _types.Add("Music");
            _types.Add("Mantra");
            _types.Add("Poetry");
        }




        #region Properties




        private string flexibleLogCategory;
        public string FlexibleLogCategory
        {
            get => flexibleLogCategory;
            set
            {
                flexibleLogCategory = value;
                OnPropertyChanged(nameof(FlexibleLogCategory));
            }
        }

        private string medium;
        public string Medium
        {
            get => medium;
            set
            {
                medium = value;
                OnPropertyChanged(nameof(Medium));
            }
        }

        private string format;
        public string Format
        {
            get => format;
            set
            {
                format = value;
                OnPropertyChanged(nameof(Format));
            }
        }

        private string bitrate;
        public string Bitrate
        {
            get => bitrate;
            set
            {
                bitrate = value;
                OnPropertyChanged(nameof(Bitrate));
            }
        }

        private string duration;
        public string Duration
        {
            get => duration;
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
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
            if (_flexiViewModel is not null)
            {
                var count = await _dataService.LogCount(Category);
                LogCount = $"{_flexiViewModel.CacheItems.Count} Logs Cached";
                _flexiViewModel.LogCount = $"{_flexiViewModel.CacheItems.Count} flexible logs cached | {count} total logs";
            }
        }

        #endregion
    }
}
