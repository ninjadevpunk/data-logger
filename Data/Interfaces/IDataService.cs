using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Identity;
using Core.DTOs.Notes;
using Core.Enums;
using Core.Models;
using Core.Models.App_Models;
using System.Runtime.CompilerServices;
using static Core.Models.LOG;

namespace Data.Interfaces
{
    public interface IDataService
    {

        // Save Database Changes
        Task SaveChangesAsync();
        Task UpdateLogAsync(LOG log);


        // --- User / Account ---
        Task SignInUserAsync();

        /// <summary>
        /// Signs in the current user by setting them as the current user in the database.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Returns a tuple: (success flag, a UserDTO if sign-in succeeded Otherwise null is returned).</returns>
        Task<(bool Success, UserDTO? Account)> SignInUserAsync(string email, string password);
        Task<bool> SignOutUserAsync();
        Task<string> UpdateProfilePicAsync(string emailAddress);

        /// <summary>
        /// Sets the currently online user to online.
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <returns>Returns whether the account is set successfully.</returns>
        Task<bool> SetCurrentUserAsync(int userId);

        /// <summary>
        /// Adds the provided account to the database.
        /// </summary>
        /// <param name="account">The account to add to the database.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<(bool, int)> AddAccountAsync(ACCOUNT account);

        // --- Initialise methods ---
        Task<IReadOnlyList<string>> InitialiseProjectsLISTAsync();
        Task<IReadOnlyList<string>> InitialiseProjectsLISTAsync(string appName);
        Task<IReadOnlyList<string>> InitialiseProjectsLISTAsync(LOG.CATEGORY category);

        Task<IReadOnlyList<string>> InitialiseApplicationsLISTAsync();
        Task<IReadOnlyList<string>> InitialiseApplicationsLISTAsync(LOG.CATEGORY category);

        Task<IReadOnlyList<string>> InitialiseSubjectsLISTAsync(LOG.CATEGORY category);
        Task<IReadOnlyList<string>> InitialiseSubjectsLISTAsync(ProjectClass project);
        Task<IReadOnlyList<string>> InitialiseSubjectsLISTAsync(string appName, string projectName);

        // Inserts
        Task<bool> CreateLOGAsync(LogCacheDTO logCacheDTO);

        /// <summary>
        /// Inserts a note DTO into the database.
        /// </summary>
        /// <param name="noteDTO">The note DTO to insert.</param>
        /// <returns>Returns a tuple: (success flag, the created note data DTO).</returns>
        Task<(bool Success, NoteDataDTO? NoteDataDTO)> CreateNoteAsync(NoteDTO noteDTO);

        /// <summary>
        /// Creates a new checklist in the database based on the provided CheckListDTO object.
        /// </summary>
        /// <param name="checkListItemDTOs">The checklist DTO to create.</param>
        /// <returns>Returns a tuple: (success flag, the created checklist data DTO).</returns>
        Task<(bool Success, CheckListDataDTO? CheckListDataDTO)> CreateCheckListAsync(CheckListDTO checkListItemDTOs);


        // Reads
        Task<IEnumerable<LOG>?> RetrieveLogsAsync(LogContext context);

        /// <summary>
        /// Retrieves notes from the database that belong to a specific user.
        /// </summary>
        /// <param name="id">The logged in user's id.</param>
        /// <returns>Returns an IEnumerable of note items, or null if an error occurs.</returns>
        Task<IEnumerable<NoteItem>?> RetrieveGenericNotesAsync(int id);

        Task<bool> EmailExistsAsync(string email);
        Task<ApplicationClass?> FindApplicationAsync(int ID, string name);
        Task<ApplicationClass?> FindApplicationByIDAsync(int appID);
        Task<ProjectClass?> FindProjectAsync(int? userID, string projectName, int appID);
        Task<ProjectClass?> FindProjectByIDAsync(int projectID);
        Task<OutputClass?> FindOutputAsync(string name);
        Task<OutputClass?> FindOutputByIDAsync(int outputID);
        Task<TypeClass?> FindTypeAsync(string name);
        Task<TypeClass?> FindTypeByIDAsync(int typeID);
        Task<SubjectClass?> FindSubjectAsync(string subject, LOG.CATEGORY category, int appID, int projectID);
        Task<int> FindSubjectIDAsync(SubjectClass subject);

        Task<IEnumerable<string>> ListQtOutputsAsync();
        Task<IEnumerable<string>> ListASOutputsAsync();
        Task<IEnumerable<string>> ListOutputsAsync(CATEGORY category);

        Task<IEnumerable<string>> ListQtTypesAsync();
        Task<IEnumerable<string>> ListASTypesAsync();
        Task<IEnumerable<string>> ListTypesAsync(CATEGORY category);

        Task<int> LogCountAsync();
        Task<int> LogCountAsync(LOG.CATEGORY category);
        Task<int> QtLogCountAsync();
        Task<int> ASLogCountAsync();
        Task<int> FlexiLogCountAsync();

        Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText);
        Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID);
        Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText, int projectID, int appID);
        Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText);
        Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int projectID);
        Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int projectID, int appID);


        // Deletes

        /// <summary>
        /// Deletes a log from the database.
        /// </summary>
        /// <param name="log">The log to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteLOGAsync(LOG log);

        /// <summary>
        /// Deletes a log from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the log to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteLOGByIDAsync(int ID);

        /// <summary>
        /// Deletes a note from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the note to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteNoteAsync(int ID);

        /// <summary>
        /// Deletes a user's account from the database using the provided account ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>Returns true if the account was deleted successfully, false otherwise.</returns>
        Task<bool> DeleteAccountByIDAsync(int ID);

        

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
