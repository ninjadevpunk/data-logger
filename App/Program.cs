#if RELEASE
using Core.Interfaces;
using Core.Services;
using FileSys.Interfaces;
using FileSys.Services;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Velopack;
#endif

namespace Data_Logger_1._3;

public static class Program
{
#if RELEASE
    private static bool DeleteDesktopShortcut()
    {
        try
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktopPath, "Data Logger.lnk");

            if (!File.Exists(shortcutPath))
                return true;

            File.Delete(shortcutPath);

            return !File.Exists(shortcutPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to delete desktop shortcut: {ex.Message}");
            return false;
        }
    }

    private static bool RemoveDlsFileAssociation()
    {
        try
        {
            const string progId = "DataLogger.File";

            Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{progId}", false);

            using RegistryKey? extensionKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\.dls", writable: true);

            if (extensionKey?.GetValue("")?.ToString() == progId)
                Registry.CurrentUser.DeleteSubKey(@"Software\Classes\.dls", false);

            NotifyFileAssociationChanged();

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to remove .dls association: {ex.Message}");
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
#endif








    [STAThread]
    public static void Main(string[] args)
    {
#if RELEASE
        if (args.Contains("--cleanup-data"))
        {
            ICacheService cacheService = new CacheMaster();

            if (!cacheService.DeleteDirectory(@"C:\DLS"))
                Debug.WriteLine("Failed to delete application data.");

            if (!cacheService.DeleteDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger")))
                Debug.WriteLine("Failed to delete ProgramData folder");

            return;
        }

#pragma warning disable CA1416 // Validate platform compatibility
        VelopackApp.Build()
            .OnBeforeUninstallFastCallback(version =>
            {
                try
                {
                    if (!DeleteDesktopShortcut())
                        Debug.WriteLine("Failed to delete shortcut");

                    if (!RemoveDlsFileAssociation())
                        Debug.WriteLine("Failed to remove file association");

                    IInstallationRegistry installationRegistry = new InstallationRegistry();

                    // Unregister current user first.
                    if (!installationRegistry.UnregisterCurrentUser())
                    {
                        Debug.WriteLine("Failed to unregister current user. Database will be preserved.");
                        return;
                    }

                    // Only remove shared application data when no registered installations remain.
                    if (!installationRegistry.HasInstallations())
                    {

                        IElevationService elevationService = new ElevationService();

                        string executablePath = Environment.ProcessPath ?? throw new InvalidOperationException("Unable to determine application executable path.");


                        if (!elevationService.ExecuteElevated(executablePath, "--cleanup-data"))
                            Debug.WriteLine("Failed to launch elevated data cleanup.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to clean up application data: {ex}");
                }
            })
            .Run();
#pragma warning restore CA1416 // Validate platform compatibility
#endif

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}