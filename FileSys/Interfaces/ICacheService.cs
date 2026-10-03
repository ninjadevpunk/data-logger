using Core.DTOs.Cache;
using Core.Enums;
using Core.Models;

namespace FileSys.Interfaces
{

    public interface ICacheService
    {
        /// <summary>
        /// Only checks if the Identifiers file exists and creates it if it doesn't. Will then insert found identifiers into the Identifiers list.
        /// </summary>
        /// <returns>Returns true if the file was successfully created in the event that the file doesn't already exist or true.</returns>
        bool IdentifiersLoaded();


        /// <summary>
        /// Adds a log cache's cache ID to the identifiers file.
        /// </summary>
        /// <param name="id">Start date as ID</param>
        void AddLogCacheIdentifier(string id);




        /// <summary>
        /// Checks if an image is a temp folder profile pic.
        /// </summary>
        /// <param name="path">The location of pic</param>
        /// <returns>Returns true if the pic is a temp folder pic; otherwise false.</returns>
        public bool IsTemporaryProfilePic(string path);

        /// <summary>
        /// Saves a temporary profile picture to the permanent profile picture directory.
        /// </summary>
        /// <param name="tempPath">The temporary profile picture path.</param>
        /// <returns>Returns the permanent file path, or an empty string if the save failed.</returns>
        public string SaveProfilePicture(string tempPath);

        /// <summary>
        /// Deletes profile pics saved in temp folder created by Browse button logic
        /// </summary>
        public void DeleteTempProfilePics();



        void DeleteCacheFile(int userId, string fileName, LogContext cacheContext);
        void DeleteLogCache(int userId, string logID, LogContext cacheContext);



        /// <summary>
        /// Saves a log on the logger.
        /// </summary>
        /// <param name="log">The log that is top be saved.</param>
        /// <param name="filePath">The location to store the log.</param>
        /// <param name="cacheContext">The type of log being saved.</param>
        void SaveLOGFile(LogCacheDTO logCacheDTO, string filePath);


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
