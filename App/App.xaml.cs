using Core.Services;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Services.CommandLogic;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Account;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Data_Logger_1._3.Views.ReportPages;
#if DEBUG
using DotNetEnv;
#endif
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
#if RELEASE
using System.Security.Cryptography;
using System.Text;
#endif
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private readonly IServiceProvider _serviceProvider;
        public static IConfiguration? Configuration { get; private set; }

        public App()
        {
#if DEBUG
            Env.Load();
#endif

            SQLitePCL.Batteries_V2.Init();
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
#if DEBUG
                    config.AddJsonFile($"appsettings.DevMode.json", optional: true);
#else
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
#endif
                })
                .ConfigureServices((context, service) =>
                {
                    service.AddSingleton<UIFactory>(services => new UIFactory(services));
                    service.AddTransient((services) => new InitialService(services));
                    service.AddTransient<PDFService>();
                    service.AddTransient<ViewModelFactory>(services => new ViewModelFactory(services));


                    string key = string.Empty;
#if DEBUG
                    key = Environment.GetEnvironmentVariable("DevMode_SQLCipher_Key")!;
#else
                    key = RetrieveEncryptionKey();
#endif

                    if (string.IsNullOrWhiteSpace(key))
                        throw new Exception("SQLCipher key is missing!");


                    var connectionString = new SqliteConnectionStringBuilder
                    {
                        DataSource = context.Configuration.GetConnectionString("DefaultConnection"),
                        Mode = SqliteOpenMode.ReadWriteCreate,
                        Password = key
                    }.ToString();

                    service.AddDbContext<EntityMaster>(options =>
                    {
                        options.UseSqlite(connectionString)
                        .LogTo(Console.WriteLine, LogLevel.Information);
#if DEBUG
                        options.EnableSensitiveDataLogging();
#endif
                    });

                    service.AddSingleton<CacheMaster>();
                    service.AddScoped((services) => new EntityReader(services));
                    service.AddScoped((services) => new EntityWriter(services));
                    service.AddScoped((services) => new EntityHandler(services));

                    service.AddSingleton((services) => new AuthService(services));

                    service.AddSingleton<IDataService>(services => new DataService(services.GetRequiredService<CacheMaster>(),
                        services.GetRequiredService<AuthService>(), services));
                    service.AddTransient<BitmapService>();
                    service.AddTransient(services => new AppSettingsService(services.GetRequiredService<IConfiguration>()));
                    service.AddTransient<SettingsService>();
                    service.AddTransient<PasswordResetService>();
                    service.AddTransient<VelopackService>();


                    service.AddSingleton((services) => new NavigationService(services));


                    service.AddTransient<Splashscreen>();
                    service.AddTransient<LogCachePage>();
                    service.AddTransient<LoggerCreatePage>();
                    service.AddTransient<PostItPage>();
                    service.AddTransient<NOTESPage>();
                    service.AddTransient<CreateNotePage>();
                    service.AddTransient<CreateCheckListPage>();
                    service.AddTransient<LoggerEditPage>();
                    service.AddTransient<LoggerViewPage>();
                    service.AddTransient<SettingsPage>();
                    service.AddTransient<VelopackUpdaterWindow>();


                    service.AddTransient<ReporterEditPage>();

                    service.AddTransient((services) => new LoginViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>(),
                        services.GetRequiredService<UIFactory>()));
                    service.AddTransient((services) => new ForgotPasswordViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>()));
                    service.AddTransient((services) => new SignUpViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>(),
                        services.GetRequiredService<UIFactory>()));
                    service.AddSingleton((services) => new MainWindowViewModel(services.GetRequiredService<NavigationService>(),
                        services.GetRequiredService<IDataService>()));

                    service.AddSingleton((services) => new CodingQtViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>()));
                    service.AddSingleton((services) => new CodingAndroidViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>()));
                    service.AddSingleton((services) => new CodingViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>()));
                    service.AddSingleton((services) => new GraphicsViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>()));
                    service.AddSingleton((services) => new FilmViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>()));
                    service.AddSingleton((services) => new FlexiViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>()));
                    service.AddSingleton((services) => new NOTESViewModel(services.GetRequiredService<NavigationService>()));

                    service.AddSingleton((services) => new QtReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new ASReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new CodeReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new GraphicsReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new FilmReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new FlexiReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<PDFService>()));

                    // Register a factory that returns ReporterDashboard given a key
                    service.AddTransient<Func<string, ReporterDashboard>>(sp => key =>
                    {
                        ReportDeskViewModel vm = key switch
                        {
                            "Code" => sp.GetRequiredService<CodeReportDeskViewModel>(),
                            "Android" => sp.GetRequiredService<ASReportDeskViewModel>(),
                            "Qt" => sp.GetRequiredService<QtReportDeskViewModel>(),
                            "Graphics" => sp.GetRequiredService<GraphicsReportDeskViewModel>(),
                            "Film" => sp.GetRequiredService<FilmReportDeskViewModel>(),
                            "Flexi" => sp.GetRequiredService<FlexiReportDeskViewModel>(),
                            _ => throw new ArgumentException($"Unknown report type '{key}'")
                        };

                        return new ReporterDashboard(vm);
                    });


                    // LOGGER


                    service.AddTransient<codeCreateViewModel>();
                    service.AddTransient((services) => new AScodeCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<CodingAndroidViewModel>(),
                        services.GetRequiredService<IDataService>()));
                    service.AddTransient((services) => new graphicCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<GraphicsViewModel>(),
                        services.GetRequiredService<IDataService>()));
                    service.AddTransient((services) => new filmCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<FilmViewModel>(),
                        services.GetRequiredService<IDataService>()));
                    service.AddTransient((services) => new flexiCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<FlexiViewModel>(),
                        services.GetRequiredService<IDataService>()));

                    service.AddTransient<codeEditViewModel>();
                    service.AddTransient((services) => new codeViewerViewModel(services.GetRequiredService<NavigationService>()));


                    // REPORTER


                    service.AddTransient<PostItViewModel>();
                    service.AddTransient((services) => new CreateNoteViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<NOTESViewModel>()));
                    service.AddTransient((services) => new CreateCheckListViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<IDataService>(),
                        services.GetRequiredService<NOTESViewModel>(), services.GetRequiredService<ViewModelFactory>()));
                    service.AddSingleton<CreateChecklistItemViewModel>();

                    service.AddTransient<Login>();
                    service.AddTransient((services) => new loginPage01(services.GetRequiredService<LoginViewModel>()));
                    service.AddTransient<loginPage02>();
                    service.AddTransient((services) => new SignUp(services.GetRequiredService<NavigationService>(), services.GetRequiredService<SignUpViewModel>()));
                    service.AddTransient((services) => new MainWindow(services.GetRequiredService<MainWindowViewModel>(), services.GetRequiredService<NavigationService>()));


                    // Account
                    service.AddTransient((services) => new SettingsViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<AuthService>(),
                        services.GetRequiredService<IDataService>(), services.GetRequiredService<SettingsService>(), services.GetRequiredService<MainWindowViewModel>(),
                        services.GetRequiredService<PasswordResetService>()));
                    service.AddTransient<UpdaterViewModel>();

                })
                .Build();

            _serviceProvider = host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var cachemaster = _serviceProvider.GetRequiredService<CacheMaster>();




            var splash = _serviceProvider.GetRequiredService<Splashscreen>();

            splash.Show();


            await AnimateProgressBar(splash.progressBar_splashscreen, 33, splash.text_progress);


            var navigationService = _serviceProvider.GetRequiredService<NavigationService>();

            await AnimateProgressBar(splash.progressBar_splashscreen, 66, splash.text_progress);

            using (var scope = _serviceProvider.CreateScope())
            {
                await AnimateProgressBar(splash.progressBar_splashscreen, 75, splash.text_progress);

                try
                {
                    var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

                    await AnimateProgressBar(splash.progressBar_splashscreen, 80, splash.text_progress);

                    // Ensure DB exists
                    await master.Database.MigrateAsync();

                    var connection = master.Database.GetDbConnection();
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        // Enable WAL Mode
                        command.CommandText = "PRAGMA journal_mode=WAL;";
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        File.WriteAllText("error.txt", $"An error occurred in OnStartUp: {ex.ToString()}");
                    }
                    catch
                    {
                        MessageBox.Show("An error occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    }
                }
            }


            await AnimateProgressBar(splash.progressBar_splashscreen, 100, splash.text_progress);

            await navigationService.NavigateToLogin();

            splash.Close();




            base.OnStartup(e);
        }












