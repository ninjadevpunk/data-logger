using Core.DTOs;
using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Viewer;
using Core.Enums;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data.Services;
using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.Components.Subcontrols_View;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Account;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using FileSys.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Data_Logger_1._3.Services
{
    internal class NavigationService
    {
        public static void CloseApp() => Application.Current.Shutdown(1);

        /// <summary>
        /// Used to track the current page the MainWindow's frame is on.
        /// Use it in the MainWindowViewModel to authorise or deny navigation paths.
        /// </summary>
        public enum NavContext
        {
            DASHBOARD, LOGGER, POSTIT,
            NOTES, CREATENOTE,
            VIEWER,
            EDITOR, EDITOR_POSTIT,
            REPORTER_DASHBOARD, UPDATER, UPDATER_POSTIT
        }

        private readonly IServiceProvider _serviceProvider;

        // LOGIN
        private Login? _login;

        // MAIN WINDOW
        private MainWindow _mainWindow = null!;

        // MAIN FRAME
        private Frame _mainWindowFrame = null!;

        // LOGGER CREATOR SUBCLASS FRAME
        private Frame _loggerFrame = null!;

        // LOGGER CREATOR ANDROID STUDIO FRAME
        private Frame? _AndroidStudioFrame;


        public NavContext NavigationContext { get; set; } = NavContext.DASHBOARD;
        public LogContext LogContext { get; set; } = LogContext.Qt;







        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }






        /// <summary>
        /// Set the MainWindow's frame to _mainWindowFrame.
        /// </summary>
        /// <param name="frame">The frame inside MainWindow window.</param>
        public void SetMainFrame(Frame frame) => _mainWindowFrame = frame;

        /// <summary>
        /// Event triggered when navigating back from the editor page.
        /// </summary>
        public event Action<Timer>? BackFromEditor;

        /// <summary>
        /// The specific instance of Timer belonging to the log cache item requesting the editor.
        /// </summary>
        private Timer? _editorTimer;

        /// <summary>
        /// Navigate back in the main frame's history - if possible.
        /// </summary>
        /// <param name="isPostItPage">Specifies if the user is on the PostIt page</param>
        public void GoBack(bool isPostItPage = false)
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();

            if (_mainWindowFrame.CanGoBack)
            {
                var contextBeforeBack = NavigationContext;

                if (!isPostItPage)
                {
                    if (NavigationContext == NavContext.POSTIT || NavigationContext == NavContext.EDITOR_POSTIT)
                    {
                        var result = MessageBox.Show(
                            "Your post it content will not be saved. Are you sure you would like to go back to the logger?",
                            "You Sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;
                    }
                    else if (NavigationContext == NavContext.CREATENOTE)
                    {
                        var result = MessageBox.Show(
                            "Your note will not be saved. Are you sure you would like to go back to the notes dashboard?",
                            "Your Note Is Empty", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;

                        mainWindowViewModel.GenericNotesChecked = false;
                        NavigationContext = NavContext.NOTES;
                    }
                    else if (NavigationContext == NavContext.LOGGER || NavigationContext == NavContext.EDITOR)
                    {
                        var result = MessageBox.Show("Your log will not be saved. Are you sure you would like to go back to the dashboard?",
                            "You Sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;
                    }
                }

                _mainWindowFrame.GoBack();

                if (contextBeforeBack == NavContext.EDITOR)
                {
                    if (_editorTimer != null)
                    {
                        BackFromEditor?.Invoke(_editorTimer);
                        _editorTimer = null;
                    }
                }
            }

            NavigationContext = NavigationContext switch
            {
                NavContext.POSTIT => NavContext.LOGGER,
                NavContext.LOGGER => NavContext.DASHBOARD,
                _ => NavigationContext
            };

            NavigationContext = NavigationContext switch
            {
                NavContext.EDITOR_POSTIT => NavContext.EDITOR,
                NavContext.EDITOR => NavContext.REPORTER_DASHBOARD,
                NavContext.REPORTER_DASHBOARD => NavContext.DASHBOARD,
                _ => NavigationContext
            };


            CheckBackNavigationButton(mainWindowViewModel);
        }

        public void CheckBackNavigationButton(MainWindowViewModel window)
        {

            if (window != null && _mainWindowFrame != null)
            {
                window.BackEnabled = _mainWindowFrame.CanGoBack;
            }

            if ((NavigationContext == NavContext.DASHBOARD || NavigationContext == NavContext.NOTES ||
                NavigationContext == NavContext.REPORTER_DASHBOARD) && window != null)
                window.BackEnabled = false;

            //CheckForwardNavigationButton(window);
        }

        /// <summary>
        /// Navigate forward in the main frame's history, if possible.
        /// </summary>
        public void GoForward()
        {
            if (_mainWindowFrame != null && _mainWindowFrame.CanGoForward)
            {
                if (NavigationContext == NavContext.LOGGER)
                {
                    _mainWindowFrame.GoForward();
                }
            }
        }

        public void CheckForwardNavigationButton(MainWindowViewModel window)
        {

            if (window != null && _mainWindowFrame != null)
            {
                window.ForwardEnabled = _mainWindowFrame.CanGoForward;
            }

            if ((NavigationContext == NavContext.DASHBOARD || NavigationContext == NavContext.NOTES ||
                NavigationContext == NavContext.REPORTER_DASHBOARD) && window != null)
                window.ForwardEnabled = false;
        }


        /// <summary>
        /// Set the logger's subclass frame.
        /// </summary>
        /// <param name="frame">The Frame UI instance.</param>
        public void SetLoggerFrame(Frame frame) => _loggerFrame = frame;

        /// <summary>
        /// Set the Android Studio frame.
        /// </summary>
        /// <param name="frame">The Frame UI instance.</param>
        public void SetAndroidStudioFrame(Frame? frame) => _AndroidStudioFrame = frame;

        /// <summary>
        /// Sets NavigationContext to DASHBOARD specifically.
        /// </summary>
        public void SetDashboardContext()
        {
            NavigationContext = NavContext.DASHBOARD;
        }

        /// <summary>
        /// Sets NavigationContext to REPORTER DASHBOARD specifically.
        /// </summary>
        public void SetReporterContext()
        {
            NavigationContext = NavContext.REPORTER_DASHBOARD;
        }



        public async Task NavigateToLogin(bool isSignOut = false)
        {
            try
            {
                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var loginWindow = uiFactory.CreateDLSWindow<Login>();
                _login = loginWindow;

                var page01 = uiFactory.CreatePage<loginPage01>();

                if (isSignOut)
                {
                    var dataService = _serviceProvider.GetRequiredService<IDataService>();
                    await dataService.SignOutUserAsync();
                    await ClearSessionState();
                }

                _login.frame_LOGIN.Navigate(page01);
                _login.Show();

                if (isSignOut)
                {
                    if (_mainWindow == null)
                        throw new InvalidOperationException("Mainwindow is null!");

                    _mainWindow.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToLogin: {ex.Message}");
                CloseApp();
            }
        }

        public void NavigateToSignUp()
        {
            try
            {
                var signUpWindow = _serviceProvider.GetRequiredService<SignUp>();
                signUpWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToSignUp: {ex.Message}");
            }
        }

        public async Task NavigateToMainWindow()
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                try
                {
                    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    _login?.Close();
                    _login = null;

                    // Store the main frame reference
                    SetMainFrame(mainWindow.frame_MAINWINDOW);

                    if (_mainWindowFrame == null)
                        throw new ArgumentNullException("MainWindow Frame is null in NavigateToMainWindow!");

                    await dataService.SignInUserAsync();

                    var user = authService.GetUser();
                    var userId = user.Id;
                    await _serviceProvider.GetRequiredService<StartUpService>().Initialise(userId);
                    var cachemaster = _serviceProvider.GetRequiredService<ICacheService>();

                    bool? isUpdating = null;
#if RELEASE
                    var settingsService = _serviceProvider.GetRequiredService<SettingsService>();
                    var settings = settingsService.Load(userId) ?? throw new InvalidOperationException("No settings!");

                    var velopackService = _serviceProvider.GetRequiredService<IVelopackService>();
                    var updateInfo = await velopackService.CheckForUpdatesAsync();


                    if (settings.ShowUpdatePopup)
                    {
                        if (updateInfo != null)
                        {
                            // Show Updater window
                            var updaterWindow = _serviceProvider.GetRequiredService<VelopackUpdaterWindow>();
                            updaterWindow.DataContext = ActivatorUtilities.CreateInstance<UpdaterViewModel>(_serviceProvider, updateInfo);
                            isUpdating = updaterWindow.ShowDialog();
                        }
                    }
#endif

                    if (isUpdating != true)
                    {
                        // User clicked Cancel or no update found
                        var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();

                        mainWindow.DataContext = mainWindowViewModel;
                        mainWindowViewModel.CodingChecked = true;
                        mainWindowViewModel.CodingQtChecked = true;
#if RELEASE
                        // User clicked cancel but update is available
                        if (updateInfo != null)
                            mainWindowViewModel.UpdaterButtonVisible = Visibility.Visible;
#endif

                        mainWindow.Show();
                        mainWindow.Activate();
                        mainWindow.Focus();

                        await NavigateToLogCachePage(LogContext.Qt);

                        _mainWindow = mainWindow ?? throw new ArgumentNullException("MainWindow is null in NavigateToMainWindow!");
                        CheckBackNavigationButton(mainWindowViewModel);

                        MessageBox.Show($"Click on \"Generic\" option in the menu panel please. In this alpha you will only be able to create coding logs.",
                            "Data Logger Alpha Version", MessageBoxButton.OK, MessageBoxImage.Information);

                        SetDashboardContext();
                    }

                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);

                    CloseApp();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToMainWindow: {ex.Message}");
            }
        }


#pragma warning disable CA1822 // Mark members as static
        public async Task<bool> NavigateToUpdaterWindowAsync(bool startUpdatingImmediately = false)
#pragma warning restore CA1822 // Mark members as static
        {
#if RELEASE
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    var velopackService = _serviceProvider.GetRequiredService<IVelopackService>();
                    var updateInfo = await velopackService.CheckForUpdatesAsync();

                    if (updateInfo == null)
                    {
                        MessageBox.Show("Unable to check for updates. Please try again later.", "Update Check Failed", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return false;
                    }

                    var updaterWindow = ActivatorUtilities.CreateInstance<VelopackUpdaterWindow>(_serviceProvider, startUpdatingImmediately);

                    updaterWindow.DataContext = ActivatorUtilities.CreateInstance<UpdaterViewModel>(_serviceProvider, updateInfo);

                    var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                    mainWindowViewModel.UpdaterButtonVisible = Visibility.Collapsed;

                    return updaterWindow.ShowDialog() == true;
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToUpdaterWindowAsync: {ex.Message}");
            }
#endif
            return false;
        }















        public async Task NavigateToPage(Page page, ViewModelBase viewModel, UserControl? subPage = null)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                if (viewModel is CodingLoggerViewModel codingLoggerViewModel)
                    await codingLoggerViewModel.AutoStartAsync();

                page.DataContext = viewModel;

                if (subPage != null)
                {
                    subPage.DataContext = viewModel;
                    _loggerFrame.Navigate(subPage);
                }

                _mainWindowFrame.Navigate(page);

                var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(window);
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex);
            }
        }

        public async Task NavigateToPage<TViewModel>(Page page, UserControl? subPage = null)
            where TViewModel : ViewModelBase
        {
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            await NavigateToPage(page, viewModel, subPage);
        }



        public async Task NavigateToPage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : ViewModelBase
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();


            try
            {
                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var page = uiFactory.CreatePage<TPage>();
                page.DataContext = _serviceProvider.GetRequiredService<TViewModel>();

                _mainWindowFrame.Navigate(page);
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex);
            }
        }

        /// <summary>
        /// Navigates to a <see cref="Page"/>. ViewModel must be set before calling.
        /// </summary>
        /// <param name="page">A <see cref="Page"/> to navigate to</param>
        public async Task NavigateToPage(Page page)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                _mainWindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await dataService.HandleExceptionAsync(invex);
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex);
            }
        }















        public void NavigateToLogCachePage()
        {
            try
            {
                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var logCachePage = uiFactory.CreatePage<LogCachePage, CodingQtViewModel>();

                SetDashboardContext();
            }
            catch (Exception)
            {
                CloseApp();
            }
        }

        public async Task NavigateToLogCachePage(LogContext logContext = LogContext.Coding)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    _mainWindowFrame.RemoveBackEntry();

                    SetDashboardContext();
                    LogContext = logContext;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

                    var logCachePage = uiFactory.CreatePage<LogCachePage>();

                    switch (logContext)
                    {
                        case LogContext.Qt:
                            {
                                await NavigateToPage<CodingQtViewModel>(logCachePage);
                                await _serviceProvider.GetRequiredService<CodingQtViewModel>().AutoStartAsync();

                                break;
                            }
                        case LogContext.AndroidStudio:
                            {
                                await NavigateToPage<CodingAndroidViewModel>(logCachePage);
                                await _serviceProvider.GetRequiredService<CodingAndroidViewModel>().AutoStartAsync();

                                break;
                            }
                        default:
                            {
                                await NavigateToPage<CodingViewModel>(logCachePage);
                                await _serviceProvider.GetRequiredService<CodingViewModel>().AutoStartAsync();
                                var codingViewModel = _serviceProvider.GetRequiredService<CodingViewModel>();
                                codingViewModel.CreateLogButtonEnabled = true;
                                codingViewModel.ReportButtonEnabled = true;

                                break;
                            }
                    }

                    var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                    CheckBackNavigationButton(mainWindowViewModel);

                    _mainWindowFrame.RemoveBackEntry();
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToLogCachePage: {ex.Message}");
            }
        }









        public async Task NavigateToLoggerCreator()
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    NavigationContext = NavContext.LOGGER;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                    var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                    var loggerCreatePage = uiFactory.CreatePage<LoggerCreatePage>();
                    var auth = _serviceProvider.GetRequiredService<IAuthService>();
                    var user = auth.GetUser();
                    string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;


                    SetLoggerFrame(loggerCreatePage.frame_VARIATIONS);

                    if (_loggerFrame == null)
                        throw new ArgumentNullException("Logger frame is null!");

                    if (LogContext == LogContext.AndroidStudio)
                    {
                        SetAndroidStudioFrame(loggerCreatePage.frame_ANDROIDSTUDIO);
                        if (_AndroidStudioFrame == null)
                            throw new ArgumentNullException("Android Studio frame is null!");
                    }
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        case LogContext.Qt:
                            {
                                await NavigateToPage<qtCreateViewModel>(loggerCreatePage, new coding_UserControl());
                                break;
                            }
                        case LogContext.AndroidStudio:
                            {
                                await NavigateToPage<AScodeCreateViewModel>(loggerCreatePage, new coding_UserControl());

                                var androidPage = new androidStudio_UserControl
                                {
                                    DataContext = _serviceProvider.GetRequiredService<AScodeCreateViewModel>()
                                };
                                _AndroidStudioFrame?.Navigate(androidPage);

                                break;
                            }
                        default:
                            {
                                await NavigateToPage<codeCreateViewModel>(loggerCreatePage, new coding_UserControl());
                                break;
                            }
                    }

                    var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                    CheckBackNavigationButton(window);
                }
                catch (XamlParseException xamlx)
                {
                    await dataService.HandleExceptionAsync(xamlx);
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToLoggerCreator: {ex.Message}");
            }

        }

        public async Task NavigateToLoggerEditor(LOGViewModel logViewModel, LogCacheDTO logCacheDTO, Timer timer, int timeRemaining)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    _editorTimer = timer;
                    NavigationContext = NavContext.EDITOR;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                    var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                    var loggerEditPage = uiFactory.CreatePage<LoggerEditPage>();
                    var auth = _serviceProvider.GetRequiredService<IAuthService>();
                    var user = auth.GetUser();
                    string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;

                    SetLoggerFrame(loggerEditPage.frame_VARIATIONS);

                    if (LogContext == LogContext.AndroidStudio)
                        SetAndroidStudioFrame(loggerEditPage.frame_ANDROIDSTUDIO);
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        default:
                            {

                                var codingCacheDTO = (CodingCacheDTO)logCacheDTO;
                                var codingViewModel = _serviceProvider.GetRequiredService<CodingViewModel>();
                                var codingEditor = viewModelFactory.CreateCodeEditViewModel(codingViewModel, codingCacheDTO, logViewModel,
                                    timeRemaining);

                                await NavigateToPage(loggerEditPage, codingEditor, new coding_UserControl());

                                break;
                            }
                    }

                    var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                    CheckBackNavigationButton(window);
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToLoggerCreator: {ex.Message}");
            }
        }



        public async Task NavigateToViewer(LogDTO logDTO)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    NavigationContext = NavContext.DASHBOARD;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                    var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                    var loggerViewPage = uiFactory.CreatePage<ViewerPage>();
                    var auth = _serviceProvider.GetRequiredService<AuthService>();
                    var user = Extensions.UserSettingsToDTO(auth.GetUser());
                    string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;

                    SetLoggerFrame(loggerViewPage.frame_VARIATIONS);

                    if (LogContext == LogContext.AndroidStudio)
                        SetAndroidStudioFrame(loggerViewPage.frame_ANDROIDSTUDIO);
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        // TODO: Qt and Android cases

                        default:
                            {
                                int bugs = 0;
                                bool isSuccessful = false;

                                if (logDTO is CodingCacheDTO codingCacheDTO)
                                {
                                    bugs = codingCacheDTO.Bugs;
                                    isSuccessful = codingCacheDTO.Success;
                                }
                                else if (logDTO is CodingDataDTO codingDataDTO)
                                {
                                    bugs = codingDataDTO.Bugs;
                                    isSuccessful = codingDataDTO.Success;
                                }
                                var codeViewerViewModel = viewModelFactory.CreateViewerViewModel(user, new CodingViewDTO
                                {
                                    ProjectName = logDTO.Project.ProjectName,
                                    ApplicationName = logDTO.ApplicationName,
                                    Date = logDTO.Start.ToString("d MMMM yyyy HH:mm"),
                                    OutputName = logDTO.OutputName,
                                    TypeName = logDTO.TypeName,
                                    PostIts = logDTO.PostIts,
                                    Bugs = $"{bugs} Bugs Found",
                                    ApplicationOpened = isSuccessful ? "Launch Successful" : "Unsuccessful Launch"
                                });

                                await NavigateToPage<ViewerViewModel>(loggerViewPage, new coding_UserControl_View());

                                break;
                            }
                    }

                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToViewer: {ex.Message}");
            }
        }


        public async Task NavigateToUpdater(LogDataDTO logDataDTO, ReportDeskViewModel reportDeskViewModel)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                try
                {
                    NavigationContext = NavContext.EDITOR;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                    var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                    var loggerEditPage = uiFactory.CreatePage<LoggerEditPage>();
                    var auth = _serviceProvider.GetRequiredService<AuthService>();
                    var user = auth.GetUser();
                    string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;

                    SetLoggerFrame(loggerEditPage.frame_VARIATIONS);

                    if (LogContext == LogContext.AndroidStudio)
                        SetAndroidStudioFrame(loggerEditPage.frame_ANDROIDSTUDIO);
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        // TODO: Qt and Android cases

                        default:
                            {
                                var codeReportDeskViewModel = (CodeReportDeskViewModel)reportDeskViewModel;
                                var codingDataDTO = (CodingDataDTO)logDataDTO;

                                var codingUpdater = viewModelFactory.CreateCodeUpdateViewModel(codeReportDeskViewModel, codingDataDTO);

                                await NavigateToPage(loggerEditPage, codingUpdater, new coding_UserControl());

                                break;
                            }
                    }

                    var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                    CheckBackNavigationButton(window);

                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} NavigateToUpdater: {ex.Message}");
            }
        }


        private static string FormatLogTime(int seconds)
        {
            if (seconds < 60)
            {
                string unit = seconds == 1 ? "second" : "seconds";
                return $"This logCacheDTO will be stored in {seconds} {unit}.";
            }

            int minutes = (int)Math.Round(seconds / 60.0);
            string unitMin = minutes == 1 ? "minute" : "minutes";
            return $"This logCacheDTO will be stored in {minutes} {unitMin}.";
        }













        public async Task NavigateToPostItCreator(LoggerViewModel logger)
        {
            NavigationContext = NavContext.POSTIT;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
            var postItPage = await uiFactory.CreatePostItPage(logger, logger.ApplicationName, logger.ProjectName);


            await NavigateToPage(postItPage);

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }

        public async Task NavigateToPostItEditor(LoggerViewModel logger, CacheStickyPostItViewModel cacheStickyPostItViewModel)
        {
            if (NavigationContext == NavContext.LOGGER)
                NavigationContext = NavContext.POSTIT;
            else if (NavigationContext == NavContext.EDITOR)
                NavigationContext = NavContext.EDITOR_POSTIT;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
            var postItPage = await uiFactory.CreateEditPostItPage(logger, cacheStickyPostItViewModel, logger.ApplicationName, logger.ProjectName);

            await NavigateToPage(postItPage);

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }

        public async Task NavigateToPostItUpdater(LoggerViewModel loggerViewModel, PostItDataDTO postItDataDTO)
        {
            // TODO
            NavigationContext = NavContext.UPDATER_POSTIT;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();

            //await NavigateToPage(postItPage);

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }







        #region Reporter




        public async Task NavigateToReporter()
        {
            _mainWindowFrame.RemoveBackEntry();

            SetReporterContext();


            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();


            switch (LogContext)
            {
                default:
                    {
                        var reporterDashboard = uiFactory.CreateReporterPage("Code");
                        dynamic reportDesk = reporterDashboard.DataContext;
                        await reportDesk.AutoStartAsync();

                        await NavigateToPage(reporterDashboard);

                        break;
                    }
            }

        }


        #endregion



















        public async Task NavigateToNotesDashboard()
        {
            await NavigateToPage<NOTESPage, NOTESDashboardViewModel>();
            var notesDashboard = _serviceProvider.GetRequiredService<NOTESDashboardViewModel>();
            notesDashboard.StartUpVisibilitySet(notesDashboard.NoteItems.Count > 0);

            NavigationContext = NavContext.NOTES;

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }

        public async Task NavigateToCreateNotesPage()
        {
            NavigationContext = NavContext.CREATENOTE;
            LogContext = LogContext.NOTES;

            await NavigateToPage<CreateNotePage, CreateNoteViewModel>();

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            window.GenericNotesChecked = true;

            CheckBackNavigationButton(window);
        }















        public void SetGenericNotesChecked(bool isChecked = false)
        {
            var mainwindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainwindowViewModel.SetGenericNotesChecked(isChecked);
        }

        public void SetChecklistNotesChecked(bool isChecked = false)
        {
            var mainwindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainwindowViewModel.SetChecklistNotesChecked(isChecked);
        }

        public async Task NavigateToSettings()
        {
            await NavigateToPage<SettingsPage, SettingsViewModel>();

            var mainwindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(mainwindowViewModel);
        }

        /// <summary>
        /// Clears singleton states for logCacheDTO out.
        /// </summary>
        public async Task ClearSessionState()
        {
            try
            {
                _serviceProvider.GetRequiredService<CodingViewModel>().OnLogOut();
                _serviceProvider.GetRequiredService<CodingAndroidViewModel>().OnLogOut();
                _serviceProvider.GetRequiredService<CodingQtViewModel>().OnLogOut();
                _serviceProvider.GetRequiredService<NOTESDashboardViewModel>().OnLogOut();
                await _serviceProvider.GetRequiredService<AuthService>().SignOutAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} ClearSessionState: {ex.Message}");
            }
        }
    }

}
