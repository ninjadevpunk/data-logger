using Core.Interfaces;
using Core.Utilities;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Installer.Services
{
    internal class InstallerFileService : IFileService
    {
        private readonly string MainFolder;
        private readonly string ProgramDataPath;
        private readonly string DepositoryPath;
        private readonly string ResourceDirectory;
        private readonly string IdentifiersPath;

        public InstallerFileService()
        {
#if DEBUG
            string configFileName = "appsettings.DevMode.json";
#else
            string configFileName = "appsettings.json";
#endif

            var installerAssembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Could not determine the entry assembly.");

            string resourceName = installerAssembly.GetManifestResourceNames()
                .First(name => name.EndsWith(configFileName, StringComparison.OrdinalIgnoreCase));

            using Stream stream = installerAssembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException("Configuration resource not found.", resourceName);

            using StreamReader reader = new StreamReader(stream);
            JObject config = JObject.Parse(reader.ReadToEnd());

            MainFolder = config["Paths"]?["Root"]?.ToString() ?? throw new InvalidOperationException("Paths:Root is missing from configuration.");
#if DEBUG
            ProgramDataPath = Path.Combine(AppContext.BaseDirectory, "Data Logger");
#else
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger");
            ProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");
#endif
            DepositoryPath = Path.Combine(MainFolder, "Depository");
            ResourceDirectory = Path.Combine(DepositoryPath, "res");
            IdentifiersPath = Path.Combine(ResourceDirectory, "_identifiers.index");

            ResourcesCreated();
        }

        public bool CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} CreateDirectory(): {ex.Message}");
                return false;
            }
        }



        public string? ReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} ReadAllText(): {ex.Message}");
                return null;
            }
        }


        public bool DeleteDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    return true;

                Directory.Delete(path, recursive: true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} DeleteDirectory(): {ex.Message}");
                return false;
            }
        }

        public bool Exists(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates vital resources for the app to function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        public bool ResourcesCreated()
        {
            // Make sure the file with Identifiers exists.
            // If it doesn't exist, create it.
            
            try
            {
                var folder = MainFolder;
                // Base Folder
                if (!Directory.Exists(MainFolder))
                {
                    Directory.CreateDirectory(folder);
                    GrantFolderPermissions(folder);
                }

                // ProgramData
                if (!Exists(ProgramDataPath))
                {
                    CreateDirectory(ProgramDataPath);
                }

                // Depository
                if (!Directory.Exists(DepositoryPath))
                {
                    folder = DepositoryPath;
                    Directory.CreateDirectory(folder);

                    // Hide Depository
                    DirectoryInfo depositoryInfo = new DirectoryInfo(folder);
                    depositoryInfo.Attributes |= FileAttributes.Hidden;
                }

                // Respurces Folder
                if (!Directory.Exists(ResourceDirectory))
                {
                    folder = ResourceDirectory;
                    Directory.CreateDirectory(folder);

                    // Main Index File
                    using (var fileStream = new FileStream(IdentifiersPath, FileMode.CreateNew)) { }
                }

                return true;
            }
            catch (UnauthorizedAccessException unex)
            {
                Debug.WriteLine($"An UnauthorizedAccessException error occurred in ResourcesCreated(): {unex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} ResourcesCreated(): {ex.Message}");
            }

            return false;

        }


        private void GrantFolderPermissions(string folderPath)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            FileSystemAccessRule accessRule = new FileSystemAccessRule(
                sid,
                FileSystemRights.Modify | FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow
                );

            dirSecurity.AddAccessRule(accessRule);
            dirInfo.SetAccessControl(dirSecurity);
#pragma warning restore CA1416 // Validate platform compatibility
        }



    }
}
