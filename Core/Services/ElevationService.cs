using Core.Interfaces;
using Core.Utilities;
using System.Diagnostics;
using System.Security.Principal;

namespace Core.Services
{
    public class ElevationService : IElevationService
    {
        public bool ExecuteElevated(string fileName, string arguments)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    Verb = "runas",
                    UseShellExecute = true
                };

                using Process? process = Process.Start(processInfo);

                if (process == null)
                    return false;

                process.WaitForExit();

                return process.ExitCode == 0;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Debug.WriteLine($"User declined UAC in ExecuteElevated(): {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} ExecuteElevated(): {ex.Message}");
                return false;
            }
        }


        public bool IsAdministrator()
        {
            try
            {

#pragma warning disable CA1416 // Validate platform compatibility
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();

                WindowsPrincipal principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
