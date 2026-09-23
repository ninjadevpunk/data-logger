

using Core.Models;
using Core.Models.App_Models;
using System.Runtime.CompilerServices;
using static Core.Models.LOG;
using static FileSys.Services.CacheMaster;

namespace Data.Interfaces
{
    public interface IDataService
    {
        // Properties
        IReadOnlyList<ProjectClass> SQLITE_PROJECTS { get; }
        IReadOnlyList<ApplicationClass> SQLITE_APPLICATIONS { get; }
        IReadOnlyList<SubjectClass> SQLITE_SUBJECTS { get; }

        // Save Database Changes
        Task SaveChangesAsync();
        Task UpdateLogAsync(LOG log);


        // --- User / Account ---
        Task SignInUser();
        Task<(bool Success, ACCOUNT? Account)> SignInUser(string email, string password);
        Task<bool> SignOutUser();
        ACCOUNT? GetUser();
        string? GetAuthorName();
        string? GetDisplayPic();
        Task<string> UpdateProfilePic(string emailAddress);

        /// <summary>
        /// Sets the currently online user to online.
        /// </summary>
        /// <param name="account">The account that is online.</param>
        /// <returns>Returns whether the account is set successfully.</returns>
        Task<bool> SetCurrentUser(ACCOUNT account);

        /// <summary>
        /// Adds the provided account to the database.
        /// </summary>
        /// <param name="account">The account to add to the database.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<bool> AddAccount(ACCOUNT account);

        // --- Initialise methods ---
        Task InitialiseProjectsLISTAsync();
        Task InitialiseProjectsLISTAsync(LOG.CATEGORY category);

        Task InitialiseApplicationsLISTAsync();
        Task InitialiseApplicationsLISTAsync(LOG.CATEGORY category);

        Task InitialiseSubjectsLIST(LOG.CATEGORY category);
        Task InitialiseSubjectsLIST(ProjectClass project);

        // Inserts
        Task<bool> CreateLOG(LOG log);


        // Reads
        Task<List<LOG>> RetrieveLogs(int userId);
        Task<IEnumerable<LOG>?> RetrieveLogs(CacheContext context);

        Task<bool> EmailExists(string email);
        Task<ApplicationClass?> FindApplication(int ID, string name);
        Task<ApplicationClass?> FindApplicationByID(int appID);
        Task<ProjectClass?> FindProject(int? userID, string projectName, int appID);
        Task<ProjectClass?> FindProjectByID(int projectID);
        Task<OutputClass?> FindOutput(string name);
        Task<OutputClass?> FindOutputByID(int outputID);
        Task<TypeClass?> FindType(string name);
        Task<TypeClass?> FindTypeByID(int typeID);
        Task<SubjectClass?> FindSubject(string subject, LOG.CATEGORY category, int appID, int projectID);
        Task<int> FindSubjectID(SubjectClass subject);

        Task<List<OutputClass>> ListQtOutputs();
        Task<List<OutputClass>> ListASOutputs();
        Task<List<OutputClass>> ListOutputs(CATEGORY category);

        Task<List<TypeClass>> ListQtTypes();
        Task<List<TypeClass>> ListASTypes();
        Task<List<TypeClass>> ListTypes(CATEGORY category);

        Task<int> LogCount();
        Task<int?> LogCount(LOG.CATEGORY category);
        Task<int> QtLogCount();
        Task<int> ASLogCount();
        Task<int> FlexiLogCountAsync();

        Task<List<CodingLOG>?> SearchQtLogs(string searchBarText);
        Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID);
        Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID, int appID);
        Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText);
        Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID);
        Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID, int appID);


        // Deletes

        /// <summary>
        /// Deletes a log from the database.
        /// </summary>
        /// <param name="log">The log to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteLOG(LOG log);

        /// <summary>
        /// Deletes a log from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the log to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteLOGByID(int ID);

        /// <summary>
        /// Deletes a note from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the note to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteNote(int ID);

        /// <summary>
        /// Deletes a user's account from the database using the provided account ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>Returns true if the account was deleted successfully, false otherwise.</returns>
        Task<bool> DeleteAccountByID(int ID);

        // Cache
        void SaveLOG(LOG log, string filePath);
        void DeleteCacheFile(string id, CacheContext cacheContext);

        // Settings

        /// <summary>
        /// Updates the profile picture of a user in the database.
        /// </summary>
        /// <param name="id">The user's id</param>
        /// <param name="filePath">The file path of the current profile picture</param>
        /// <returns>Returns true if the profile pic change succeeded, false otherwise.</returns>
        Task<bool> UpdateProfilePicAsync(int id, string filePath);

        /// <summary>
        /// Updates a user's information in the database based on the provided UserSettings object.
        /// </summary>
        /// <param name="user">The user settings to update</param>
        /// <returns>Returns true if the user information was updated successfully, false otherwise.</returns>
        Task<bool> UpdateUserAsync(UserSettings user);

        /// <summary>
        /// Updates a user's password in the database based on the provided email and new password.
        /// </summary>
        /// <param name="newPassword">The user's new password</param>
        /// <param name="email">The user's email address</param>
        /// <returns>Returns true if the password was updated successfully, false otherwise.</returns>
        Task<bool> UpdateUserPasswordAsync(string newPassword, string email);

        // Feedback

        /// <summary>
        /// Handles a data error by creating an automatic exception feedback report.
        /// </summary>
        /// <param name="error">The data error containing the exception information.</param>
        /// <param name="methodName">The name of the method where the error occurred.</param>
        /// <returns>A task representing the asynchronous feedback creation operation.</returns>
        Task HandleExceptionAsync(DataError dataError, [CallerMemberName] string methodName = "");

        /// <summary>
        /// Handles an exception by creating an automatic exception feedback report.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="methodName">The name of the method where the exception occurred.</param>
        /// <returns>A task representing the asynchronous feedback creation operation.</returns>
        Task HandleExceptionAsync(Exception ex, [CallerMemberName] string methodName = "");

    }
}
