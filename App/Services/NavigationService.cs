using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.Components.Subcontrols_View;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Account;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Services
{
    public class NavigationService
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
        private MainWindow _mainWindow;

        // MAIN FRAME
        private Frame _mainwindowFrame;

        // LOGGER CREATOR SUBCLASS FRAME
        private Frame _loggerFrame;

        // LOGGER CREATOR ANDROID STUDIO FRAME
        private Frame? _AndroidStudioFrame;


        public NavContext NavigationContext { get; set; } = NavContext.DASHBOARD;
        public CacheContext CacheContext { get; set; } = CacheContext.Qt;







        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }






        /// <summary>
        /// Set the MainWindow's frame to _mainwindowFrame.
        /// </summary>
        /// <param name="frame">The frame inside MainWindow window.</param>
        public void SetMainFrame(Frame frame) => _mainwindowFrame = frame;

        /// <summary>
        /// Navigate back in the main frame's history, if possible.
        /// </summary>
        /// <param name="isPost"></param>
        public void GoBack(bool isPost)
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();

            if (_mainwindowFrame != null && _mainwindowFrame.CanGoBack)
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
                            "Your log will not be saved. Are you sure you would like to go back to the dashboard?",
                            "You Sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;
                    }
                }

                _mainwindowFrame.GoBack();
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

            if (window != null && _mainwindowFrame != null)
            {
                window.BackEnabled = _mainwindowFrame.CanGoBack;
            }

            if (NavigationContext == NavContext.DASHBOARD || NavigationContext == NavContext.NOTES ||
                NavigationContext == NavContext.REPORTER_DASHBOARD)
                window.BackEnabled = false;

            //CheckForwardNavigationButton(window);
        }

        /// <summary>
        /// Navigate forward in the main frame's history, if possible.
        /// </summary>
        public void GoForward()
        {
            if (_mainwindowFrame != null && _mainwindowFrame.CanGoForward)
            {
                if (NavigationContext == NavContext.LOGGER)
                {
                    _mainwindowFrame.GoForward();
                }
            }
        }

        public void CheckForwardNavigationButton(MainWindowViewModel window)
        {

            if (window != null && _mainwindowFrame != null)
            {
                window.ForwardEnabled = _mainwindowFrame.CanGoForward;
            }

            if (NavigationContext == NavContext.DASHBOARD || NavigationContext == NavContext.NOTES ||
                NavigationContext == NavContext.REPORTER_DASHBOARD)
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
            var loginWindow = _serviceProvider.GetRequiredService<Login>();
            _login = loginWindow;

            var page01 = _serviceProvider.GetRequiredService<loginPage01>();

            if (isSignOut)
            {
                var dataService = _serviceProvider.GetRequiredService<IDataService>();
                await dataService.SignOutUser();
                ClearSessionState();
            }

            _login.frame_LOGIN.Navigate(page01);
            _login.Show();

            if (isSignOut)
            {
                _mainWindow.Close();
            }
        }

        public void NavigateToSignUp()
        {
            var signUpWindow = _serviceProvider.GetRequiredService<SignUp>();
            signUpWindow.Show();
        }

        public async Task NavigateToMainWindow()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

                if(_login != null)
                {
                    _login.Close();
                    _login = null;
                }

                // Store the main frame reference
                SetMainFrame(mainWindow.frame_MAINWINDOW);


                await dataService.SignInUser();

                await _serviceProvider.GetRequiredService<InitialService>().Initialise(dataService.GetUser().accountID);

                bool? isUpdating = null;
#if RELEASE
                var settingsService = _serviceProvider.GetRequiredService<SettingsService>();
                var settings = settingsService.Load(dataService.GetUser().accountID) ?? new Settings();

                var velopackService = _serviceProvider.GetRequiredService<VelopackService>();
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
                    if(updateInfo != null)
                        mainWindowViewModel.UpdaterButtonVisible = Visibility.Visible;
