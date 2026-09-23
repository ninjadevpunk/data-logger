using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using FileSys.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static Core.Models.LOG;
using static FileSys.Services.CacheMaster;

namespace Data.Services
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
        /// Sets the currently online user to online.
        /// </summary>
        /// <param name="account">The account that is online.</param>
        /// <returns>Returns whether the account is set successfully.</returns>
        public async Task<bool> SetCurrentUser(ACCOUNT account) => await UseWriterAsync(writer => writer.SetCurrentUser(account));

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



        public async Task<bool> SignOutUser()
        {
            var account = _authService?.Account;
            if (account == null)
                return false;

            return await UseWriterAsync(writer => writer.UnsetCurrentUser(account));
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


        public ACCOUNT? GetUser()
        {
            return _authService.Account;
        }

        public string? GetAuthorName()
        {
            return _authService.Account?.FirstName + " " + _authService.Account?.LastName;
        }

        public CacheMaster GetCachemaster()
        {
            return _cachemaster;
        }

        public string? GetDisplayPic()
        {
            return _authService.Account?.ProfilePic;
        }


        private async Task<ACCOUNT> EnsureValidUserAsync()
        {
            var user = GetUser();
            if (user == null)
            {
                var ex = new InvalidOperationException("Cannot perform operation without a valid logged-in user.");

                await using var scope = _serviceProvider.CreateAsyncScope();
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

                await writer.HandleExceptionAsync(ex);

                throw ex;
            }

            return user;
        }


        /// <summary>
        /// Retrieves projects from the database and populates SQLITE_PROJECTS, skipping unnamed projects.
        /// </summary>
        public async Task InitialiseProjectsLISTAsync()
        {
            var user = await EnsureValidUserAsync();

            SQLITE_PROJECTS.Clear();

            await UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListProjects(user.accountID);

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
        public async Task InitialiseProjectsLISTAsync(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();

            SQLITE_PROJECTS.Clear();

            await UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListProjects(category, user.accountID);

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
        public async Task InitialiseApplicationsLISTAsync()
        {
            var user = await EnsureValidUserAsync();

            SQLITE_APPLICATIONS.Clear();

            await UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListApplications(user.accountID);

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
        public async Task InitialiseApplicationsLISTAsync(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();

            SQLITE_APPLICATIONS.Clear();

            await UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListApplications(category, user.accountID);

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
        public async Task InitialiseSubjectsLIST(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();

            SQLITE_SUBJECTS.Clear();

            await UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListSubjects(category, user.accountID);

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
        public async Task InitialiseSubjectsLIST(ProjectClass project)
        {
            var user = await EnsureValidUserAsync();

            SQLITE_SUBJECTS.Clear();

            await UseReaderAsync<Task>(async reader =>
            {
                var collection = await reader.ListSubjects(project, user.accountID);

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
        public async Task<List<SubjectClass>> ListSubjects(ProjectClass project)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

                try
                {
                    var user = await EnsureValidUserAsync();
                    var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                    return await reader.ListSubjects(project, user.accountID);
                }
                catch (InvalidOperationException invex)
                {
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<SubjectClass>();
        }

        /// <summary>
        /// Retrieves all Qt outputs from the database.
        /// </summary>
        /// <returns>Returns the Qt outputs as a List.</returns>
        public async Task<List<OutputClass>> ListQtOutputs()
        {
            try
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
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<OutputClass>();
        }

        /// <summary>
        /// Retrieves all Android Studio outputs from the database.
        /// </summary>
        /// <returns>Returns the AS outputs as a List.</returns>
        public async Task<List<OutputClass>> ListASOutputs()
        {
            try
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
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<OutputClass>();
        }

        /// <summary>
        /// Retrieves outputs from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of outputs.</param>
        /// <returns>Returns the outputs as a List.</returns>
        public async Task<List<OutputClass>> ListOutputs(CATEGORY category)
        {
            try
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
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<OutputClass>();
        }



        /// <summary>
        /// Retrieves all Qt types from the database.
        /// </summary>
        /// <returns>Returns the Qt types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>> ListQtTypes()
        {
            try
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
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<TypeClass>();
        }

        /// <summary>
        /// Retrieves all Android Studio types from the database.
        /// </summary>
        /// <returns>Returns the AS types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>> ListASTypes()
        {
            try
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
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<TypeClass>();
        }

        /// <summary>
        /// Retrieves types from the database of a specified category.
        /// </summary>
        /// <param name="category">The category of types to retrieve.</param>
        /// <returns>Returns the types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>> ListTypes(CATEGORY category)
        {
            try
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
                    await writer.HandleExceptionAsync(invex);
                }
                catch (Exception ex)
                {
                    await writer.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }

            return new List<TypeClass>();
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
                await writer.HandleExceptionAsync(invex);
                throw;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
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
                await writer.HandleExceptionAsync(invex);
                throw;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
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
                await writer.HandleExceptionAsync(invex);
                throw;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
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
        public async Task<SubjectClass?> FindSubject(string subject, LOG.CATEGORY category, int appID, int projectID)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.FindSubject(subject, category, user.accountID, appID, projectID));
        }

        /// <summary>
        /// Finds the ID of a subject.
        /// </summary>
        public async Task<int> FindSubjectID(SubjectClass subject)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.FindSubjectID(subject, user.accountID));
        }














        #region Log Storage & Inserts






        public void SaveLOG(LOG log, string filePath)
        {
            _cachemaster.SaveLog(log, filePath);
        }

        /// <summary>
        /// Inserts a log into the database.
        /// </summary>
        /// <param name="log">The log to insert.</param>
        /// <returns>Returns true if the log was successfully created; otherwise false.</returns>
        public async Task<bool> CreateLOG(LOG log)
        {
            var user = await EnsureValidUserAsync();
            return await UseWriterAsync(writer => writer.CreateLOG(log, user.accountID));
        }

        /// <summary>
        /// Adds the provided account to the database.
        /// </summary>
        /// <param name="account">The account to add to the database.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> AddAccount(ACCOUNT account)
        {
            return await UseWriterAsync(writer => writer.AddAccount(account));
        }





        #endregion




        #region Log Retrieval




        /// <summary>
        /// Retrieves all logs from the database.
        /// </summary>
        /// <returns>Returns a List of LOG objects, or null if an error occurs.</returns>
        public Task<List<LOG>> RetrieveLogs(int userId)
        {
            return UseReaderAsync(reader => reader.RetrieveLogs(userId));
        }

        /// <summary>
        /// Retrieves logs from the database for a specific cache context.
        /// </summary>
        /// <param name="context">The cache context to filter logs.</param>
        /// <param name="id">The logged in user's id.</param>
        /// <returns>Returns an IEnumerable of LOG objects, or null if an error occurs.</returns>
        public async Task<IEnumerable<LOG>?> RetrieveLogs(CacheContext context)
        {
            var user = await EnsureValidUserAsync();
            int id = user.accountID;

            return await UseReaderAsync<IEnumerable<LOG>>(async reader =>
            {
                return context switch
                {
                    CacheContext.Qt => await reader.RetrieveQtCodingLogs(id),
                    CacheContext.AndroidStudio => await reader.RetrieveAndroidCodingLogs(id),
                    CacheContext.Coding => await reader.RetrieveCodingLogs(id),
                    CacheContext.Graphics => await reader.RetrieveGraphicsLogs(id),
                    CacheContext.Film => await reader.RetrieveFilmLogs(id),
                    CacheContext.Flexi => await reader.RetrieveFlexiNotesLogs(id),
                    _ => Enumerable.Empty<LOG>()
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
        public async Task<int?> LogCount(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.LogCount(category, user.accountID));
        }

        /// <summary>
        /// Counts the total number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs, or 0 if an error occurs.</returns>
        public async Task<int> QtLogCount()
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.QtLogCount(user.accountID));
        }

        /// <summary>
        /// Counts the total number of Android Studio logs.
        /// </summary>
        /// <returns>The count of AS logs, or 0 if an error occurs.</returns>
        public async Task<int> ASLogCount()
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.ASLogCount(user.accountID));
        }

        /// <summary>
        /// Counts the total number of Flexi Notes logs.
        /// </summary>
        /// <returns>The count of Flexi Notes logs, or 0 if an error occurs.</returns>
        public async Task<int> FlexiLogCountAsync()
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.FlexiNotesLogCount(user.accountID));
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
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.SearchCodingLogs(searchBarText, user.accountID));
        }
        /// <summary>
        /// Searches coding logs within a specific project.
        /// </summary>
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.SearchCodingLogs(searchBarText, projectID, user.accountID));
        }

        /// <summary>
        /// Searches coding logs within a specific project and application.
        /// </summary>
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID, int appID)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.SearchCodingLogs(searchBarText, user.accountID, projectID, appID));
        }





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
        /// <param name="id">The user's id</param>
        /// <param name="filePath">The file path of the current profile picture</param>
        /// <returns>Returns true if the profile pic change succeeded, false otherwise.</returns>
        public Task<bool> UpdateProfilePicAsync(int id, string filePath)
        {
            return UseHandlerAsync(handler => handler.UpdateProfilePicAsync(id, filePath));
        }

        /// <summary>
        /// Updates a user's information in the database based on the provided UserSettings object.
        /// </summary>
        /// <param name="user">The user settings to update</param>
        /// <returns>Returns true if the user information was updated successfully, false otherwise.</returns>
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

        /// <summary>
        /// Deletes a user's account from the database using the provided account ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>Returns true if the account was deleted successfully, false otherwise.</returns>
        public Task<bool> DeleteAccountByID(int ID) =>
            UseHandlerAsync(handler => handler.DeleteAccountAsync(ID));














        #endregion






        #endregion


        #region Cache Management




        public void DeleteCacheFile(string id, CacheContext cacheContext)
        {
            _cachemaster.DeleteViewModel(id, cacheContext);
        }






        #endregion









        #region Feedback Management



        /// <summary>
        /// Handles a data error by creating an automatic exception feedback report.
        /// </summary>
        /// <param name="error">The data error containing the exception information.</param>
        /// <param name="methodName">The name of the method where the error occurred.</param>
        /// <returns>A task representing the asynchronous feedback creation operation.</returns>
        public async Task HandleExceptionAsync(DataError dataError, [CallerMemberName] string methodName = "")
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();


                await writer.HandleExceptionAsync(dataError, methodName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        /// <summary>
        /// Handles an exception by creating an automatic exception feedback report.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="methodName">The name of the method where the exception occurred.</param>
        /// <returns>A task representing the asynchronous feedback creation operation.</returns>
        public async Task HandleExceptionAsync(Exception exception, [CallerMemberName] string methodName = "")
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

                await writer.HandleExceptionAsync(exception, methodName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        #endregion

    }
}
