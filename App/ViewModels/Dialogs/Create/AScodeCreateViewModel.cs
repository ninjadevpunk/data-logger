using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dialogs.Create
{
    public class AScodeCreateViewModel : codeCreateViewModel
    {
        private readonly CodingAndroidViewModel _ASviewModel;

        public AScodeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService)
            : base(navigationService, logCacheViewModel, dataService)
        {
            ApplicationName = AndroidStudio;
            ApplicationToolTip = "This field cannot be changed. Only Android Studio logs are allowed in this tab.";
            ASOnly = true;
            AppFieldEnabled = false;
            IsSimple = false;
            IsConsidered = false;

            _ASviewModel = (CodingAndroidViewModel)_viewModel;

            

            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _ASviewModel, _dataService);
        }

        public override async Task AutoStartAsync()
        {
            await InitializeProjects();
            await InitializeOutputTypes();
            await InitializeLogTypes();

            await UpdateLogCount();
        }















        #region Properties



        private bool isSimple;
        public bool IsSimple
        {
            get => isSimple;
            set
            {
                isSimple = value;
                SetGradleDaemonVisibility();
                OnPropertyChanged(nameof(IsSimple));
            }
        }

        private bool isConsidered;
        public bool IsConsidered
        {
            get
            {
                return isConsidered;
            }
            set
            {
                isConsidered = value;
                OnPropertyChanged(nameof(IsConsidered));
            }
        }

        #endregion






        #region Initialization Methods




        private async Task InitializeProjects()
        {
            await _dataService.InitialiseProjectsLISTAsync(LOG.CATEGORY.CODING);
            LoadProjects();

            _applications.Clear();
        }

        private void LoadProjects()
        {
            var items = _dataService.SQLITE_PROJECTS;
            _projects.Clear();

            foreach (ProjectClass project in items)
            {
                if (project.Application.Name == AndroidStudio)
                    _projects.Add(project.Name);
            }
        }

        private async Task InitializeOutputTypes()
        {
            Output = "APK (*.apk)";
            _outputs.Clear();
            
            foreach(var output in await _dataService.ListASOutputs())
            {
                _outputs.Add(output.Name);
            }
        }

        private async Task InitializeLogTypes()
        {
            Type = "Sync";
            _types.Clear();

            foreach(var type in await _dataService.ListASTypes())
            {
                _types.Add(type.Name);
            }
        }




        #endregion









        #region Log Count

        public override async Task UpdateLogCount()
        {
            if (ASOnly && _ASviewModel is not null)
            {
                var count = await _dataService.ASLogCount();
                LogCount = $"{_ASviewModel.CacheItems.Count} Logs Cached";
                _ASviewModel.LogCount = $"{_ASviewModel.CacheItems.Count} android logs cached | {count} total logs";
            }
        }

        #endregion




        #region UI Visibility Handling

        private void SetGradleDaemonVisibility()
        {
            var visibility = IsSimple ? Visibility.Hidden : Visibility.Visible;

            GradleDaemonVisibility = visibility;
        }

        #endregion













        #region Time Tracking Properties





        private void UpdateTime(ref DateTime timeField, int hours, int minutes, int seconds, int milliseconds)
        {
            timeField = DateTime.Parse($"{DateTime.Now.ToLongDateString()} {hours}:{minutes}:{seconds}.{milliseconds}");
        }

        private void SetAndNotify(ref DateTime field, DateTime value, string propertyName)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        private void SetAndNotify(ref Visibility field, Visibility value, string propertyName)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        private void SetAndNotify(ref int field, int value, Action updateMethod, string propertyName)
        {
            field = value;
            updateMethod();
            OnPropertyChanged(propertyName);
        }








        #region Sync Time



        private DateTime syncTime;
        public DateTime SyncTime { get => syncTime; set => SetAndNotify(ref syncTime, value, nameof(SyncTime)); }

        private int syncHours, syncMinutes, syncSeconds, syncMilliseconds;
        public int SyncHours { get => syncHours; set => SetAndNotify(ref syncHours, value, UpdateSyncTime, nameof(SyncHours)); }
        public int SyncMinutes { get => syncMinutes; set => SetAndNotify(ref syncMinutes, value, UpdateSyncTime, nameof(SyncMinutes)); }
        public int SyncSeconds { get => syncSeconds; set => SetAndNotify(ref syncSeconds, value, UpdateSyncTime, nameof(SyncSeconds)); }
        public int SyncMilliseconds { get => syncMilliseconds; set => SetAndNotify(ref syncMilliseconds, value, UpdateSyncTime, nameof(SyncMilliseconds)); }

        private void UpdateSyncTime() => UpdateTime(ref syncTime, syncHours, syncMinutes, syncSeconds, syncMilliseconds);



        #endregion




        #region Gradle Daemon Time



        private Visibility gradleDaemonVisibility;
        public Visibility GradleDaemonVisibility { get => gradleDaemonVisibility; set => SetAndNotify(ref gradleDaemonVisibility, value, nameof(GradleDaemonVisibility)); }

        private DateTime gradleDaemonTime;
        public DateTime GradleDaemonTime { get => gradleDaemonTime; set => SetAndNotify(ref gradleDaemonTime, value, nameof(GradleDaemonTime)); }

        private int gradleDaemonHours, gradleDaemonMinutes, gradleDaemonSeconds, gradleDaemonMilliseconds;
        public int GradleDaemonHours { get => gradleDaemonHours; set => SetAndNotify(ref gradleDaemonHours, value, UpdateGradleDaemonTime, nameof(GradleDaemonHours)); }
        public int GradleDaemonMinutes { get => gradleDaemonMinutes; set => SetAndNotify(ref gradleDaemonMinutes, value, UpdateGradleDaemonTime, nameof(GradleDaemonMinutes)); }
        public int GradleDaemonSeconds { get => gradleDaemonSeconds; set => SetAndNotify(ref gradleDaemonSeconds, value, UpdateGradleDaemonTime, nameof(GradleDaemonSeconds)); }
        public int GradleDaemonMilliseconds { get => gradleDaemonMilliseconds; set => SetAndNotify(ref gradleDaemonMilliseconds, value, UpdateGradleDaemonTime, nameof(GradleDaemonMilliseconds)); }

        private void UpdateGradleDaemonTime() => UpdateTime(ref gradleDaemonTime, gradleDaemonHours, gradleDaemonMinutes, gradleDaemonSeconds, gradleDaemonMilliseconds);



        #endregion





        #region Run Build Time




        private Visibility runBuildVisibility;
        public Visibility RunBuildVisibility { get => runBuildVisibility; set => SetAndNotify(ref runBuildVisibility, value, nameof(RunBuildVisibility)); }

        private DateTime runBuildTime;
        public DateTime RunBuildTime { get => runBuildTime; set => SetAndNotify(ref runBuildTime, value, nameof(RunBuildTime)); }

        private int runBuildHours, runBuildMinutes, runBuildSeconds, runBuildMilliseconds;
        public int RunBuildHours { get => runBuildHours; set => SetAndNotify(ref runBuildHours, value, UpdateRunBuildTime, nameof(RunBuildHours)); }
        public int RunBuildMinutes { get => runBuildMinutes; set => SetAndNotify(ref runBuildMinutes, value, UpdateRunBuildTime, nameof(RunBuildMinutes)); }
        public int RunBuildSeconds { get => runBuildSeconds; set => SetAndNotify(ref runBuildSeconds, value, UpdateRunBuildTime, nameof(RunBuildSeconds)); }
        public int RunBuildMilliseconds { get => runBuildMilliseconds; set => SetAndNotify(ref runBuildMilliseconds, value, UpdateRunBuildTime, nameof(RunBuildMilliseconds)); }

        private void UpdateRunBuildTime() => UpdateTime(ref runBuildTime, runBuildHours, runBuildMinutes, runBuildSeconds, runBuildMilliseconds);



        #endregion




        #region Load Build Time



        private Visibility loadBuildVisibility;
        public Visibility LoadBuildVisibility { get => loadBuildVisibility; set => SetAndNotify(ref loadBuildVisibility, value, nameof(LoadBuildVisibility)); }

        private DateTime loadBuildTime;
        public DateTime LoadBuildTime { get => loadBuildTime; set => SetAndNotify(ref loadBuildTime, value, nameof(LoadBuildTime)); }

        private int loadBuildHours, loadBuildMinutes, loadBuildSeconds, loadBuildMilliseconds;
        public int LoadBuildHours { get => loadBuildHours; set => SetAndNotify(ref loadBuildHours, value, UpdateLoadBuildTime, nameof(LoadBuildHours)); }
        public int LoadBuildMinutes { get => loadBuildMinutes; set => SetAndNotify(ref loadBuildMinutes, value, UpdateLoadBuildTime, nameof(LoadBuildMinutes)); }
        public int LoadBuildSeconds { get => loadBuildSeconds; set => SetAndNotify(ref loadBuildSeconds, value, UpdateLoadBuildTime, nameof(LoadBuildSeconds)); }
        public int LoadBuildMilliseconds { get => loadBuildMilliseconds; set => SetAndNotify(ref loadBuildMilliseconds, value, UpdateLoadBuildTime, nameof(LoadBuildMilliseconds)); }

        private void UpdateLoadBuildTime() => UpdateTime(ref loadBuildTime, loadBuildHours, loadBuildMinutes, loadBuildSeconds, loadBuildMilliseconds);



        #endregion





        #region Configure Build Time



        private Visibility configureBuildVisibility;
        public Visibility ConfigureBuildVisibility { get => configureBuildVisibility; set => SetAndNotify(ref configureBuildVisibility, value, nameof(ConfigureBuildVisibility)); }

        private DateTime configureBuildTime;
        public DateTime ConfigureBuildTime { get => configureBuildTime; set => SetAndNotify(ref configureBuildTime, value, nameof(ConfigureBuildTime)); }

        private int configureBuildHours, configureBuildMinutes, configureBuildSeconds, configureBuildMilliseconds;
        public int ConfigureBuildHours { get => configureBuildHours; set => SetAndNotify(ref configureBuildHours, value, UpdateConfigureBuildTime, nameof(ConfigureBuildHours)); }
        public int ConfigureBuildMinutes { get => configureBuildMinutes; set => SetAndNotify(ref configureBuildMinutes, value, UpdateConfigureBuildTime, nameof(ConfigureBuildMinutes)); }
        public int ConfigureBuildSeconds { get => configureBuildSeconds; set => SetAndNotify(ref configureBuildSeconds, value, UpdateConfigureBuildTime, nameof(ConfigureBuildSeconds)); }
        public int ConfigureBuildMilliseconds { get => configureBuildMilliseconds; set => SetAndNotify(ref configureBuildMilliseconds, value, UpdateConfigureBuildTime, nameof(ConfigureBuildMilliseconds)); }

        private void UpdateConfigureBuildTime() => UpdateTime(ref configureBuildTime, configureBuildHours, configureBuildMinutes, configureBuildSeconds, configureBuildMilliseconds);



        #endregion




        #region All Projects Time



        private Visibility allProjectsVisibility;
        public Visibility AllProjectsVisibility { get => allProjectsVisibility; set => SetAndNotify(ref allProjectsVisibility, value, nameof(AllProjectsVisibility)); }

        private DateTime allProjectsTime;
        public DateTime AllProjectsTime { get => allProjectsTime; set => SetAndNotify(ref allProjectsTime, value, nameof(AllProjectsTime)); }

        private int allProjectsHours, allProjectsMinutes, allProjectsSeconds, allProjectsMilliseconds;
        public int AllProjectsHours { get => allProjectsHours; set => SetAndNotify(ref allProjectsHours, value, UpdateAllProjectsTime, nameof(AllProjectsHours)); }
        public int AllProjectsMinutes { get => allProjectsMinutes; set => SetAndNotify(ref allProjectsMinutes, value, UpdateAllProjectsTime, nameof(AllProjectsMinutes)); }
        public int AllProjectsSeconds { get => allProjectsSeconds; set => SetAndNotify(ref allProjectsSeconds, value, UpdateAllProjectsTime, nameof(AllProjectsSeconds)); }
        public int AllProjectsMilliseconds { get => allProjectsMilliseconds; set => SetAndNotify(ref allProjectsMilliseconds, value, UpdateAllProjectsTime, nameof(AllProjectsMilliseconds)); }

        private void UpdateAllProjectsTime() => UpdateTime(ref allProjectsTime, allProjectsHours, allProjectsMinutes, allProjectsSeconds, allProjectsMilliseconds);



        #endregion








        #endregion




    }
}