#if RELEASE
        private string RetrieveEncryptionKey()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");
            string keyFilePath = Path.Combine(folder, "secret.bin");

            try
            {
                if (!File.Exists(keyFilePath))
                {
                    var newKey = Guid.NewGuid().ToString("N");

                    var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(newKey), null, DataProtectionScope.LocalMachine);

                    File.WriteAllBytes(keyFilePath, encrypted);
                    return newKey;
                }

                var protectedData = File.ReadAllBytes(keyFilePath);
                var decryptedBytes = ProtectedData.Unprotect(protectedData, null, DataProtectionScope.LocalMachine);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred. We apologize for the inconvenience and cannot run the application at this time.", "Fatal Startup Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }
        }
#endif



        private async Task AnimateProgressBar(ProgressBar progressBar, double targetValue, TextBlock textBlock)
        {
            var duration = TimeSpan.FromSeconds(1);
            var animation = new DoubleAnimation(targetValue, duration);
            var textAnimation = new DoubleAnimation(targetValue, duration);

            var tcs = new TaskCompletionSource<bool>();

            animation.Completed += (s, e) => tcs.SetResult(true);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);

            var currentValue = progressBar.Value;
            var stepValue = (targetValue - currentValue) / (duration.TotalMilliseconds / 100);

            for (var i = currentValue; i <= targetValue; i += stepValue)
            {
                textBlock.Text = $"{(int)i}%";
                await Task.Delay(100);
            }

            await tcs.Task;
        }








        public static void LogException(Exception ex, [CallerMemberName] string methodName = "")
        {
            Debug.WriteLine($"Exception occurred in {methodName}(): {ex.Message}");
        }


        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                await dataService.SignOutUser();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Exception occurred in OnExit");
            }

            base.OnExit(e);
        }


    }

}
