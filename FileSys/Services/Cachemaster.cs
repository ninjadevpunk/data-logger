using Core.DTOs.Cache;
using Core.Enums;
using Core.Models;
using Core.Utilities;
using FileSys.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileSys.Services
{
    /// <summary>
    /// Class for temporary and permanent file storage. Related to the ENTITYMASTER class. This class creates files to permanently keep contents of LOGS 
    /// for backup purposes.
    /// </summary>
    public class CacheMaster : ICacheService
    {


        public List<string>? Identifiers { get; set; } = new();
        public StreamWriter? Writer { get; set; }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public readonly string MainFolder;
        public readonly string DepositoryPath;
        public readonly string SettingsPath;
        public readonly string ProgramDataPath;

        public readonly string ResourceDirectory;
        public readonly string IdentifiersPath;
        public readonly string SubjectIdsPath;
        public readonly string PostitIdsPath;


        public CacheMaster(bool mustDefer = false)
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
            ProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");

            DepositoryPath = Path.Combine(MainFolder, "Depository");

            ResourceDirectory = Path.Combine(DepositoryPath, "res");

            IdentifiersPath = Path.Combine(ResourceDirectory, "_identifiers.index");
            SubjectIdsPath = Path.Combine(ResourceDirectory, "subject.index");
            PostitIdsPath = Path.Combine(ResourceDirectory, "postit.index");
#if DEBUG
            SettingsPath = Path.Combine(AppContext.BaseDirectory, "Data Logger");
#else
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger");
#endif


            if (!mustDefer)
            {
                if (ResourcesCreated())
                {
                    IdentifiersChecked();
                }
            }
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
                Debug.WriteLine($"Exception occurred in CreateDirectory(): {ex.Message}");
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
                Debug.WriteLine($"Exception occurred in ReadAllText(): {ex.Message}");
                return null;
            }
        }

        public bool WriteAllText(string path, string contents)
        {
            try
            {
                File.WriteAllText(path, contents);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in WriteAllText(): {ex.Message}");
                return false;
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
                Debug.WriteLine($"Exception occurred in DeleteDirectory(): {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Creates a settings file. Throws an <see cref="InvalidOperationException"/> is the settings path is invalid.
        /// </summary>
        /// <param name="userId">The logged-in user's ID.</param>
        /// <returns>Returns whether the operation was successful or not.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool CreateSettingsFile(int userId)
        {
            try
            {
                var userFolder = Path.Combine(SettingsPath ?? throw new InvalidOperationException("Path is null!"), userId.ToString());

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                return true;
            }
            catch (UnauthorizedAccessException unex)
            {
                Debug.WriteLine($"UnauthorizedAccessException error occurred in CreateSettingsFile(userId): {unex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in CreateSettingsFile(userId): {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Creates vital resources for the app to function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        public bool ResourcesCreated()
        {
            /** Make sure the file with Identifiers exists.
             * If it doesn't exist, create it.
             */
            try
            {
                var folder = MainFolder;
                // Base Folder
                if (!Directory.Exists(MainFolder))
                {
                    folder ??= string.Empty;
                    Directory.CreateDirectory(folder);
                    GrantFolderPermissions(folder);
                }

                // Depository
                if (!Directory.Exists(DepositoryPath))
                {
                    folder = DepositoryPath ?? string.Empty;
                    Directory.CreateDirectory(folder);

                    // Hide Depository
                    DirectoryInfo depositoryInfo = new DirectoryInfo(folder);
                    depositoryInfo.Attributes |= FileAttributes.Hidden;
                }

                // Respurces Folder
                if (!Directory.Exists(ResourceDirectory))
                {
                    folder = ResourceDirectory ?? string.Empty;
                    Directory.CreateDirectory(folder);

                    // Main Index File
                    using (var fileStream = new FileStream(IdentifiersPath!, FileMode.CreateNew)) { }

                    // Subject ID Index
                    using (var fileStream = new FileStream(SubjectIdsPath!, FileMode.CreateNew)) { }

                    // PostIt ID Index
                    using (var fileStream = new FileStream(PostitIdsPath!, FileMode.CreateNew)) { }
                }

                return true;
            }
            catch (UnauthorizedAccessException unex)
            {
                Debug.WriteLine($"An UnauthorizedAccessException error occurred in ResourcesCreated(): {unex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred in ResourcesCreated(): {ex.Message}");
                Identifiers = null;
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


        /// <summary>
        /// Only checks if the Identifiers file exists and creates it if it doesn't. Will then insert found identifiers into the Identifiers list.
        /// </summary>
        /// <returns>Returns true if the file was successfully created in the event that the file doesn't already exist or true because all identifiers found were successfully added.
        /// </returns>
        public bool IdentifiersChecked()
        {
            if (Identifiers is not null)
                Identifiers.Clear();
            else
                return false;

            try
            {
                if (!File.Exists(IdentifiersPath))
                {
                    var f = new FileStream(IdentifiersPath!, FileMode.CreateNew);
                    f.Close();

                }
                else
                {

                    foreach (string s in File.ReadLines(IdentifiersPath))
                    {
                        Identifiers.Add(s);
                    }

                }


            }
            catch (Exception)
            {
                Identifiers = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the Identifiers file exists and adds a new log's ID into the list. P.S. Will call IdentifiersChecked() automatically.
        /// </summary>
        /// <param name="log">The log that will be cached.</param>
        /// <returns>The log's ID if the identifier was successfully inserted.</returns>
        public string IdentifiersChecked(LOG log)
        {
            if (IdentifiersChecked() && Identifiers is not null)
            {
                try
                {
                    var id = log.Start.ToString("yyyyMMdd_HHmmss");

                    /** If the id doesn't exist, add it to the Identifiers List and write the new id to the
                     * Identifiers.index file.
                     */

                    if (!Identifiers.Contains(id))
                    {
                        Identifiers.Add(id);

                        Writer = new StreamWriter(IdentifiersPath!);
                        foreach (string t in Identifiers.Order())
                        {
                            Writer.WriteLine(t);
                        }

                    }

                    return id;
                }
                catch (Exception)
                {
                    Identifiers = null;
                }
            }

            return "";

        }

        public void IdentifiersChecked(string id)
        {
            if (IdentifiersChecked() && Identifiers is not null)
            {
                try
                {

                    // If the id doesn't exist, add it to the Identifiers List and write the new id to the
                    // Identifiers .index file.

                    if (!Identifiers.Contains(id))
                    {
                        Identifiers.Add(id);

                        Writer = new StreamWriter(IdentifiersPath!);
                        foreach (string t in Identifiers.Order())
                        {
                            Writer.WriteLine(t);
                        }

                        Writer.Close();

                    }

                }
                catch (Exception)
                {
                    Identifiers = null;
                }
            }


        }

        /// <summary>
        /// Saves a temporary profile picture to the permanent profile picture directory.
        /// </summary>
        /// <param name="tempPath">The temporary profile picture path.</param>
        /// <returns>Returns the permanent file path, or an empty string if the save failed.</returns>
        public string SaveProfilePicture(string tempPath)
        {
            try
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger", "Profile Pics");

                Directory.CreateDirectory(folder);

                string newPath = Path.Combine(folder, $"{Guid.NewGuid()}.png");

                if (tempPath.Contains("/Assets"))
                    return string.Empty;

                File.Copy(tempPath, newPath, overwrite: false);

                return newPath;
            }
            catch (Exception ex)
            {
                //App.LogExceptionAsync(ex);
                Debug.WriteLine(ex, "Exception occurred in SaveProfilePicture");
                return string.Empty;
            }
        }

        /// <summary>
        /// Deletes profile pics saved in temp folder created by Browse button logic
        /// </summary>
        public void DeleteTempProfilePics()
        {
            try
            {
                // Save to User's Temp folder
                string folder = Path.Combine(Path.GetTempPath(), "Data Logger", "Temp Profile Pics");

                if (!Directory.Exists(folder))
                    return;

                // Delete all files.
                foreach (var file in Directory.GetFiles(folder))
                {
                    File.Delete(file);
                }

                // Delete any subdirectories and their contents.
                foreach (var directory in Directory.GetDirectories(folder))
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
                //App.LogExceptionAsync(ex);
                Debug.WriteLine(ex, "Exception occurred in DeleteTempProfilePics");
            }
        }

        public void DeleteCacheFile(int userId, string fileName, LogContext cacheContext)
        {
            DeleteLogCache(userId, fileName, cacheContext);
        }









        /// <summary>
        /// Saves a log on the logger.
        /// </summary>
        /// <param name="log">The log that is top be saved</param>
        /// <param name="filePath">The location to store the log</param>
        public void SaveLOG(LogCacheDTO logCacheDTO, string filePath)
        {

            using var fileStream = new FileStream(filePath, FileMode.Create);


            switch (logCacheDTO.Context)
            {
                case LogContext.Qt:
                    {
                        break;
                    }
                default:
                    {
                        var codingCacheDTO = (CodingCacheDTO)logCacheDTO;
                        string json = JsonConvert.SerializeObject(codingCacheDTO, settings);

                        File.WriteAllText(filePath, json);
                        break;
                    }
            }
        }

        /// <summary>
        /// Loads a log from the specified file.
        /// </summary>
        /// <param name="filePath">The location of the log to load.</param>
        /// <returns>The loaded log, or <see langword="null"/> if the log could not be loaded.</returns>
        public LOG? LoadLOG(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);

            var codingLog = JsonConvert.DeserializeObject<CodingLOG>(json, settings);
            return codingLog;
        }


        public string LogCacheExtension(LogContext context)
        {
            switch (context)
            {
                case LogContext.Qt:
                    return "qtcache";
                case LogContext.AndroidStudio:
                    return "ascache";
                case LogContext.Coding:
                    return "cdecache";
                case LogContext.Graphics:
                    return "gpscache";
                case LogContext.Film:
                    return "flmcache";
                case LogContext.Flexi:
                    return "flxcache";
                default:
                    return string.Empty;
            }
        }



        public void DeleteLogCache(int userId, string identifier, LogContext cacheContext)
        {
            if (Identifiers is null || !Identifiers.Contains(identifier))
                return;

            try
            {
                var filePath = $@"{ResourceDirectory}\{userId}\{identifier}.{LogCacheExtension(cacheContext)}";

                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Remove logID from IDENTIFIERS
                Identifiers.Remove(identifier);

                if (File.Exists(IdentifiersPath))
                    File.Delete(IdentifiersPath);

                // Create new IDENTIFIERS file with updated list
                using var fileStream = new FileStream(IdentifiersPath!, FileMode.Create);
                fileStream.Close();

                // Write IDENTIFIERS to new file
                using var writer = new StreamWriter(IdentifiersPath!);
                foreach (string id in Identifiers.Order())
                {
                    writer.WriteLine(id);
                }
                writer.Close();
            }
            catch (IOException ioX)
            {
                Debug.WriteLine($"IOException found near DeleteLogCache(identifier, cacheContext): {ioX.Message}");
            }
            catch (Exception ex)
            {
                // Handle exception
                Debug.WriteLine($"Exception found near DeleteLogCache(identifier, cacheContext): {ex.Message}");
            }
        }

        private void SaveCache(int userId, string cacheFileName, string json, LogContext cacheContext)
        {
            try
            {
                string filePath = $@"{ResourceDirectory}\{userId}\{cacheFileName}.{LogCacheExtension(cacheContext)}";

                File.WriteAllText(filePath, json);
            }
            catch (IOException ioX)
            {
                Debug.WriteLine($"IOException found near SaveCache(logID,json...: {ioX.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near SaveCache(logID,json...: {e.Message}");
            }
        }


        #region Coding




        /// <summary>
        /// Saves a temporarily logged Qt log after its been annotated.
        /// </summary>
        /// <param name="qtCacheDTO">The Qt log to store</param>
        public void SaveQtCodeCache(QtCacheDTO qtCacheDTO)
        {
            try
            {
                var userId = qtCacheDTO.UserId;
                if (!EnsureDTOCacheDirectory(userId))
                    throw new InvalidOperationException("Directory doesn't exist!");

                string json = JsonConvert.SerializeObject(qtCacheDTO, settings);
                var cacheFileName = qtCacheDTO.Start.ToString("yyyyMMdd_HHmmss");

                SaveCache(qtCacheDTO.UserId, cacheFileName, json, LogContext.Qt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in SaveQtCodeCache: {ex.Message}");
            }
        }


        /// <summary>
        /// Saves a temporarily logged coding log after its been annotated.
        /// </summary>
        /// <param name="codingLOG">The coding log to store</param>
        /// <param name="cacheContext">The type of log being cached</param>        
        public void SaveCodeCache(CodingCacheDTO codingCacheDTO, LogContext cacheContext = LogContext.Coding)
        {
            try
            {
                var userId = codingCacheDTO.UserId;
                if (!EnsureDTOCacheDirectory(userId))
                    throw new InvalidOperationException("Directory doesn't exist!");

                string json = JsonConvert.SerializeObject(codingCacheDTO, settings);
                var cacheFileName = codingCacheDTO.Start.ToString("yyyyMMdd_HHmmss");

                SaveCache(codingCacheDTO.UserId, cacheFileName, json, cacheContext);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in SaveCodeCache: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves a temporarily logged Android coding log after its been annotated.
        /// </summary>
        /// <param name="androidCacheDTO">The Android coding log to store</param>
        public void SaveAndroidCodeCache(AndroidCacheDTO androidCacheDTO)
        {
            try
            {
                var userId = androidCacheDTO.UserId;
                if (!EnsureDTOCacheDirectory(userId))
                    throw new InvalidOperationException("Directory doesn't exist!");

                string json = JsonConvert.SerializeObject(androidCacheDTO, settings);
                var cacheFileName = androidCacheDTO.Start.ToString("yyyyMMdd_HHmmss");

                SaveCache(androidCacheDTO.UserId, cacheFileName, json, LogContext.AndroidStudio);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in SaveAndroidCodeCache: {ex.Message}");
            }
        }



        /// <summary>
        /// Retrieves a list of code log cache.
        /// </summary>
        /// <returns>A list of QtCacheDTO for a Qt coding log list.</returns>
        public IReadOnlyList<QtCacheDTO> LoadQtCodeCache(int userId)
        {
            if (Identifiers != null)
            {
                try
                {
                    string extenstion = LogCacheExtension(LogContext.Qt);
                    string root = $@"{ResourceDirectory}\{userId}";
                    List<QtCacheDTO> list = [];

                    foreach (string id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath))
                        {
                            var json = File.ReadAllText(filePath);

                            var qtCacheDTO = JsonConvert.DeserializeObject<QtCacheDTO>(json, settings);

                            if (qtCacheDTO != null && qtCacheDTO.ApplicationName == Constants.Qt)
                                list.Add(qtCacheDTO);
                        }
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception occurred in LoadQtCodeCache: {ex.Message}");
                }
            }

            return [];
        }

        /// <summary>
        /// Retrieves a list of code log cache.
        /// </summary>
        /// <returns>A list of CodingCacheDTO for a coding log list.</returns>
        public IReadOnlyList<CodingCacheDTO> LoadCodeCache(int userId)
        {
            if (Identifiers != null)
            {
                try
                {
                    const string extenstion = "cdecache";
                    string root = $@"{ResourceDirectory}\{userId}";
                    List<CodingCacheDTO> list = [];

                    foreach (string id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath))
                        {
                            var json = File.ReadAllText(filePath);

                            var codingCacheDTO = JsonConvert.DeserializeObject<CodingCacheDTO>(json, settings)!;
                            list.Add(codingCacheDTO);
                        }
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception occurred in LoadCodeCache: {ex.Message}");
                }
            }

            return [];
        }

        /// <summary>
        /// Retrieves a list of Android coding log cache.
        /// </summary>
        /// <returns>A list of AndroidCacheDTO for an Android coding log list.</returns>
        public IReadOnlyList<AndroidCacheDTO> LoadASCodeCache(int userId)
        {
            if (Identifiers != null)
            {
                try
                {
                    string extenstion = LogCacheExtension(LogContext.AndroidStudio);
                    string root = $@"{ResourceDirectory}\{userId}";
                    List<AndroidCacheDTO> list = [];

                    foreach (string id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath))
                        {
                            var json = File.ReadAllText(filePath);

                            var androidCacheDTO = JsonConvert.DeserializeObject<AndroidCacheDTO>(json, settings)!;
                            list.Add(androidCacheDTO);
                        }
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception occurred in LoadASCodeCache: {ex.Message}");
                }
            }

            return [];
        }

        private bool EnsureDTOCacheDirectory(int userId)
        {
            var resSubDirectory = $@"{ResourceDirectory}\{userId}";
            if (Directory.Exists(resSubDirectory))
                return true;

            Directory.CreateDirectory(resSubDirectory);

            return Directory.Exists(resSubDirectory);
        }




        #endregion

    }
}
