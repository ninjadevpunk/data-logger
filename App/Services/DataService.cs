using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static Data_Logger_1._3.Models.LOG;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Services
{

    public class DataService : IDataService
    {
        private readonly CacheMaster _cachemaster;
        private readonly IServiceProvider _serviceProvider;

        private readonly AuthService _authService;

        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Meerkat 2024.3.1";

        internal ProjectsLIST SQLITE_PROJECTS { get; private set; } = new();
        internal ApplicationsLIST SQLITE_APPLICATIONS { get; private set; } = new();
        internal List<SubjectClass> SQLITE_SUBJECTS { get; private set; } = new();

        IReadOnlyList<ProjectClass> IDataService.SQLITE_PROJECTS => SQLITE_PROJECTS;
        IReadOnlyList<ApplicationClass> IDataService.SQLITE_APPLICATIONS => SQLITE_APPLICATIONS;
        IReadOnlyList<SubjectClass> IDataService.SQLITE_SUBJECTS => SQLITE_SUBJECTS;


        public DataService(CacheMaster cachemaster, AuthService authService, IServiceProvider serviceProvider)
        {
            _cachemaster = cachemaster;

            _authService = authService;
            _serviceProvider = serviceProvider;
        }
















        /// <summary>
        /// Signs in the current user by setting them as the current user in the database.
        /// </summary>
        public async Task SignInUser()
        {
            var account = _authService?.Account;
            if (account == null)
                return;

            await UseWriterAsync(writer => writer.SetCurrentUser(account));
        }

        /// <summary>
        /// Signs in the current user by setting them as the current user in the database.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Returns a tuple: (success flag, the ACCOUNT object if sign-in succeeded ?? otherwise null is returned).</returns>
        public async Task<(bool Success, ACCOUNT? Account)> SignInUser(string email, string password)
        {
            var temporaryAccount = await UseReaderAsync(reader => reader.FindAccountByEmail(email, password));

            if (temporaryAccount == null)
                return (false, null);

            temporaryAccount.IsOnline = true;

            var result = await UseWriterAsync(writer => writer.SetCurrentUser(temporaryAccount));

            return (result, temporaryAccount);
        }



        public async Task SignOutUser()
        {
            var account = _authService?.Account;
            if (account == null)
                return;

            await UseWriterAsync(writer => writer.UnsetCurrentUser(account));
        }


        private static bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        public async Task<string> UpdateProfilePic(string emailAddress)
        {
            return await UseReaderAsync(reader => reader.RetrieveProfilePic(emailAddress));
        }


        public ACCOUNT GetUser()
        {
            return _authService.Account;
        }

        public string GetAuthorName()
        {
            return _authService.Account.FirstName + " " + _authService.Account.LastName;
        }

        public CacheMaster GetCachemaster()
        {
            return _cachemaster;
        }

        public string GetDisplayPic()
        {
            return _authService.Account.ProfilePic;
        }




        /// <summary>
        /// Retrieves projects from the database and populates SQLITE_PROJECTS, skipping unnamed projects.
        /// </summary>
        public Task InitialiseProjectsLISTAsync()
        {
            SQLITE_PROJECTS.Clear();

            return UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListProjects(GetUser().accountID);

                foreach (var pro in collection)
                {
                    if (pro != null && pro.Name != "Unnamed Project")
                        SQLITE_PROJECTS.Add(pro);
                }

                return Task.CompletedTask;
            });
        }





        /// <summary>
        /// Retrieves projects from the database of a specified category and populates SQLITE_PROJECTS, skipping unnamed projects.
        /// </summary>
        /// <param name="category">The type of project.</param>
        public Task InitialiseProjectsLISTAsync(LOG.CATEGORY category)
        {
            SQLITE_PROJECTS.Clear();

            return UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListProjects(category, GetUser().accountID);

                foreach (var pro in collection)
                {
                    if (pro != null && pro.Name != "Unnamed Project")
                        SQLITE_PROJECTS.Add(pro);
                }

                return Task.CompletedTask;
            });
        }




        /// <summary>
        /// Retrieves applications from the database and populates SQLITE_APPLICATIONS.
        /// </summary>
        public Task InitialiseApplicationsLISTAsync()
        {
            SQLITE_APPLICATIONS.Clear();

            return UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListApplications(GetUser().accountID);

                foreach (var app in collection)
                {
                    if (app != null)
                        SQLITE_APPLICATIONS.Add(app);
                }

                return Task.CompletedTask;
            });
        }



        /// <summary>
        /// Retrieves applications from the database of a specified category and populates SQLITE_APPLICATIONS.
        /// </summary>
        /// <param name="category">The type of application.</param>
        public Task InitialiseApplicationsLISTAsync(LOG.CATEGORY category)
        {
            SQLITE_APPLICATIONS.Clear();

            return UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListApplications(category, GetUser().accountID);

                foreach (var app in collection)
                {
                    if (app != null)
                        SQLITE_APPLICATIONS.Add(app);
                }

                return Task.CompletedTask;
            });
        }




        /// <summary>
        /// Retrieves subjects from the database of a specified category and populates SQLITE_SUBJECTS, skipping "No Subject".
        /// </summary>
        /// <param name="category">The type of subjects being retrieved.</param>
        public Task InitialiseSubjectsLIST(LOG.CATEGORY category)
        {
            SQLITE_SUBJECTS.Clear();

            return UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListSubjects(category, GetUser().accountID);

                foreach (var subject in collection)
                {
                    if (subject != null && subject.Subject != "No Subject")
                        SQLITE_SUBJECTS.Add(subject);
                }

                return Task.CompletedTask;
            });
        }



        /// <summary>
        /// Retrieves subjects from the database for a specific project and populates SQLITE_SUBJECTS.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved only.</param>
        public Task InitialiseSubjectsLIST(ProjectClass project)
        {
            SQLITE_SUBJECTS.Clear();

            return UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListSubjects(project, GetUser().accountID);

                foreach (var subject in collection)
                {
                    if (subject != null)
                        SQLITE_SUBJECTS.Add(subject);
                }

                return Task.CompletedTask;
            });
        }









        /// <summary>
        /// Retrieves subjects from the database. Used every time the subjects are needed as a list immediately.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved ONLY.</param>
        /// <returns>Returns the subjects as a List.</returns>
        public async Task<List<SubjectClass>?> ListSubjects(ProjectClass project)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListSubjects(project, GetUser().accountID);
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListSubjects(project)", "InvalidOperationException");
                return new List<SubjectClass>();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListSubjects(project)");
                return new List<SubjectClass>();
            }
        }

        /// <summary>
        /// Retrieves all Qt outputs from the database.
        /// </summary>
        /// <returns>Returns the Qt outputs as a List.</returns>
        public async Task<List<OutputClass>?> ListQtOutputs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListQtOutputs();
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListQtOutputs()", "InvalidOperationException");
                return new List<OutputClass>();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListQtOutputs()");
                return new List<OutputClass>();
            }
        }

        /// <summary>
        /// Retrieves all Android Studio outputs from the database.
        /// </summary>
        /// <returns>Returns the AS outputs as a List.</returns>
        public async Task<List<OutputClass>?> ListASOutputs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListASOutputs();
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListASOutputs()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListASOutputs()");
            }

            return null;
        }

        /// <summary>
        /// Retrieves outputs from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of outputs.</param>
        /// <returns>Returns the outputs as a List.</returns>
        public async Task<List<OutputClass>?> ListOutputs(CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListOutputs(category);
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListOutputs(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListOutputs(category)");
            }

            return null;
        }



        /// <summary>
        /// Retrieves all Qt types from the database.
        /// </summary>
        /// <returns>Returns the Qt types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>?> ListQtTypes()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListQtTypes();
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListQtTypes()", "InvalidOperationException");
                return null;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListQtTypes()");
                return null;
            }
        }

        /// <summary>
        /// Retrieves all Android Studio types from the database.
        /// </summary>
        /// <returns>Returns the AS types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>?> ListASTypes()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListASTypes();
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListASTypes()", "InvalidOperationException");
                return null;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListASTypes()");
                return null;
            }
        }

        /// <summary>
        /// Retrieves types from the database of a specified category.
        /// </summary>
        /// <param name="category">The category of types to retrieve.</param>
        /// <returns>Returns the types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>?> ListTypes(CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                return await reader.ListTypes(category);
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "ListTypes(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "ListTypes(category)");
            }

            return null;
        }










        /// <summary>
        /// Executes a function with an EntityReader instance, managing scope and exceptions.
        /// </summary>
        private async Task<T> UseReaderAsync<T>(Func<EntityReader, Task<T>> func)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await func(reader);
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "UseReaderAsync<T>(func)", "InvalidOperationException");
                throw;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "UseReaderAsync<T>(func)");
                throw;
            }
        }

        /// <summary>
        /// Executes a function with an EntityWriter instance, managing scope and exceptions.
        /// </summary>
        private async Task<T> UseWriterAsync<T>(Func<EntityWriter, Task<T>> func)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await func(writer);
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "UseWriterAsync<T>(func)", "InvalidOperationException");
                throw;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "UseWriterAsync<T>(func)");
                throw;
            }
        }

        /// <summary>
        /// Executes a function with an EntityHandler instance, managing scope and exceptions.
        /// </summary>
        private async Task<T> UseHandlerAsync<T>(Func<EntityHandler, Task<T>> func)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var handler = scope.ServiceProvider.GetRequiredService<EntityHandler>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await func(handler);
            }
            catch (InvalidOperationException invex)
            {
                await writer.HandleExceptionAsync(invex, "UseHandlerAsync<T>(func)", "InvalidOperationException");
                throw;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "UseHandlerAsync<T>(func)");
                throw;
            }
        }



        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="email">The email provided by the user signing up.</param>
        /// <returns>Returns whether the email exists or not. Will throw an ExmailConflictException if the email exists.</returns>
        public Task<bool> EmailExists(string email)
        {
            return UseReaderAsync(reader => reader.EmailExists(email));
        }


        /// <summary>
        /// Finds an application by ID or name.
        /// </summary>
        /// <param name="ID">The application ID.</param>
        /// <param name="name">The application name.</param>
        /// <returns>Returns the ApplicationClass if found; otherwise null.</returns>
        public Task<ApplicationClass?> FindApplication(int ID, string name)
        {
            return UseReaderAsync(reader => reader.FindApplication(ID, name));
        }


        /// <summary>
        /// Finds an application by its ID.
        /// </summary>
        /// <param name="appID">The application ID.</param>
        /// <returns>Returns the ApplicationClass if found; otherwise null.</returns>
        public Task<ApplicationClass?> FindApplicationByID(int appID)
        {
            return UseReaderAsync(reader => reader.FindApplicationByID(appID));
        }

        /// <summary>
        /// Finds a project by user ID, project name, and application ID.
        /// </summary>
        /// <param name="userID">The user ID (nullable).</param>
        /// <param name="projectName">The project name.</param>
        /// <param name="appID">The application ID.</param>
        /// <returns>Returns the ProjectClass if found; otherwise null.</returns>
        public Task<ProjectClass?> FindProject(int? userID, string projectName, int appID)
        {
            return UseReaderAsync(reader => reader.FindProject(userID, projectName, appID));
        }

        /// <summary>
        /// Finds a project by its ID.
        /// </summary>
        /// <param name="projectID">The project ID.</param>
        /// <returns>Returns the ProjectClass if found; otherwise null.</returns>
        public Task<ProjectClass?> FindProjectByID(int projectID)
        {
            return UseReaderAsync(reader => reader.FindProjectByID(projectID));
        }



        /// <summary>
        /// Finds an output by its name.
        /// </summary>
        public Task<OutputClass?> FindOutput(string name)
        {
            return UseReaderAsync(reader => reader.FindOutput(name));
        }

        /// <summary>
        /// Finds an output by its ID.
        /// </summary>
        public Task<OutputClass?> FindOutputByID(int outputID)
        {
            return UseReaderAsync(reader => reader.FindOutputByID(outputID));
        }

        /// <summary>
        /// Finds a type by its name.
        /// </summary>
        public Task<TypeClass?> FindType(string name)
        {
            return UseReaderAsync(reader => reader.FindType(name));
        }

        /// <summary>
        /// Finds a type by its ID.
        /// </summary>
        public Task<TypeClass?> FindTypeByID(int typeID)
        {
            return UseReaderAsync(reader => reader.FindTypeByID(typeID));
        }

        /// <summary>
        /// Finds a subject by its name and category.
        /// </summary>
        public Task<SubjectClass?> FindSubject(string subject, LOG.CATEGORY category, int appID, int projectID)
        {
            return UseReaderAsync(reader => reader.FindSubject(subject, category, GetUser().accountID, appID, projectID));
        }

        /// <summary>
        /// Finds the ID of a subject.
        /// </summary>
        public Task<int> FindSubjectID(SubjectClass subject)
        {
            return UseReaderAsync(reader => reader.FindSubjectID(subject, GetUser().accountID));
        }














        #region Log Storage






        public void SaveLOG(LOG log, string filePath)
        {
            _cachemaster.SaveLog(log, filePath);
        }

        /// <summary>
        /// Inserts a log into the database.
        /// </summary>
        /// <param name="log">The log to insert.</param>
        /// <returns>Returns true if the log was successfully created; otherwise false.</returns>
        public Task<bool> CreateLOG(LOG log)
        {
            return UseWriterAsync(writer => writer.CreateLOG(log, GetUser().accountID));
        }






        #endregion




        #region Log Retrieval




        /// <summary>
        /// Retrieves all logs from the database.
        /// </summary>
        /// <returns>Returns a List of LOG objects, or null if an error occurs.</returns>
        public Task<List<LOG>?> RetrieveLogs(int userId)
        {
            return UseReaderAsync(reader => reader.RetrieveLogs(userId));
        }

        /// <summary>
        /// Retrieves logs from the database for a specific cache context.
        /// </summary>
        /// <param name="context">The cache context to filter logs.</param>
        /// <param name="id">The logged in user's id.</param>
        /// <returns>Returns an IEnumerable of LOG objects, or null if an error occurs.</returns>
        public Task<IEnumerable<LOG>?> RetrieveLogs(CacheContext context)
        {
            int id = GetUser().accountID;

            return UseReaderAsync<IEnumerable<LOG>?>(async reader =>
            {
                return context switch
                {
                    CacheContext.Qt => await reader.RetrieveQtCodingLogs(id),
                    CacheContext.AndroidStudio => await reader.RetrieveAndroidCodingLogs(id),
                    CacheContext.Coding => await reader.RetrieveCodingLogs(id),
                    CacheContext.Graphics => await reader.RetrieveGraphicsLogs(id),
                    CacheContext.Film => await reader.RetrieveFilmLogs(id),
                    CacheContext.Flexi => await reader.RetrieveFlexiNotesLogs(id),
                    _ => null
                };
            });
        }





        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The total count of logs, or 0 if an error occurs.</returns>
        public Task<int> LogCount()
        {
            return UseReaderAsync(reader => reader.LogCount());
        }

        /// <summary>
        /// Counts the number of logs in a specific category.
        /// </summary>
        /// <param name="category">The log category to count.</param>
        /// <returns>The count of logs in the specified category, or 0 if an error occurs.</returns>
        public Task<int?> LogCount(LOG.CATEGORY category)
        {
            return UseReaderAsync(reader => reader.LogCount(category, GetUser().accountID));
        }

        /// <summary>
        /// Counts the total number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs, or 0 if an error occurs.</returns>
        public Task<int> QtLogCount()
        {
            return UseReaderAsync(reader => reader.QtLogCount(GetUser().accountID));
        }

        /// <summary>
        /// Counts the total number of Android Studio logs.
        /// </summary>
        /// <returns>The count of AS logs, or 0 if an error occurs.</returns>
        public Task<int> ASLogCount()
        {
            return UseReaderAsync(reader => reader.ASLogCount(GetUser().accountID));
        }

        /// <summary>
        /// Counts the total number of Flexi Notes logs.
        /// </summary>
        /// <returns>The count of Flexi Notes logs, or 0 if an error occurs.</returns>
        public Task<int> FlexiLogCountAsync()
        {
            return UseReaderAsync(reader => reader.FlexiNotesLogCount(GetUser().accountID));
        }











        // ---------------- Qt Logs ----------------

        /// <summary>
        /// Searches Qt logs based on a search text.
        /// </summary>
        public Task<List<CodingLOG>?> SearchQtLogs(string searchBarText) =>
            UseReaderAsync(reader => reader.SearchQtLogs(searchBarText));

        /// <summary>
        /// Searches Qt logs within a specific project.
        /// </summary>
        public Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID) =>
            UseReaderAsync(reader => reader.SearchQtLogs(searchBarText, projectID));

        /// <summary>
        /// Searches Qt logs within a specific project and application.
        /// </summary>
        public Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID, int appID) =>
            UseReaderAsync(reader => reader.SearchQtLogs(searchBarText, projectID, appID));


        // ---------------- Coding Logs ----------------

        /// <summary>
        /// Searches coding logs based on a search text.
        /// </summary>
        public Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText) =>
            UseReaderAsync(reader => reader.SearchCodingLogs(searchBarText, GetUser().accountID));

        /// <summary>
        /// Searches coding logs within a specific project.
        /// </summary>
        public Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID) =>
            UseReaderAsync(reader => reader.SearchCodingLogs(searchBarText, projectID, GetUser().accountID));

        /// <summary>
        /// Searches coding logs within a specific project and application.
        /// </summary>
        public Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID, int appID) =>
            UseReaderAsync(reader => reader.SearchCodingLogs(searchBarText, GetUser().accountID, projectID, appID));





        #endregion




        #region Log Management




        #region Updates



        //public async Task<bool> UpdateQtLog(LOG lOG) => await _handler.UpdateQtLog(lOG);

        //public async Task<bool> UpdateNotesLog(NoteItem noteItem) => await _handler.UpdateNotesLog(noteItem);

        /// <summary>
        /// Saves all pending changes in the database through the reader.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await UseReaderAsync<Task>(async reader =>
            {
                await reader.SaveChangesAsync();

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Saves all pending changes on the current log to the database through the reader.
        /// </summary>
        /// <param name="log">The edited log</param>
        public async Task UpdateLogAsync(LOG log)
        {
            await UseHandlerAsync<Task>(async handler =>
            {
                await handler.UpdateLogAsync(log);

                return Task.CompletedTask;
            });
        }


        /// <summary>
        /// Updates the profile picture of a user in the database.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="filePath">The file path of the current profile picture</param>
        /// <returns>Returns true if the profile pic change succeeded, false otherwise.</returns>
        public Task<bool> UpdateProfilePicAsync(int id, string filePath)
        {
            return UseHandlerAsync(handler => handler.UpdateProfilePicAsync(id, filePath));
        }


        public Task<bool> UpdateUserAsync(UserSettings user)
        {
            return UseHandlerAsync(handler => handler.UpdateUserAsync(user));
        }


        /// <summary>
        /// Updates a user's password in the database based on the provided email and new password.
        /// </summary>
        /// <param name="newPassword">The user's new password</param>
        /// <param name="email">The user's email address</param>
        /// <returns>Returns true if the password was updated successfully, false otherwise.</returns>
        public Task<bool> UpdateUserPasswordAsync(string newPassword, string email)
        {
            return UseHandlerAsync(handler => handler.ChangePasswordAsync(newPassword, email));
        }








        #endregion






        #region Deleteions






        /// <summary>
        /// Deletes a log from the database.
        /// </summary>
        /// <param name="log">The log to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        public Task<bool> DeleteLOG(LOG log)
        {
            return UseHandlerAsync(handler => handler.DeleteLOG(log));
        }

        /// <summary>
        /// Deletes a log from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the log to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        public Task<bool> DeleteLOGByID(int ID) =>
            UseHandlerAsync(handler => handler.DeleteLOGByID(ID));

        /// <summary>
        /// Deletes a note from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the note to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        public Task<bool> DeleteNote(int ID) =>
            UseHandlerAsync(handler => handler.DeleteNote(ID));














        #endregion






        #endregion


        #region Cache Management




        public void DeleteCacheFile(string id, CacheContext cacheContext)
        {
            _cachemaster.DeleteViewModel(id, cacheContext);
        }






        #endregion




        #region Cache Retrieval




        /// <summary>
        /// Retrieves Qt cache. Qt cache are Qt logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Qt dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <returns>An observable collection of QtLOGViewModels</returns>
        public ObservableCollection<QtLOGViewModel> RetrieveQtCache(LogCacheViewModel logCacheViewModel, IDataService dataService)
        {
            return _cachemaster.LoadQtViewModels(logCacheViewModel, dataService, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves Android Studio cache. Android Studio cache are Android Studio logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Android Studio dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of AndroidLOGViewModels</returns>
        public ObservableCollection<AndroidLOGViewModel> RetrieveASCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadASViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves coding cache. Coding cache are coding logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The coding dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of CodeLOGViewModels</returns>
        public ObservableCollection<CodeLOGViewModel> RetrieveCodeCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadCodeViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves graphics cache. Graphics cache are graphics logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The graphics dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of GraphicsLOGViewModels</returns>
        public ObservableCollection<GraphicsLOGViewModel> RetrieveGraphicsCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadGraphicsViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves film cache. Film cache are film logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The film dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FilmLOGViewModels</returns>
        public ObservableCollection<FilmLOGViewModel> RetrieveFilmCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFilmViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves flexible cache. Flexible cache are flexible logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The flexible dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FlexiLOGViewModels</returns>
        public ObservableCollection<FlexiLOGViewModel> RetrieveFlexibleCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFlexiViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        #endregion







        #region Feedback Management



        public async Task HandleExceptionAsync(Exception exception, string methodName, string exceptionType = "Exception")
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


            await writer.HandleExceptionAsync(exception, methodName, exceptionType);
        }

        public async Task HandleExceptionAsync(string methodName, Exception ex, string messageBoxCaption, string exceptionType = "Exception")
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            await writer.HandleExceptionAsync(methodName, ex, messageBoxCaption, exceptionType: exceptionType);
        }



        #endregion

        }
}