#endif

                    mainWindow.Show();
                    mainWindow.Activate();
                    mainWindow.Focus();

                    await NavigateToLogCachePage(CacheContext.Qt);

                    _mainWindow = mainWindow;

                    CheckBackNavigationButton(mainWindowViewModel);

                    MessageBox.Show($"Click on \"Generic\" option in the menu panel please. In this alpha you will only be able to create coding logs.",
                        "Data Logger Alpha Version", MessageBoxButton.OK, MessageBoxImage.Information);

                    SetDashboardContext();
                }


            }
            catch (InvalidOperationException invex)
            {
                await dataService.HandleExceptionAsync(invex, "NavigateToMainWindow()", "InvalidOperationException");

                CloseApp();
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToMainWindow()");

                CloseApp();
            }
        }


        public async Task<bool> NavigateToUpdaterWindowAsync(bool startUpdatingImmediately = false)
        {
#if RELEASE
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                var velopackService = _serviceProvider.GetRequiredService<VelopackService>();
                var updateInfo = await velopackService.CheckForUpdatesAsync();

                if (updateInfo == null)
                {
                    MessageBox.Show("Unable to check for updates. Please try again later.", "Update Check Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                await dataService.HandleExceptionAsync(ex, "NavigateToUpdaterWindowAsync()");
            }
#endif
            return false;
        }















        public async Task NavigateToPage<TViewModel>(Page page, UserControl subPage = null)
            where TViewModel : ViewModelBase
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                if (NavigationContext == NavContext.LOGGER)
                {
                    if (_loggerFrame is null)
                        throw new InvalidOperationException("Logger frame not set!");

                    if (CacheContext == CacheContext.AndroidStudio && _AndroidStudioFrame is null)
                        throw new InvalidOperationException("Android Studio frame not set!");
                }

                var viewModel = _serviceProvider.GetRequiredService<TViewModel>();



                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;


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

                    case graphicCreateViewModel vm:
                        await ApplyAsync(vm);
                        break;

                    case filmCreateViewModel vm:
                        await ApplyAsync(vm);
                        break;

                    case flexiCreateViewModel vm:
                        await ApplyAsync(vm, autoStart: false);
                        break;

                    default:
                        page.DataContext = viewModel;
                        break;
                }

                if (subPage != null)
                {
                    subPage.DataContext = viewModel;
                    _loggerFrame.Navigate(subPage);
                }

                _mainwindowFrame.Navigate(page);

                var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(window);
            }
            catch (InvalidOperationException invex)
            {
                await dataService.HandleExceptionAsync(invex, "NavigateToPage<TViewModel>(page)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToPage<TViewModel>(page)");
            }
        }



        public async Task NavigateToPage(Page page, ViewModelBase viewModel, UserControl subPage = null)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();


            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                if (NavigationContext == NavContext.LOGGER || NavigationContext == NavContext.EDITOR)
                {
                    if (_loggerFrame is null)
                        throw new InvalidOperationException("Logger frame not set!");

                    if (CacheContext == CacheContext.AndroidStudio && _AndroidStudioFrame is null)
                        throw new InvalidOperationException("Android Studio frame not set!");
                }

                page.DataContext = viewModel;

                if (subPage != null)
                {
                    subPage.DataContext = viewModel;
                    _loggerFrame.Navigate(subPage);
                }
                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await dataService.HandleExceptionAsync(invex, "NavigateToPage(page, viewModel)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToPage(page, viewModel)");
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
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var page = uiFactory.CreatePage<TPage>();
                page.DataContext = _serviceProvider.GetRequiredService<TViewModel>();

                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await dataService.HandleExceptionAsync(invex, "NavigateToPage(page, viewModel)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToPage(page, viewModel)");
            }
        }

        public async Task NavigateToPage(Page page)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await dataService.HandleExceptionAsync(invex, "NavigateToPage<TPage>()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToPage<TPage>()");
            }
        }















        public void NavigateToLogCachePage()
        {
            try
            {
                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

                var logCachePage = uiFactory.CreatePage<LogCachePage>();
                logCachePage.DataContext = _serviceProvider.GetRequiredService<CodingQtViewModel>();


                if (_mainwindowFrame != null)
                    _mainwindowFrame.Navigate(logCachePage);

                SetDashboardContext();
            }
            catch (Exception ex)
            {
                CloseApp();
            }
        }

        public async Task NavigateToLogCachePage(CacheContext cacheContext)
        {
            _mainwindowFrame.RemoveBackEntry();

            SetDashboardContext();
            CacheContext = cacheContext;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

            var logCachePage = uiFactory.CreatePage<LogCachePage>();

            switch (cacheContext)
            {
                case CacheContext.Qt:
                    {
                        await NavigateToPage<CodingQtViewModel>(logCachePage);
                        await _serviceProvider.GetRequiredService<CodingQtViewModel>().AutoStartAsync();

                        break;
                    }
                case CacheContext.AndroidStudio:
                    {
                        await NavigateToPage<CodingAndroidViewModel>(logCachePage);
                        await _serviceProvider.GetRequiredService<CodingAndroidViewModel>().AutoStartAsync();

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        await NavigateToPage<GraphicsViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<GraphicsViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Film:
                    {
                        await NavigateToPage<FilmViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<FilmViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Flexi:
                    {
                        await NavigateToPage<FlexiViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<FlexiViewModel>().UpdateLogCount();

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

            _mainwindowFrame.RemoveBackEntry();
        }

        public async Task NavigateToLogCachePage<TViewModel>(CacheContext cacheContext) where TViewModel : LogCacheViewModel
        {
            await NavigateToPage<LogCachePage, TViewModel>();

            SetDashboardContext();

            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(mainWindowViewModel);
        }











        /// <summary>
        /// Handles the visibility of a display pic - usually in a logger interface.
        /// </summary>
        /// <param name="viewModel">The viewmodel housing the visibility property.</param>
        /// <param name="path">The profile picture path</param>
        private void DisplayPicVisibilityHandler(dynamic viewModel, string path = "")
        {
            if (path.Contains("/Assets/"))
            {
                viewModel.SignUpImage = null;
                viewModel.DisplayPicVisibility = Visibility.Visible;
            }
            else
                viewModel.SignUpImage = BitmapService.LoadImage(path);
        }

        /// <summary>
        /// Checks a path and sets ShowDefault to the correct visibility.
        /// </summary>
        /// <param name="viewModel">The viewmodel housing the visibility property.</param>
        /// <param name="path">The profile picture path</param>
        private void ShowDefaultHandler(dynamic viewModel, string path = "")
        {
            if (path.Contains("/Assets/"))
            {
                viewModel.SignUpImage = null;
                viewModel.ShowDefault = Visibility.Visible;
            }
            else
                viewModel.SignUpImage = BitmapService.LoadImage(path);
        }


        public async Task NavigateToLoggerCreator()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                NavigationContext = NavContext.LOGGER;

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                var loggerCreatePage = uiFactory.CreatePage<LoggerCreatePage>();
                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;


                SetLoggerFrame(loggerCreatePage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerCreatePage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    case CacheContext.Qt:
                        {
                            var qtLogger = viewModelFactory.CreateQtCodeCreateViewModel();
                            await qtLogger.AutoStartAsync(true);

                            DisplayPicVisibilityHandler(qtLogger, path);

                            await NavigateToPage(loggerCreatePage, qtLogger, new coding_UserControl());

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            await NavigateToPage<AScodeCreateViewModel>(loggerCreatePage, new coding_UserControl());

                            var androidPage = new androidStudio_UserControl();
                            androidPage.DataContext = _serviceProvider.GetRequiredService<AScodeCreateViewModel>();
                            _AndroidStudioFrame?.Navigate(androidPage);

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            await NavigateToPage<graphicCreateViewModel>(loggerCreatePage, new graphics_UserControl());

                            break;
                        }
                    case CacheContext.Film:
                        {
                            await NavigateToPage<filmCreateViewModel>(loggerCreatePage, new film_UserControl());

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            await NavigateToPage<flexiCreateViewModel>(loggerCreatePage, new flexi_UserControl());

                            break;
                        }
                    default:
                        {
                            var codingLogger = viewModelFactory.CreateCodeCreateViewModel();
                            await codingLogger.AutoStartAsync();

                            DisplayPicVisibilityHandler(codingLogger, path);

                            await NavigateToPage(loggerCreatePage, codingLogger, new coding_UserControl());

                            break;
                        }
                }

                var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(window);
            }
            catch (XamlParseException xamlx)
            {
                await dataService.HandleExceptionAsync(xamlx, "NavigateToLoggerCreator()", "XamlParseException");
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToLoggerCreator()");
            }

        }

        public async Task NavigateToLoggerEditor(ViewModelBase viewModelBase)
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
                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;

                SetLoggerFrame(loggerEditPage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerEditPage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    default:
                        {
                            var codingLOGViewModel = viewModelBase as CodeLOGViewModel;

                            var codingEditor = viewModelFactory.CreateCodeEditViewModel(codingLOGViewModel);

                            await codingEditor.AutoStartAsync();

                            DisplayPicVisibilityHandler(codingEditor, path);

                            var log = codingLOGViewModel._CodeLOG;

                            codingEditor.ProjectName = log.Project.Name;
                            codingEditor.ApplicationName = log.Application.Name;

                            codingEditor.StartDate = log.Start;
                            codingEditor.StartHours = log.Start.Hour;
                            codingEditor.StartMinutes = log.Start.Minute;
                            codingEditor.StartSeconds = log.Start.Second;
                            codingEditor.StartMilliseconds = log.Start.Millisecond;

                            codingEditor.EndDate = log.End;
                            codingEditor.EndHours = log.End.Hour;
                            codingEditor.EndMinutes = log.End.Minute;
                            codingEditor.EndSeconds = log.End.Second;
                            codingEditor.EndMilliseconds = log.End.Millisecond;

                            codingEditor.Output = log.Output.Name;
                            codingEditor.Type = log.Type.Name;


                            foreach (var postIt in log.PostItList)
                            {
                                var project = postIt.Subject.Project;

                                var editPostIt = new EditPostItViewModel(this, dataService, codingEditor, log.Project,
                                    new(this, dataService, codingEditor, project,
                                        postIt.Subject.Subject, postIt.Error, postIt.ERCaptureTime, postIt.Solution,
                                        postIt.SOCaptureTime, postIt.Suggestion, postIt.Comment));
                                await editPostIt.AutoStartAsync(project);

                                codingEditor.PostIts.Add(editPostIt);
                            }

                            codingEditor.BugsFound = log.Bugs;
                            codingEditor.ApplicationOpened = log.Success;


                            await NavigateToPage(loggerEditPage, codingEditor, new coding_UserControl());

                            break;
                        }
                }

                var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(window);
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToLoggerEditor()");
            }
        }

        public async Task NavigateToViewer(ViewModelBase viewModelBase)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                NavigationContext = NavContext.DASHBOARD;

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var loggerViewPage = uiFactory.CreatePage<LoggerViewPage>();
                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;

                SetLoggerFrame(loggerViewPage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerViewPage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    default:
                        {
                            var codingLOGViewModel = viewModelBase as CodeLOGViewModel;

                            var codeViewerViewModel = _serviceProvider.GetRequiredService<codeViewerViewModel>();

                            DisplayPicVisibilityHandler(codeViewerViewModel, path);

                            var log = codingLOGViewModel?._CodeLOG;

                            if (log == null)
                                throw new ArgumentNullException("Log cannot be null");

                            codeViewerViewModel.ProjectName = log.Project.Name;
                            codeViewerViewModel.ApplicationName = log.Application.Name;
                            codeViewerViewModel.Date = log.Start.ToString("d MMMM yyyy HH:ss");

                            codeViewerViewModel.Output = log.Output.Name;
                            codeViewerViewModel.Type = log.Type.Name;

                            foreach (var postIt in log.PostItList)
                            {
                                codeViewerViewModel.AddPostIt(new PostItViewModel(this,
                                        postIt.Subject.Project, postIt.Subject.Subject, postIt.Error,
                                        postIt.Solution, postIt.Suggestion, postIt.Comment));
                            }

                            codeViewerViewModel.BugsFound = log.Bugs == 1 ? $"{log.Bugs.ToString()} Bug Found" : $"{log.Bugs.ToString()} Bugs Found";
                            codeViewerViewModel.ApplicationOpened = log.Success ? "Launch Successful" : "Unsuccessful Launch";


                            await NavigateToPage(loggerViewPage, codeViewerViewModel, new coding_UserControl_View());

                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToLoggerEditor()");
            }
        }

        public async Task NavigateToViewer(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                NavigationContext = NavContext.DASHBOARD;

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var loggerViewPage = uiFactory.CreatePage<LoggerViewPage>();
                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;

                SetLoggerFrame(loggerViewPage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerViewPage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    default:
                        {

                            var codeViewerViewModel = _serviceProvider.GetRequiredService<codeViewerViewModel>();
                            codeViewerViewModel.OKCommand = new ViewerOKCommand(this, ViewType.Log);

                            DisplayPicVisibilityHandler(codeViewerViewModel, path);

                            var codingLOG = (CodingLOG)log;

                            codeViewerViewModel.ProjectName = codingLOG.Project.Name;
                            codeViewerViewModel.ApplicationName = codingLOG.Application.Name;
                            codeViewerViewModel.Date = codingLOG.Start.ToString("d MMMM yyyy HH:mm");

                            codeViewerViewModel.Output = codingLOG.Output.Name;
                            codeViewerViewModel.Type = codingLOG.Type.Name;

                            foreach (var postIt in codingLOG.PostItList)
                            {
                                codeViewerViewModel.AddPostIt(new PostItViewModel(this,
                                        postIt.Subject.Project, postIt.Subject.Subject, postIt.Error,
                                        postIt.Solution, postIt.Suggestion, postIt.Comment));
                            }

                            codeViewerViewModel.BugsFound = codingLOG.Bugs == 1 ? $"{codingLOG.Bugs.ToString()} Bug Found" : $"{codingLOG.Bugs.ToString()} Bugs Found";
                            codeViewerViewModel.ApplicationOpened = codingLOG.Success ? "Launch Successful" : "Unsuccessful Launch";


                            await NavigateToPage(loggerViewPage, codeViewerViewModel, new coding_UserControl_View());

                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToLoggerEditor()");
            }
        }

        public async Task NavigateToLogViewer(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {
                NavigationContext = NavContext.EDITOR;

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var loggerViewPage = uiFactory.CreatePage<LoggerViewPage>();
                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;

                SetLoggerFrame(loggerViewPage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerViewPage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    default:
                        {
                            var codingLOG = (CodingLOG)log;

                            var codeViewerViewModel = _serviceProvider.GetRequiredService<codeViewerViewModel>();

                            DisplayPicVisibilityHandler(codeViewerViewModel, path);


                            codeViewerViewModel.ProjectName = codingLOG.Project.Name;
                            codeViewerViewModel.ApplicationName = codingLOG.Application.Name;
                            codeViewerViewModel.Date = codingLOG.Start.ToString("d MMMM yyyy HH:mm");

                            codeViewerViewModel.Output = codingLOG.Output.Name;
                            codeViewerViewModel.Type = codingLOG.Type.Name;

                            foreach (var postIt in codingLOG.PostItList)
                            {
                                codeViewerViewModel.AddPostIt(new PostItViewModel(this,
                                        postIt.Subject.Project, postIt.Subject.Subject, postIt.Error,
                                        postIt.Solution, postIt.Suggestion, postIt.Comment));
                            }

                            codeViewerViewModel.BugsFound = codingLOG.Bugs == 1 ? $"{codingLOG.Bugs.ToString()} Bug Found" : $"{codingLOG.Bugs.ToString()} Bugs Found";
                            codeViewerViewModel.ApplicationOpened = codingLOG.Success ? "Launch Successful" : "Unsuccessful Launch";


                            await NavigateToPage(loggerViewPage, codeViewerViewModel, new coding_UserControl_View());

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "NavigateToLoggerEditor()");
            }
        }

        public async Task NavigateToUpdater(LOG log, ReportDeskViewModel reportDeskViewModel)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            try
            {
                NavigationContext = NavContext.EDITOR;

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                var loggerEditPage = uiFactory.CreatePage<LoggerEditPage>();
                var auth = _serviceProvider.GetRequiredService<AuthService>();
                string path = string.IsNullOrWhiteSpace(auth.Account?.ProfilePic) ? "/Assets/login/user.png" : auth.Account.ProfilePic;

                SetLoggerFrame(loggerEditPage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerEditPage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    default:
                        {

                            var codingUpdater = viewModelFactory.CreateCodeUpdateViewModel(
                                (CodeReportDeskViewModel)reportDeskViewModel,
                                log);

                            await codingUpdater.AutoStartAsync();
                            DisplayPicVisibilityHandler(codingUpdater, path);


                            codingUpdater.ProjectName = log.Project.Name;
                            codingUpdater.ApplicationName = log.Application.Name;

                            codingUpdater.StartDate = log.Start;
                            codingUpdater.StartHours = log.Start.Hour;
                            codingUpdater.StartMinutes = log.Start.Minute;
                            codingUpdater.StartSeconds = log.Start.Second;
                            codingUpdater.StartMilliseconds = log.Start.Millisecond;

                            codingUpdater.EndDate = log.End;
                            codingUpdater.EndHours = log.End.Hour;
                            codingUpdater.EndMinutes = log.End.Minute;
                            codingUpdater.EndSeconds = log.End.Second;
                            codingUpdater.EndMilliseconds = log.End.Millisecond;

                            codingUpdater.Output = log.Output.Name;
                            codingUpdater.Type = log.Type.Name;

                            var dataService = _serviceProvider.GetRequiredService<IDataService>();

                            foreach (var postIt in log.PostItList)
                            {
                                var project = log.Project;
                                var editPostIt = new EF_EditPostItViewModel(postIt.postItID, this, dataService, codingUpdater,
                                    project,
                                    new(postIt.postItID, this, dataService, codingUpdater, project,
                                        postIt.Subject.Subject, postIt.Error, postIt.ERCaptureTime, postIt.Solution,
                                        postIt.SOCaptureTime, postIt.Suggestion, postIt.Comment));
                                await editPostIt.AutoStartAsync(project);

                                codingUpdater.PostIts.Add(editPostIt);
                            }

                            var codingLog = (CodingLOG)log;
                            codingUpdater.BugsFound = codingLog.Bugs;
                            codingUpdater.ApplicationOpened = codingLog.Success;


                            await NavigateToPage(loggerEditPage, codingUpdater, new coding_UserControl());

                            break;
                        }
                }

                var window = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(window);

            }
            catch (Exception ex)
            {

            }
        }


        private string FormatLogTime(int seconds)
        {
            if (seconds < 60)
            {
                string unit = seconds == 1 ? "second" : "seconds";
                return $"This log will be stored in {seconds} {unit}.";
            }

            int minutes = (int)Math.Round(seconds / 60.0);
            string unitMin = minutes == 1 ? "minute" : "minutes";
            return $"This log will be stored in {minutes} {unitMin}.";
        }













        public async Task NavigateToPostItCreator(LoggerCreateViewModel logger)
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
            if (createViewModel is LoggerCreateViewModel)
            {
                var logger = createViewModel as LoggerCreateViewModel;
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
            _mainwindowFrame.RemoveBackEntry();

            SetReporterContext();


            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();


            switch (CacheContext)
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
            CacheContext = CacheContext.NOTES;

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
        /// Clears singleton states for log out.
        /// </summary>
        public void ClearSessionState()
        {
            _serviceProvider.GetRequiredService<CodingViewModel>().OnLogOut();
            _serviceProvider.GetRequiredService<NOTESViewModel>().OnLogOut();
            _serviceProvider.GetRequiredService<AuthService>().SignOut();
        }
    }

}
