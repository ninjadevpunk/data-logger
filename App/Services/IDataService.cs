using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using static Data_Logger_1._3.Models.LOG;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Services
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
        Task SignOutUser();
        ACCOUNT GetUser();
        string GetAuthorName();
        string GetDisplayPic();
        Task<string> UpdateProfilePic(string emailAddress);

        // --- CacheMaster access ---
        CacheMaster GetCachemaster();

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

        Task<List<OutputClass>?> ListQtOutputs();
        Task<List<OutputClass>?> ListASOutputs();
        Task<List<OutputClass>?> ListOutputs(CATEGORY category);

        Task<List<TypeClass>?> ListQtTypes();
        Task<List<TypeClass>?> ListASTypes();
        Task<List<TypeClass>?> ListTypes(CATEGORY category);

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
        Task<bool> DeleteLOG(LOG log);
        Task<bool> DeleteLOGByID(int ID);
        Task<bool> DeleteNote(int ID);

        // Cache
        void SaveLOG(LOG log, string filePath);
        void DeleteCacheFile(string id, CacheContext cacheContext);

        ObservableCollection<QtLOGViewModel> RetrieveQtCache(LogCacheViewModel logCacheViewModel,
            IDataService dataService);

        ObservableCollection<AndroidLOGViewModel> RetrieveASCache(LogCacheViewModel logCacheViewModel);
        ObservableCollection<CodeLOGViewModel> RetrieveCodeCache(LogCacheViewModel logCacheViewModel);
        ObservableCollection<GraphicsLOGViewModel> RetrieveGraphicsCache(LogCacheViewModel logCacheViewModel);
        ObservableCollection<FilmLOGViewModel> RetrieveFilmCache(LogCacheViewModel logCacheViewModel);
        ObservableCollection<FlexiLOGViewModel> RetrieveFlexibleCache(LogCacheViewModel logCacheViewModel);

        // Settings
        Task<bool> UpdateProfilePicAsync(int id, string filePath);
        Task<bool> UpdateUserAsync(UserSettings user);
        Task<bool> UpdateUserPasswordAsync(string newPassword, string email);

        // Feedback
        Task HandleExceptionAsync(Exception exception, string methodName, string exceptionType = "Exception");
        Task HandleExceptionAsync(string methodName, Exception ex, string messageBoxMessage = "A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
            string exceptionType = "Exception");

    }
}
