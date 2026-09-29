using Core.DTOs.Cache;
using Core.Enums;
using Core.Models;

namespace FileSys.Interfaces
{

    public interface ICacheService
    {
        bool CreateDirectory(string path);
        public bool Exists(string path) => File.Exists(path);
        string? ReadAllText(string path);
        bool WriteAllText(string path, string contents);
        bool DeleteDirectory(string path);

        /// <summary>
        /// Creates a settings file. Throws an <see cref="InvalidOperationException"/> is the settings path is invalid.
        /// </summary>
        /// <param name="userId">The logged-in user's ID.</param>
        /// <returns>Returns whether the operation was successful or not.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        bool CreateSettingsFile(int userId);


        /// <summary>
        /// Creates vital resources for the app to function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        bool ResourcesCreated();


        /// <summary>
        /// Only checks if the Identifiers file exists and creates it if it doesn't. Will then insert found identifiers into the Identifiers list.
        /// </summary>
        /// <returns>Returns true if the file was successfully created in the event that the file doesn't already exist or true because all identifiers found were successfully added.
        /// </returns>
        bool IdentifiersChecked();

        /// <summary>
        /// Checks if the Identifiers file exists and adds a new log's ID into the list. P.S. Will call IdentifiersChecked() automatically.
        /// </summary>
        /// <param name="log">The log that will be cached.</param>
        /// <returns>The log's ID if the identifier was successfully inserted.</returns>
        string IdentifiersChecked(LOG log);

        void IdentifiersChecked(string id);

        public string SaveProfilePicture(string tempPath);
        public void DeleteTempProfilePics();
        void DeleteCacheFile(int userId, string fileName, LogContext cacheContext);
        void DeleteLogCache(int userId, string logID, LogContext cacheContext);



        /// <summary>
        /// Saves a log on the logger.
        /// </summary>
        /// <param name="log">The log that is top be saved.</param>
        /// <param name="filePath">The location to store the log.</param>
        /// <param name="cacheContext">The type of log being saved.</param>
        void SaveLOG(LogCacheDTO logCacheDTO, string filePath);


        /// <summary>
        /// Loads a log from the specified file.
        /// </summary>
        /// <param name="filePath">The location of the log to load.</param>
        /// <returns>The loaded log, or <see langword="null"/> if the log could not be loaded.</returns>
        LOG? LoadLOG(string filePath);



        // Cache



        /// <summary>
        /// Saves a temporarily logged Qt log after its been annotated.
        /// </summary>
        /// <param name="qtCacheDTO">The Qt log to store</param>
        void SaveQtCodeCache(QtCacheDTO qtCacheDTO);


        /// <summary>
        /// Saves a temporarily logged coding log after its been annotated.
        /// </summary>
        /// <param name="codingCacheDTO">The coding log to store</param>
        /// <param name="logContext">The type of log being cached</param>
        void SaveCodeCache(CodingCacheDTO codingCacheDTO, LogContext logContext = LogContext.Coding);


        /// <summary>
        /// Saves a temporarily logged Android coding log after its been annotated.
        /// </summary>
        /// <param name="androidCacheDTO">The Android coding log to store</param>
        void SaveAndroidCodeCache(AndroidCacheDTO androidCacheDTO);



        /// <summary>
        /// Retrieves a list of code log cache.
        /// </summary>
        /// <returns>A list of QtCacheDTO for a Qt coding log list.</returns>
        IReadOnlyList<QtCacheDTO> LoadQtCodeCache(int userId);


        /// <summary>
        /// Retrieves a list of code log cache.
        /// </summary>
        /// <returns>A list of CodingCacheDTO for a coding log list.</returns>
        IReadOnlyList<CodingCacheDTO> LoadCodeCache(int userId);


        /// <summary>
        /// Retrieves a list of Android coding log cache.
        /// </summary>
        /// <returns>A list of AndroidCacheDTO for an Android coding log list.</returns>
        IReadOnlyList<AndroidCacheDTO> LoadASCodeCache(int userId);

        string LogCacheExtension(LogContext context);


    }
}
