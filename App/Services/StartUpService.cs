using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Notes;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    public class StartUpService
    {
        #region SERVICES


        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly SettingsService _settingsService;
        private readonly ICacheService _cacheMaster;

        #endregion



        private readonly MainWindowViewModel _mainWindowViewModel;



        #region Loggers



        private readonly qtCreateViewModel _qtLogger;
        private readonly AScodeCreateViewModel _asLogger;
        private readonly codeCreateViewModel _codingLogger;




        #endregion




        #region Dashboard 



        private readonly CodingQtViewModel _codingQtDashboard;

        private readonly CodingAndroidViewModel _codingAndroidDashboard;

        private readonly CodingViewModel _codingDashboard;

        private readonly NOTESDashboardViewModel _notesDashboard;



        #endregion






        #region Editors





        #endregion







        #region Viewers





        #endregion






        #region Updaters








        #endregion





        /// <summary>
        /// Retrieve the viewmodels and services from DI
        /// </summary>
        /// <param name="serviceProvider">The DI instance</param>
        public StartUpService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _authService = serviceProvider.GetRequiredService<IAuthService>();
            _dataService = serviceProvider.GetRequiredService<IDataService>();
            _settingsService = serviceProvider.GetRequiredService<SettingsService>();
            _cacheMaster = serviceProvider.GetRequiredService<ICacheService>();

            var userId = _authService.GetUser().UserId;

            var qtList = _cacheMaster.LoadQtCodeCache(userId);
            var asList = _cacheMaster.LoadASCodeCache(userId);
            var cdeList = _cacheMaster.LoadCodeCache(userId);

            var logViewModelFactory = _serviceProvider.GetRequiredService<LOGViewModelFactory>();
            _codingQtDashboard = serviceProvider.GetRequiredService<CodingQtViewModel>();
            _codingAndroidDashboard = serviceProvider.GetRequiredService<CodingAndroidViewModel>();
            _codingDashboard = serviceProvider.GetRequiredService<CodingViewModel>();

            try
            {
                var navigationService = _serviceProvider.GetRequiredService<NavigationService>();



                var qtLogs = ConvertLogsToViewModels(qtList, qtLog => logViewModelFactory.Create((QtCacheDTO)qtLog, _codingQtDashboard, false));
                LoadCacheItems(qtLogs, _codingQtDashboard);

                var cdeLogs = ConvertLogsToViewModels(cdeList, codingLog => new CodeLOGViewModel(navigationService, (CodingCacheDTO)codingLog,
                    _codingDashboard, _dataService, _cacheMaster, false));
                LoadCacheItems(cdeLogs, _codingDashboard);

                var asLogs = ConvertLogsToViewModels(asList, androidLog => logViewModelFactory.Create((AndroidCacheDTO)androidLog,
                    _codingAndroidDashboard, false));
                LoadCacheItems(asLogs, _codingAndroidDashboard);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} LoadCachedLogs: {ex.Message}");
            }

            _notesDashboard = serviceProvider.GetRequiredService<NOTESDashboardViewModel>();

            _qtLogger = serviceProvider.GetRequiredService<qtCreateViewModel>();
            _asLogger = serviceProvider.GetRequiredService<AScodeCreateViewModel>();
            _codingLogger = serviceProvider.GetRequiredService<codeCreateViewModel>();

            _mainWindowViewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
        }

        /// <summary>
        /// Call to start up required operations.
        /// </summary>
        public async Task Initialise(int id)
        {
            await _dataService.InitialiseApplicationsLISTAsync();
            await _dataService.InitialiseProjectsLISTAsync();

            LoadCachedLogs();
            await LoadNotes(id);
            SetUpSettings();
            SetupUserProfile();
        }

        private void LoadCachedLogs()
        {
            
        }

        private ObservableCollection<LOGViewModel> ConvertLogsToViewModels(IEnumerable<LogCacheDTO> logs, Func<LogCacheDTO, LOGViewModel> converter)
        {
            ObservableCollection<LOGViewModel> list = [];

            foreach (var log in logs)
                list.Add(converter(log));

            return list;
        }

        private void LoadCacheItems(ObservableCollection<LOGViewModel> cacheList, LogCacheViewModel dashboard) 
        {
            if (cacheList.Count > 0)
                dashboard.CacheItems = cacheList;
        }

        private async Task LoadNotes(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

            foreach (var note in await dataService.RetrieveGenericNotesAsync(id) ?? [])
            {
                _notesDashboard.NoteItems.Add(new(_dataService, _notesDashboard, new NotesViewItemDTO
                {
                    NoteDTO = new NoteDataDTO
                    {
                        UserId = note.accountID,
                        NoteItemId = note.ID,
                        CreationDate = note.Start.ToLongDateString(),
                        ModifiedDate = note.End.ToLongDateString(),
                        NoteContent = note.Content,
                    },
                    Subject = note.Subject,
                    Content = note.Content,
                    CreationDate = note.Start.ToLongDateString()
                }));
            }

            var hasNotes = _notesDashboard.NoteItems.Count > 0;
            _notesDashboard.StartUpVisibilitySet(hasNotes);
        }

        private void SetupUserProfile()
        {
            var user = _authService.GetUser();
            if (user != null)
            {
                if (user.ProfilePic!.Contains(Constants.Avatar))
                {
                    _mainWindowViewModel.SignUpImage = null;
                    _mainWindowViewModel.ShowDefault = Visibility.Visible;
                }
                else
                    _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(user.ProfilePic);
            }
        }


        private void SetUpSettings()
        {
            try
            {
                var user = _authService.GetUser();

                var id = user.UserId;

                if (_settingsService.CreateSettingsFile())
                {
                    var settings = _settingsService.Load(id);

                    if (settings != null && string.IsNullOrWhiteSpace(settings.User.Email))
                    {
                        settings.User.Id = user.UserId;
                        settings.User.Name = user.Name;
                        settings.User.Surname = user.Surname;
                        settings.User.Email = user.Email;
                        settings.User.ProfilePic = user.ProfilePic ?? Constants.Avatar;

                        _settingsService.Save(id, settings, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ArgumentNullException in SetUpSettings(): {ex.Message}");
            }

        }








    }
}
