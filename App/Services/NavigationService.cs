using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Viewer;
using Core.Enums;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data.Services;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.Components.Subcontrols_View;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
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
        public void CloseApp() => Application.Current.Shutdown(1);

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
        private MainWindow? _mainWindow;

        // MAIN FRAME
        private Frame? _mainWindowFrame;

        // LOGGER CREATOR SUBCLASS FRAME
        private Frame? _loggerFrame;

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
        /// Navigate back in the main frame's history, if possible.
        /// </summary>
        /// <param name="isPost"></param>
        public void GoBack(bool isPost)
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();

            if (_mainWindowFrame != null && _mainWindowFrame.CanGoBack)
            {
                if (!isPost)
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
                        var result = MessageBox.Show(
                            "Your logCacheDTO will not be saved. Are you sure you would like to go back to the dashboard?",
                            "You Sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;
                    }
                }

                _mainWindowFrame.GoBack();
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
                var loginWindow = _serviceProvider.GetRequiredService<Login>();
                _login = loginWindow;

                var page01 = _serviceProvider.GetRequiredService<loginPage01>();

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
                Debug.WriteLine($"Exception occurred in NavigateToLogin: {ex.Message}");
            }
        }

        public void NavigateToSignUp()
        {
            var signUpWindow = _serviceProvider.GetRequiredService<SignUp>();
            signUpWindow.Show();
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

                    if (_login != null)
                    {
                        _login.Close();
                        _login = null;
                    }

                    // Store the main frame reference
                    SetMainFrame(mainWindow.frame_MAINWINDOW);


                    await dataService.SignInUserAsync();

                    var user = authService.GetUser() ?? throw new InvalidOperationException("User cannot be null!");
                    var userId = user.UserId;
                    await _serviceProvider.GetRequiredService<StartUpService>().Initialise(userId);
                    var cachemaster = _serviceProvider.GetRequiredService<ICacheService>();
                    cachemaster.CreateSettingsFile(userId);

                    bool? isUpdating = null;
#if RELEASE
                    var settingsService = _serviceProvider.GetRequiredService<SettingsService>();
                    var settings = settingsService.Load(user.accountID) ?? throw new InvalidOperationException("No settings!");

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
                        if (updateInfo != null)
                            mainWindowViewModel.UpdaterButtonVisible = Visibility.Visible;
#endif

                        mainWindow.Show();
                        mainWindow.Activate();
                        mainWindow.Focus();

                        await NavigateToLogCachePage(LogContext.Qt);

                        _mainWindow = mainWindow;

                        CheckBackNavigationButton(mainWindowViewModel);

                        MessageBox.Show($"Click on \"Generic\" option in the menu panel please. In this alpha you will only be able to create coding logs.",
                            "Data Logger Alpha Version", MessageBoxButton.OK, MessageBoxImage.Information);

                        SetDashboardContext();
                    }


                }
                catch (InvalidOperationException invex)
                {
                    await dataService.HandleExceptionAsync(invex);

                    CloseApp();
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);

                    CloseApp();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in NavigateToMainWindow: {ex.Message}");
            }
        }


        public async Task<bool> NavigateToUpdaterWindowAsync(bool startUpdatingImmediately = false)
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
                Debug.WriteLine($"Exception occurred in NavigateToUpdaterWindowAsync: {ex.Message}");
            }
