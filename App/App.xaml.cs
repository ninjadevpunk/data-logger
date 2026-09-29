using Core;
using Core.Services;
using Data;
using Data.Interfaces;
using Data.Services;
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
using FileSys;
using FileSys.Interfaces;

#if DEBUG
using DotNetEnv;
#endif
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
#if RELEASE
using System.Security.Cryptography;
using System.Text;
#endif
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.LogViewModels;

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
                    service.AddSingleton<UIFactory>();
                    service.AddTransient<StartUpService>();
                    service.AddTransient<PDFService>();
                    service.AddTransient<ViewModelFactory>();


                    service.AddFileSysDependencies();
                    service.AddCoreDependencies();
                    service.AddDataDependencies(context.Configuration);

                    service.AddTransient<BitmapService>();
                    service.AddTransient(services => new AppSettingsService(services.GetRequiredService<IConfiguration>()));
                    service.AddTransient<SettingsService>();
                    service.AddTransient<PasswordResetService>();
                    service.AddTransient<VelopackService>();


                    service.AddSingleton<NavigationService>();


                    service.AddTransient<Splashscreen>();
                    service.AddTransient<LogCachePage>();
                    service.AddTransient<LoggerCreatePage>();
                    service.AddTransient<PostItPage>();
                    service.AddTransient<NOTESPage>();
                    service.AddTransient<CreateNotePage>();
                    service.AddTransient<CreateCheckListPage>();
                    service.AddTransient<LoggerEditPage>();
                    service.AddTransient<ViewerPage>();
                    service.AddTransient<SettingsPage>();
                    service.AddTransient<VelopackUpdaterWindow>();


                    service.AddTransient<ReporterEditPage>();

                    service.AddTransient<LoginViewModel>();
                    service.AddTransient<ForgotPasswordViewModel>();
                    service.AddTransient<SignUpViewModel>();
                    service.AddSingleton<MainWindowViewModel>();

                    service.AddTransient<LOGViewModelFactory>();
                    service.AddSingleton<CodingQtViewModel>();
                    service.AddSingleton<CodingAndroidViewModel>();
                    service.AddSingleton<CodingViewModel>();
                    service.AddSingleton<NOTESViewModel>();

                    service.AddSingleton<QtReportDeskViewModel>();
                    service.AddSingleton<ASReportDeskViewModel>();
                    service.AddSingleton<CodeReportDeskViewModel>();

                    // Register a factory that returns ReporterDashboard given a key
                    service.AddTransient<Func<string, ReporterDashboard>>(sp => key =>
                    {
                        ReportDeskViewModel vm = key switch
                        {
                            "Code" => sp.GetRequiredService<CodeReportDeskViewModel>(),
                            "Android" => sp.GetRequiredService<ASReportDeskViewModel>(),
                            "Qt" => sp.GetRequiredService<QtReportDeskViewModel>(),
                            _ => throw new ArgumentException($"Unknown report type '{key}'")
                        };

                        return new ReporterDashboard(vm);
                    });


                    // LOGGER


                    service.AddTransient<codeCreateViewModel>();
                    service.AddTransient<AScodeCreateViewModel>();

                    service.AddTransient<codeEditViewModel>();
                    service.AddTransient<ViewViewModelFactory>();


                    // REPORTER


                    service.AddTransient<PostItViewModel>();
                    service.AddTransient<CacheStickyPostItViewModel>();
                    service.AddTransient<DataStickyPostItViewModel>();
                    service.AddTransient<CreateNoteViewModel>();
                    service.AddTransient<CreateCheckListViewModel>();
                    service.AddSingleton<CreateChecklistItemViewModel>();

                    service.AddTransient<Login>();
                    service.AddTransient<loginPage01>();
                    service.AddTransient<loginPage02>();
                    service.AddTransient<SignUp>();
                    service.AddTransient<MainWindow>();


                    // Account
                    service.AddTransient<SettingsViewModel>();
                    service.AddTransient<UpdaterViewModel>();

                })
                .Build();

            _serviceProvider = host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var cachemaster = _serviceProvider.GetRequiredService<ICacheService>();




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




        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                await dataService.SignOutUserAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, $"Exception occurred in OnExit: {ex.Message}");
            }

            base.OnExit(e);
        }


    }

}
