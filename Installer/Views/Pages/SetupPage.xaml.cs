using Core.Interfaces;
using Core.Utilities;
using FileSys.Interfaces;
using FileSys.Services;
using Installer.Services;
using Microsoft.Win32;
using Migration.Services;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for SetupPage.xaml
    /// </summary>
    public partial class SetupPage : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly IFileService _fileService;
        private readonly IInstallationRegistry _installationRegistry;
        private readonly DbMigrationService _databaseMigration;

        public readonly string _programDataPath;
        private readonly string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Data Logger");
        const int APP_SIZE = 300;
        private readonly bool _isReinstall;

        public SetupPage(MainWindow mainWindow, bool isReinstall = false)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _isReinstall = isReinstall;

            _fileService = new InstallerFileService();
            _installationRegistry = new InstallationRegistry();
            _databaseMigration = new DbMigrationService();

#if DEBUG
            _programDataPath = Path.Combine(AppContext.BaseDirectory, "Data Logger");
#else
            _programDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");
#endif
            textBlock_INSTALL_LOCATION.Text = installPath;
            textBlock_APP_SIZE.Text = $"{APP_SIZE}MB";
        }







        private bool CreateDesktopShortcut()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "Data Logger.lnk");
                string exePath = Path.Combine(installPath, "Data Logger.exe");

                Type shellType = Type.GetTypeFromProgID("WScript.Shell")!;
                dynamic shell = Activator.CreateInstance(shellType)!;
                dynamic shortcut = shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = exePath;
                shortcut.WorkingDirectory = installPath;
                shortcut.Description = "Data Logger";
                shortcut.IconLocation = exePath;
                shortcut.Save();

                Marshal.FinalReleaseComObject(shortcut);
                Marshal.FinalReleaseComObject(shell);

                return _fileService.Exists(shortcutPath);
            }
            catch
            {
                return false;
            }
        }

        private bool PinToTaskbar()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "Data Logger.lnk");

                if (!_fileService.Exists(shortcutPath))
                    return false;

                Type shellType = Type.GetTypeFromProgID("Shell.Application")!;
                dynamic shell = Activator.CreateInstance(shellType)!;

                dynamic folder = shell.Namespace(desktopPath);
                dynamic item = folder.ParseName("Data Logger.lnk");

                bool pinned = false;

                foreach (dynamic verb in item.Verbs())
                {
                    string name = verb.Name;

                    if (name.Contains("taskbar", StringComparison.OrdinalIgnoreCase))
                    {
                        verb.DoIt();
                        pinned = true;
                        break;
                    }
                }

                Marshal.FinalReleaseComObject(item);
                Marshal.FinalReleaseComObject(folder);
                Marshal.FinalReleaseComObject(shell);

                return pinned;
            }
            catch
            {
                return false;
            }
        }

        private string ExtractDlsIcon()
        {
            string iconPath = Path.Combine(_programDataPath, "dls_icon.ico");

            using Stream? resource = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetManifestResourceNames()
                        .First(name => name.EndsWith("dls_icon.ico", StringComparison.OrdinalIgnoreCase)))
                ?? throw new InvalidOperationException("Embedded dls_icon.ico was not found.");
            using FileStream file = new(iconPath, FileMode.Create, FileAccess.Write, FileShare.None);

            resource.CopyTo(file);

            return iconPath;
        }

        private bool AssociateDlsFiles()
        {
            string progId = "DataLogger.File";
            string exePath = Path.Combine(installPath, "Data Logger.exe");
            string iconPath = ExtractDlsIcon();

            try
            {
                if (!_fileService.Exists(exePath))
                    return false;

                using RegistryKey extensionKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\.dls") ?? throw new InvalidOperationException("Failed to create .dls registry key.");
                extensionKey.SetValue("", progId);

                using RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}") ?? throw new InvalidOperationException("Failed to create DataLogger.File registry key.");
                progIdKey.SetValue("", "Data Logger File");

                using RegistryKey iconKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}\DefaultIcon") ?? throw new InvalidOperationException("Failed to create DefaultIcon registry key.");
                iconKey.SetValue("", $"\"{iconPath}\",0");

                using RegistryKey commandKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}\shell\open\command") ?? throw new InvalidOperationException("Failed to create open command registry key.");
                commandKey.SetValue("", $"\"{exePath}\" \"%1\"");

                NotifyFileAssociationChanged();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to associate .dls files: {ex.Message}");

                try
                {
                    Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{progId}", false);

                    using RegistryKey? extensionKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\.dls", true);

                    if (extensionKey?.GetValue("")?.ToString() == progId)
                        Registry.CurrentUser.DeleteSubKey(@"Software\Classes\.dls", false);

                    NotifyFileAssociationChanged();
                }
                catch (Exception cleanupEx)
                {
                    Debug.WriteLine($"Failed to clean up .dls association: {cleanupEx.Message}");
                }

                return false;
            }
        }

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private static void NotifyFileAssociationChanged()
        {
            const uint SHCNE_ASSOCCHANGED = 0x08000000;
            const uint SHCNF_IDLIST = 0x0000;

            SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }

        private bool BackupDatabase(string databasePath, string backupPath)
        {
            try
            {
                string? backupDirectory = Path.GetDirectoryName(backupPath);

                if (string.IsNullOrWhiteSpace(backupDirectory))
                    return false;

                _fileService.CreateDirectory(backupDirectory);

                if (File.Exists(backupPath))
                    File.Delete(backupPath);

                File.Move(databasePath, backupPath);

                return File.Exists(backupPath) && !File.Exists(databasePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to backup database: {ex.Message}");
                return false;
            }
        }


        private static string ExtractSetup()
        {
            string resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .First(name => name.EndsWith("Setup.exe", StringComparison.OrdinalIgnoreCase));

            string setupPath = Path.Combine(Path.GetTempPath(), "Setup.exe");

            using Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException("Embedded Setup.exe was not found.");
            using FileStream file = new(setupPath, FileMode.Create, FileAccess.Write, FileShare.None);

            resource.CopyTo(file);

            return setupPath;
        }




        private async void on_INSTALL_ClickedAsync(object sender, RoutedEventArgs e)
        {
            _mainWindow.ShowInstallingPage();

            try
            {
                // Ensure Disk Space is Sufficient
                DriveInfo drive = new("C:");
                if (drive.AvailableFreeSpace < APP_SIZE * 1024 * 1024)
                {
                    _mainWindow.ShowFailedPage("Insufficient storage space available.");
                    return;
                }

                // Create ProgramData and Resources folder
                bool resourcesCreated = _fileService.ResourcesCreated();
                bool programDataCreated = _fileService.Exists(_programDataPath);

                if (!programDataCreated || !resourcesCreated)
                {
                    if (!_isReinstall)
                        _mainWindow.ShowFailedPage("Failed to create required folders.");
                    else
                        _mainWindow.ShowFailedPage(isSecondFailure: true);

                    return;
                }

                // Capture the currently installed version
                string previousVersion = string.Empty;

                string currentExePath = Path.Combine(installPath, "current", "Data Logger.exe");

                if (File.Exists(currentExePath))
                {
                    previousVersion = FileVersionInfo.GetVersionInfo(currentExePath).ProductVersion ?? string.Empty;
                }

                // Database paths
#if RELEASE
                string alpha5DatabasePath = @"C:\DLS\Depository\LOGS.db";
                string alpha5BackupPath = @"C:\DLS\Depository\_backup\LOGS_OLD_1.3-alpha.5.db";
                string alpha6DatabasePath = alpha5DatabasePath;
#endif


                string setupPath = ExtractSetup();

                ProcessStartInfo processInfo = new()
                {
                    FileName = setupPath,
                    Arguments = $"--silent --installto \"{installPath}\"",
                    UseShellExecute = true
                };

                Process? process = Process.Start(processInfo) ?? throw new InvalidOperationException("Process failed to start.");
                using (process)
                {
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        _mainWindow.ShowFailedPage(isSecondFailure: _isReinstall);
                        return;
                    }
                }

                File.Delete(setupPath);

#if RELEASE
                /* DATABASE MIGRATION */

                if (previousVersion == "1.3.0-alpha.5")
                {
                    // Verify the Alpha 5 database exists
                    if (File.Exists(alpha5DatabasePath))
                    {
                        // Move Alpha 5 database to its backup location
                        bool backupSuccessful = BackupDatabase(alpha5DatabasePath, alpha5BackupPath);

                        if (!backupSuccessful)
                        {
                            _mainWindow.ShowFailedPage(isSecondFailure: _isReinstall);
                            return;
                        }

                        try
                        {
                            // Create the encrypted Alpha 6 database from the Alpha 5 backup
                            await DbMigrationService.MigrateAsync(alpha5BackupPath, alpha6DatabasePath);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Database migration failed: {ex}");

                            _mainWindow.ShowFailedPage(isSecondFailure: _isReinstall);
                            return;
                        }
                    }
                }

                /* POST-INSTALLATION SETUP */

                // Register Data Logger instance
                _installationRegistry.RegisterCurrentUser();

                // Desktop shortcut
                if (this.checkBox_CREATE_SHORTCUT.IsChecked ?? false)
                {
                    ShortcutIsCreated = CreateDesktopShortcut();
                }

                // .dls file association
                if (this.checkBox_ASSOCIATE_DLS.IsChecked ?? false)
                {
                    AssociateDlsFiles();
                }

#endif
                _mainWindow.ShowLaunchPage();
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} on_INSTALL_ClickedAsync(): {ex.Message}");
            }

            _mainWindow.ShowFailedPage(isSecondFailure: _isReinstall);
        }


    }
}