#endif
            return false;
        }















        public async Task NavigateToPage<TViewModel>(Page page, UserControl? subPage = null)
            where TViewModel : ViewModelBase
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                if (_mainWindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                if (NavigationContext == NavContext.LOGGER)
                {
                    if (_loggerFrame is null)
                        throw new InvalidOperationException("Logger frame not set!");

                    if (LogContext == LogContext.AndroidStudio && _AndroidStudioFrame is null)
                        throw new InvalidOperationException("Android Studio frame not set!");
                }

                var viewModel = _serviceProvider.GetRequiredService<TViewModel>();



                var auth = _serviceProvider.GetRequiredService<IAuthService>();
                var user = auth.GetUser();
                string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;


                async Task ApplyAsync(dynamic vm, bool autoStart = true)
                {
                    vm.SignUpImage = vm.SignUpImage ?? BitmapService.LoadImage(path);
                    DisplayPicVisibilityHandler(vm, path);

                    if (autoStart)
                        await vm.AutoStartAsync();

                    page.DataContext = vm;
                }

                switch (viewModel)
                {
                    case AScodeCreateViewModel vm:
                        await ApplyAsync(vm);
                        break;

                    default:
                        page.DataContext = viewModel;
                        break;
                }

                if (subPage != null)
                {
                    subPage.DataContext = viewModel;

                    if (_loggerFrame == null)
                        throw new InvalidOperationException("Logger frame is null!");
                    _loggerFrame.Navigate(subPage);
                }

                _mainWindowFrame.Navigate(page);

                var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(window);
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



        public async Task NavigateToPage(Page page, ViewModelBase viewModel, UserControl? subPage = null)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();


            try
            {
                if (_mainWindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                if (NavigationContext == NavContext.LOGGER || NavigationContext == NavContext.EDITOR)
                {
                    if (_loggerFrame is null)
                        throw new InvalidOperationException("Logger frame not set!");

                    if (LogContext == LogContext.AndroidStudio && _AndroidStudioFrame is null)
                        throw new InvalidOperationException("Android Studio frame not set!");
                }

                page.DataContext = viewModel;

                if (subPage != null)
                {
                    subPage.DataContext = viewModel;

                    if (_loggerFrame == null)
                        throw new InvalidOperationException("Logger frame is null!");

                    _loggerFrame.Navigate(subPage);
                }
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

        public async Task NavigateToPage<TPage, TViewModel>()
            where TPage : Page, new()
            where TViewModel : ViewModelBase
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();


            try
            {
                if (_mainWindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var page = uiFactory.CreatePage<TPage>();
                page.DataContext = _serviceProvider.GetRequiredService<TViewModel>();

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

        public async Task NavigateToPage(Page page)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                if (_mainWindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

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

                var logCachePage = uiFactory.CreatePage<LogCachePage>();
                logCachePage.DataContext = _serviceProvider.GetRequiredService<CodingQtViewModel>();


                if (_mainWindowFrame != null)
                    _mainWindowFrame.Navigate(logCachePage);

                SetDashboardContext();
            }
            catch (Exception)
            {
                CloseApp();
            }
        }

        public async Task NavigateToLogCachePage(LogContext logContext)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    if (_mainWindowFrame is null)
                        throw new InvalidOperationException("Main frame not set!");

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
                                _serviceProvider.GetRequiredService<CodingViewModel>().CreateLogButtonEnabled = true;
                                _serviceProvider.GetRequiredService<CodingViewModel>().ReportButtonEnabled = true;

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
                Debug.WriteLine($"Exception occurred in NavigateToLogCachePage: {ex.Message}");
            }
        }

        public async Task NavigateToLogCachePage<TViewModel>(LogContext logContext) where TViewModel : LogCacheViewModel
        {
            try
            {
                await NavigateToPage<LogCachePage, TViewModel>();

                SetDashboardContext();

                var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(mainWindowViewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in NavigateToLogCachePage: {ex.Message}");
            }
        }











        /// <summary>
        /// Handles the visibility of a display pic - usually in a logger interface.
        /// </summary>
        /// <param name="viewModel">The viewmodel housing the visibility property.</param>
        /// <param name="path">The profile picture path</param>
        private async Task DisplayPicVisibilityHandler(dynamic viewModel, string path = "")
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    if (path.Contains("/Assets/"))
                    {
                        viewModel.SignUpImage = null;
                        viewModel.DisplayPicVisibility = Visibility.Visible;
                    }
                    else
                        viewModel.SignUpImage = BitmapService.LoadImage(path);
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in DisplayPicVisibilityHandler: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks a path and sets ShowDefault to the correct visibility.
        /// </summary>
        /// <param name="viewModel">The viewmodel housing the visibility property.</param>
        /// <param name="path">The profile picture path</param>
        private async Task ShowDefaultHandler(dynamic viewModel, string path = "")
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    if (path.Contains("/Assets/"))
                    {
                        viewModel.SignUpImage = null;
                        viewModel.ShowDefault = Visibility.Visible;
                    }
                    else
                        viewModel.SignUpImage = BitmapService.LoadImage(path);
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in ShowDefaultHandler: {ex.Message}");
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

                    if (LogContext == LogContext.AndroidStudio)
                        SetAndroidStudioFrame(loggerCreatePage.frame_ANDROIDSTUDIO);
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        case LogContext.Qt:
                            {
                                var qtLogger = viewModelFactory.CreateQtCodeCreateViewModel();
                                await qtLogger.AutoStartAsync(true);

                                await DisplayPicVisibilityHandler(qtLogger, path);

                                await NavigateToPage(loggerCreatePage, qtLogger, new coding_UserControl());

                                break;
                            }
                        case LogContext.AndroidStudio:
                            {
                                await NavigateToPage<AScodeCreateViewModel>(loggerCreatePage, new coding_UserControl());

                                var androidPage = new androidStudio_UserControl();
                                androidPage.DataContext = _serviceProvider.GetRequiredService<AScodeCreateViewModel>();
                                _AndroidStudioFrame?.Navigate(androidPage);

                                break;
                            }
                        default:
                            {
                                var codingLogger = viewModelFactory.CreateCodeCreateViewModel();
                                await codingLogger.AutoStartAsync();

                                await DisplayPicVisibilityHandler(codingLogger, path);

                                await NavigateToPage(loggerCreatePage, codingLogger, new coding_UserControl());

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
                Debug.WriteLine($"Exception occurred in NavigateToLoggerCreator: {ex.Message}");
            }

        }

        public async Task NavigateToLoggerEditor(ViewModelBase viewModelBase, LogCacheDTO logCacheDTO)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
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
                                var codingLOGViewModel = viewModelBase as CodeLOGViewModel;

                                var codingEditor = viewModelFactory.CreateCodeEditViewModel(codingLOGViewModel ?? 
                                    throw new ArgumentNullException("codingLOGViewModel is null!"));

                                await codingEditor.AutoStartAsync();

                                await DisplayPicVisibilityHandler(codingEditor, path);

                                var codingCacheDTO = (CodingCacheDTO)logCacheDTO;

                                codingEditor.ProjectName = codingCacheDTO.ProjectName;
                                codingEditor.ApplicationName = codingCacheDTO.ApplicationName;

                                codingEditor.StartDate = codingCacheDTO.Start;
                                codingEditor.StartHours = codingCacheDTO.Start.Hour;
                                codingEditor.StartMinutes = codingCacheDTO.Start.Minute;
                                codingEditor.StartSeconds = codingCacheDTO.Start.Second;
                                codingEditor.StartMilliseconds = codingCacheDTO.Start.Millisecond;

                                codingEditor.EndDate = codingCacheDTO.End;
                                codingEditor.EndHours = codingCacheDTO.End.Hour;
                                codingEditor.EndMinutes = codingCacheDTO.End.Minute;
                                codingEditor.EndSeconds = codingCacheDTO.End.Second;
                                codingEditor.EndMilliseconds = codingCacheDTO.End.Millisecond;

                                codingEditor.Output = codingCacheDTO.OutputName;
                                codingEditor.Type = codingCacheDTO.TypeName;


                                

                                codingEditor.BugsFound = codingCacheDTO.Bugs;
                                codingEditor.ApplicationOpened = codingCacheDTO.Success;


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
                Debug.WriteLine($"Exception occurred in NavigateToLoggerCreator: {ex.Message}");
            }
        }

        

        public async Task NavigateCacheToViewer(LogCacheDTO logCacheDTO)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    NavigationContext = NavContext.DASHBOARD;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                    var loggerViewPage = uiFactory.CreatePage<ViewerPage>();
                    var auth = _serviceProvider.GetRequiredService<AuthService>();
                    var user = auth.GetUser();
                    string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;

                    SetLoggerFrame(loggerViewPage.frame_VARIATIONS);

                    if (LogContext == LogContext.AndroidStudio)
                        SetAndroidStudioFrame(loggerViewPage.frame_ANDROIDSTUDIO);
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        default:
                            {
                                var codingCacheDTO = (CodingCacheDTO)logCacheDTO;

                                var viewViewModelFactory = _serviceProvider.GetRequiredService<ViewViewModelFactory>();
                                var codeViewerViewModel = viewViewModelFactory.Create(user, new CodingViewDTO
                                {
                                    ProjectName = codingCacheDTO.ProjectName,
                                    ApplicationName = codingCacheDTO.ApplicationName,
                                    Date = codingCacheDTO.Start.ToString("d MMMM yyyy HH:mm"),
                                    OutputName = codingCacheDTO.OutputName,
                                    TypeName = codingCacheDTO.TypeName,
                                    PostIts = codingCacheDTO.PostIts,
                                    Bugs = codingCacheDTO.Bugs.ToString(),
                                    ApplicationOpened = codingCacheDTO.Success ? "Launch Successful" : "Unsuccessful Launch"
                                });

                                await DisplayPicVisibilityHandler(codeViewerViewModel, path);
                                await NavigateToPage(loggerViewPage, codeViewerViewModel, new coding_UserControl_View());

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
                Debug.WriteLine($"Exception occurred in NavigateToViewer: {ex.Message}");
            }
        }

        public async Task NavigateDataToViewer(LogDataDTO logDataDTO)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    NavigationContext = NavContext.VIEWER;

                    var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                    var loggerViewPage = uiFactory.CreatePage<ViewerPage>();
                    var auth = _serviceProvider.GetRequiredService<AuthService>();
                    var user = auth.GetUser();
                    string path = string.IsNullOrWhiteSpace(user.ProfilePic) ? Constants.Avatar : user.ProfilePic;

                    SetLoggerFrame(loggerViewPage.frame_VARIATIONS);

                    if (LogContext == LogContext.AndroidStudio)
                        SetAndroidStudioFrame(loggerViewPage.frame_ANDROIDSTUDIO);
                    else
                        SetAndroidStudioFrame(null);

                    switch (LogContext)
                    {
                        default:
                            {
                                var codingDataDTO = (CodingDataDTO)logDataDTO;

                                var viewViewModelFactory = _serviceProvider.GetRequiredService<ViewViewModelFactory>();
                                var codeViewerViewModel = viewViewModelFactory.Create(user, new CodingViewDTO
                                {
                                    ProjectName = codingDataDTO.ProjectName,
                                    ApplicationName = codingDataDTO.ApplicationName,
                                    Date = codingDataDTO.Start.ToString("d MMMM yyyy HH:mm"),
                                    OutputName = codingDataDTO.OutputName,
                                    TypeName = codingDataDTO.TypeName,
                                    PostIts = codingDataDTO.PostIts,
                                    Bugs = codingDataDTO.Bugs.ToString(),
                                    ApplicationOpened = codingDataDTO.Success ? "Launch Successful" : "Unsuccessful Launch"
                                });

                                await DisplayPicVisibilityHandler(codeViewerViewModel, path);
                                await NavigateToPage(loggerViewPage, codeViewerViewModel, new coding_UserControl_View());

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
                Debug.WriteLine($"Exception occurred in NavigateToLogViewer: {ex.Message}");
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
                        default:
                            {

                                var codingUpdater = viewModelFactory.CreateCodeUpdateViewModel(
                                    (CodeReportDeskViewModel)reportDeskViewModel, logDataDTO);

                                await codingUpdater.AutoStartAsync();
                                await DisplayPicVisibilityHandler(codingUpdater, path);

                                var codingDataDTO = (CodingDataDTO)logDataDTO;


                                codingUpdater.ProjectName = codingDataDTO.ProjectName;
                                codingUpdater.ApplicationName = codingDataDTO.ApplicationName;

                                codingUpdater.StartDate = codingDataDTO.Start;
                                codingUpdater.StartHours = codingDataDTO.Start.Hour;
                                codingUpdater.StartMinutes = codingDataDTO.Start.Minute;
                                codingUpdater.StartSeconds = codingDataDTO.Start.Second;
                                codingUpdater.StartMilliseconds = codingDataDTO.Start.Millisecond;

                                codingUpdater.EndDate = codingDataDTO.End;
                                codingUpdater.EndHours = codingDataDTO.End.Hour;
                                codingUpdater.EndMinutes = codingDataDTO.End.Minute;
                                codingUpdater.EndSeconds = codingDataDTO.End.Second;
                                codingUpdater.EndMilliseconds = codingDataDTO.End.Millisecond;

                                codingUpdater.Output = codingDataDTO.OutputName;
                                codingUpdater.Type = codingDataDTO.TypeName;

                                //foreach (var postIt in codingDataDTO.PostItList)
                                //{
                                //    var project = log.Project;
                                //    var editPostIt = new EF_EditPostItViewModel(postIt.postItID, this, dataService, codingUpdater,
                                //        project,
                                //        new(postIt.postItID, this, dataService, codingUpdater, project,
                                //            postIt.Subject.Subject, postIt.Error, postIt.ERCaptureTime, postIt.Solution,
                                //            postIt.SOCaptureTime, postIt.Suggestion, postIt.Comment));
                                //    await editPostIt.AutoStartAsync(project);

                                //    codingUpdater.PostIts.Add(editPostIt);
                                //}

                                codingUpdater.BugsFound = codingDataDTO.Bugs;
                                codingUpdater.ApplicationOpened = codingDataDTO.Success;


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
                Debug.WriteLine($"Exception occurred in NavigateToUpdater: {ex.Message}");
            }
        }


        private string FormatLogTime(int seconds)
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
            if (NavigationContext == NavContext.EDITOR)
                NavigationContext = NavContext.EDITOR_POSTIT;
            else
                NavigationContext = NavContext.POSTIT;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
            var postItPageViewModel = await viewModelFactory.CreatePostItViewModel(logger, logger.Category);
            var postItPage = uiFactory.CreatePostItPage(postItPageViewModel);

            await NavigateToPage(postItPage);

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }

        public async Task NavigateToPostItCreator(ReporterUpdaterViewModel reporterUpdaterViewModel)
        {
            NavigationContext = NavContext.EDITOR_POSTIT;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
            var efEditPostItViewModel = await viewModelFactory.Create_EF_EditPostItViewModel(reporterUpdaterViewModel, reporterUpdaterViewModel.Category);
            var postItPage = uiFactory.Create_EF_PostItPage(efEditPostItViewModel);

            await NavigateToPage(postItPage);

            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }

        public async Task NavigateToPostItCreator(ViewModelBase createViewModel, ViewModelBase stickyNotesViewModel)
        {
            if (createViewModel is LoggerViewModel)
            {
                var logger = createViewModel as LoggerViewModel;
                var postItViewModel = stickyNotesViewModel as PostItViewModel;


                var subject = postItViewModel.Subject;
                await postItViewModel.LoadSubjectsAsync(logger.Category);

                if (NavigationContext == NavContext.EDITOR)
                {
                    NavigationContext = NavContext.EDITOR_POSTIT;
                }
                else
                {
                    NavigationContext = NavContext.POSTIT;

                    var context = PostCommand.PostItContext.POSTIT;
                    postItViewModel.PostCommand = new PostCommand(PostCommand.ActionType.Edit, context, this, logger, postItViewModel);
                }

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

                postItViewModel.Subject = subject;

                if (postItViewModel.Subject != "")
                    postItViewModel.PlaceholderText = "";

                var postItPage = uiFactory.CreatePostItPage(postItViewModel);

                await NavigateToPage(postItPage);

            }
            else
            {
                var updater = createViewModel as ReporterUpdaterViewModel;
                var efPostItViewModel = stickyNotesViewModel as EF_EditPostItViewModel;

                var subject = efPostItViewModel.Subject;
                await efPostItViewModel.LoadSubjectsAsync(updater.Category);

                if (NavigationContext == NavContext.EDITOR)
                {
                    NavigationContext = NavContext.EDITOR_POSTIT;
                }
                else
                {
                    NavigationContext = NavContext.POSTIT;

                    efPostItViewModel.PostCommand = new EF_PostCommand(this, updater, efPostItViewModel);
                }

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

                efPostItViewModel.Subject = subject;

                if (efPostItViewModel.Subject != "")
                    efPostItViewModel.PlaceholderText = "";

                var postItPage = uiFactory.Create_EF_PostItPage(efPostItViewModel);

                await NavigateToPage(postItPage);
            }


            var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(window);
        }







        #region Reporter




        public async Task NavigateToReporter()
        {
            if(_mainWindowFrame != null)
                _mainWindowFrame.RemoveBackEntry();

            SetReporterContext();


            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();


            switch (LogContext)
            {
                default:
                    {
                        var reporterDashboard = uiFactory.CreateReporterPage("Code");
                        var reportDesk = _serviceProvider.GetRequiredService<CodeReportDeskViewModel>();

                        if (!reportDesk.DeskSet)
                        {
                            await reportDesk.AutoStartAsync();
                            reportDesk.DeskSet = true;
                        }

                        await NavigateToPage(reporterDashboard);

                        break;
                    }
            }

        }


        #endregion



















        public async Task NavigateToNotesDashboard()
        {
            await NavigateToPage<NOTESPage, NOTESViewModel>();
            var notesDashboard = _serviceProvider.GetRequiredService<NOTESViewModel>();
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
                _serviceProvider.GetRequiredService<NOTESViewModel>().OnLogOut();
                await _serviceProvider.GetRequiredService<AuthService>().SignOutAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in ClearSessionState: {ex.Message}");
            }
        }
    }

}
