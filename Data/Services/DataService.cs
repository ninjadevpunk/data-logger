using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Identity;
using Core.DTOs.Notes;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static Core.Models.LOG;

namespace Data.Services
{
    public class DataService : IDataService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IAuthService _authService;



        public DataService(IAuthService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
        }
















        /// <summary>
        /// Signs in the current user by setting them as the current user in the database.
        /// </summary>
        public async Task SignInUserAsync()
        {
            var user = _authService.GetUser();

            await UseWriterAsync(writer => writer.SetCurrentUserAsync(user.UserId));
        }

        /// <summary>
        /// Sets the currently online user to online.
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <returns>Returns whether the user is set successfully.</returns>
        public async Task<bool> SetCurrentUserAsync(int userId) => await UseWriterAsync(writer => writer.SetCurrentUserAsync(userId));


        /// <summary>
        /// Signs in the current user by setting them as the current user in the database.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Returns a tuple: (success flag, a UserDTO if sign-in succeeded Otherwise null is returned).</returns>
        public async Task<(bool Success, UserDTO? Account)> SignInUserAsync(string email, string password)
        {
            var temporaryAccount = await UseReaderAsync(reader => reader.FindAccountByEmailAsync(email, password));

            if (temporaryAccount == null)
                return (false, null);

            int id = temporaryAccount.UserId;

            var result = await UseWriterAsync(writer => writer.SetCurrentUserAsync(id));

            return (result, temporaryAccount);
        }



        public async Task<bool> SignOutUserAsync()
        {
            var user = _authService.GetUser();
            if (user == null)
                return false;

            return await UseWriterAsync(writer => writer.UnsetCurrentUserAsync(user.UserId));
        }


        

        public async Task<string> UpdateProfilePicAsync(string emailAddress)
        {
            return await UseReaderAsync(reader => reader.RetrieveProfilePic(emailAddress));
        }




        private async Task<UserDTO> EnsureValidUserAsync()
        {
            var user = _authService.GetUser();

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
        public async Task<IReadOnlyList<string>> InitialiseProjectsLISTAsync()
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_PROJECTS = [];

            return await UseReaderAsync(async reader =>
            {
                var collection = await reader.ListProjects(user.UserId);

                foreach (var pro in collection)
                {
                    if (pro != null)
                        SQLITE_PROJECTS.Add(pro.Name);
                }

                return SQLITE_PROJECTS;
            });
        }






        /// <summary>
        /// Retrieves projects from the database of a specified category and populates SQLITE_PROJECTS, skipping unnamed projects.
        /// </summary>
        /// <param name="appName">The name of the app to filter by</param>
        public async Task<IReadOnlyList<string>> InitialiseProjectsLISTAsync(string appName)
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_PROJECTS = [];

            return await UseReaderAsync<IReadOnlyList<string>>(async reader =>
            {
                var collection = await reader.ListProjects(appName, user.UserId);

                foreach (var pro in collection)
                {
                    if (pro != null)
                        SQLITE_PROJECTS.Add(pro.Name);
                }

                return SQLITE_PROJECTS;
            });
        }


        /// <summary>
        /// Retrieves projects from the database of a specified category and populates SQLITE_PROJECTS, skipping unnamed projects.
        /// </summary>
        /// <param name="category">The type of project.</param>
        public async Task<IReadOnlyList<string>> InitialiseProjectsLISTAsync(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_PROJECTS = [];

            return await UseReaderAsync<IReadOnlyList<string>>(async reader =>
            {
                var collection = await reader.ListProjects(category, user.UserId);

                foreach (var pro in collection)
                {
                    if (pro != null)
                        SQLITE_PROJECTS.Add(pro.Name);
                }

                return SQLITE_PROJECTS;
            });
        }





        /// <summary>
        /// Retrieves applications from the database and populates SQLITE_APPLICATIONS.
        /// </summary>
        public async Task<IReadOnlyList<string>> InitialiseApplicationsLISTAsync()
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_APPLICATIONS = [];

            return await UseReaderAsync<IReadOnlyList<string>>(async reader =>
            {
                var collection = await reader.ListApplications(user.UserId);

                foreach (var app in collection)
                {
                    if (app != null)
                        SQLITE_APPLICATIONS.Add(app.Name);
                }

                return SQLITE_APPLICATIONS;
            });
        }




        /// <summary>
        /// Retrieves applications from the database of a specified category and populates SQLITE_APPLICATIONS.
        /// </summary>
        /// <param name="category">The type of application.</param>
        public async Task<IReadOnlyList<string>> InitialiseApplicationsLISTAsync(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_APPLICATIONS = [];

            return await UseReaderAsync<IReadOnlyList<string>>(async reader =>
            {
                var collection = await reader.ListApplications(category, user.UserId);

                foreach (var app in collection)
                {
                    if (app != null)
                        SQLITE_APPLICATIONS.Add(app.Name);
                }

                return SQLITE_APPLICATIONS;
            });
        }




        /// <summary>
        /// Retrieves subjects from the database of a specified category and populates SQLITE_SUBJECTS, skipping "No Subject".
        /// </summary>
        /// <param name="category">The type of subjects being retrieved.</param>
        public async Task<IReadOnlyList<string>> InitialiseSubjectsLISTAsync(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_SUBJECTS = [];

            return await UseReaderAsync<IReadOnlyList<string>>(async reader =>
            {
                var collection = await reader.ListSubjects(category, user.UserId);

                foreach (var subject in collection)
                {
                    if (subject != null && subject.Subject != "No Subject")
                        SQLITE_SUBJECTS.Add(subject.Subject);
                }

                return SQLITE_SUBJECTS;
            });
        }



        /// <summary>
        /// Retrieves subjects from the database for a specific project and populates SQLITE_SUBJECTS.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved only.</param>
        public async Task<IReadOnlyList<string>> InitialiseSubjectsLISTAsync(ProjectClass project)
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_SUBJECTS = [];

            return await UseReaderAsync<IReadOnlyList<string>>(async reader =>
            {
                var collection = await reader.ListSubjects(project, user.UserId);

                foreach (var subject in collection)
                {
                    if (subject != null)
                        SQLITE_SUBJECTS.Add(subject.Subject);
                }

                return SQLITE_SUBJECTS;
            });
        }

        /// <summary>
        /// Retrieves subjects from the database for a specific project and populates SQLITE_SUBJECTS.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved only.</param>
        public async Task<IReadOnlyList<string>> InitialiseSubjectsLISTAsync(string appName, string projectName)
        {
            var user = await EnsureValidUserAsync();

            List<string> SQLITE_SUBJECTS = [];

            var project = await FindProjectAsync(user.UserId, projectName, appName);

            return await UseReaderAsync(async reader =>
            {
                if (project == null)
                    return [];

                var collection = await reader.ListSubjects(project, user.UserId);

                foreach (var subject in collection)
                {
                    if (subject != null)
                        SQLITE_SUBJECTS.Add(subject.Subject);
                }

                return SQLITE_SUBJECTS;
            });
        }








        /// <summary>
        /// Retrieves subjects from the database. Used every time the subjects are needed as a list immediately.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved ONLY.</param>
        /// <returns>Returns the subjects as a List.</returns>
        public async Task<IEnumerable<SubjectClass>> ListSubjects(ProjectClass project)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

                try
                {
                    var user = await EnsureValidUserAsync();
                    var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();
                    return await reader.ListSubjects(project, user.UserId);
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
        public async Task<IEnumerable<string>> ListQtOutputsAsync()
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

            return [];
        }

        /// <summary>
        /// Retrieves all Android Studio outputs from the database.
        /// </summary>
        /// <returns>Returns the AS outputs as a List.</returns>
        public async Task<IEnumerable<string>> ListASOutputsAsync()
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

            return [];
        }

        /// <summary>
        /// Retrieves outputs from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of outputs.</param>
        /// <returns>Returns the outputs as a List.</returns>
        public async Task<IEnumerable<string>> ListOutputsAsync(CATEGORY category)
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

            return [];
        }



        /// <summary>
        /// Retrieves all Qt types from the database.
        /// </summary>
        /// <returns>Returns the Qt types as a List, or null if an error occurs.</returns>
        public async Task<IEnumerable<string>> ListQtTypesAsync()
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

            return [];
        }

        /// <summary>
        /// Retrieves all Android Studio types from the database.
        /// </summary>
        /// <returns>Returns the AS types as a List, or null if an error occurs.</returns>
        public async Task<IEnumerable<string>> ListASTypesAsync()
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

            return [];
        }

        /// <summary>
        /// Retrieves types from the database of a specified category.
        /// </summary>
        /// <param name="category">The category of types to retrieve.</param>
        /// <returns>Returns the types as a List, or null if an error occurs.</returns>
        public async Task<IEnumerable<string>> ListTypesAsync(CATEGORY category)
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

            return [];
        }










        /// <summary>
        /// Executes a function with an EntityReader instance, managing scope and exceptions.
        /// </summary>
        private async Task<T> UseReaderAsync<T>(Func<EntityReader, Task<T>> func)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in UseReaderAsync:{ex.Message}");
            }

#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
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
        public Task<bool> EmailExistsAsync(string email)
        {
            return UseReaderAsync(reader => reader.EmailExists(email));
        }


        /// <summary>
        /// Finds an application by ID or name.
        /// </summary>
        /// <param name="ID">The application ID.</param>
        /// <param name="name">The application name.</param>
        /// <returns>Returns the ApplicationClass if found; otherwise null.</returns>
        public Task<ApplicationClass?> FindApplicationAsync(int ID, string name)
        {
            return UseReaderAsync(reader => reader.FindApplication(ID, name));
        }


        /// <summary>
        /// Finds an application by its ID.
        /// </summary>
        /// <param name="appID">The application ID.</param>
        /// <returns>Returns the ApplicationClass if found; otherwise null.</returns>
        public Task<ApplicationClass?> FindApplicationByIDAsync(int appID)
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
        public Task<ProjectClass?> FindProjectAsync(int? userID, string projectName, int appID)
        {
            return UseReaderAsync(reader => reader.FindProjectAsync(userID, projectName, appID));
        }

        /// <summary> 
        /// Finds a project based on project name and app name. Throws if a user is not logged in.
        /// </summary>
        /// <param name="userID">The logged in user</param>
        /// <param name="projectName">The project's name</param>
        /// <param name="appName">The app's name</param>
        /// <returns>A project object from the database</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<ProjectClass?> FindProjectAsync(int? userID, string projectName, string appName) 
            => UseReaderAsync(reader => reader.FindProjectAsync(userID, projectName, appName));

        /// <summary>
        /// Finds a project by its ID.
        /// </summary>
        /// <param name="projectID">The project ID.</param>
        /// <returns>Returns the ProjectClass if found; otherwise null.</returns>
        public Task<ProjectClass?> FindProjectByIDAsync(int projectID)
        {
            return UseReaderAsync(reader => reader.FindProjectByID(projectID));
        }



        /// <summary>
        /// Finds an output by its name.
        /// </summary>
        public Task<OutputClass?> FindOutputAsync(string name)
        {
            return UseReaderAsync(reader => reader.FindOutput(name));
        }

        /// <summary>
        /// Finds an output by its ID.
        /// </summary>
        public Task<OutputClass?> FindOutputByIDAsync(int outputID)
        {
            return UseReaderAsync(reader => reader.FindOutputByID(outputID));
        }

        /// <summary>
        /// Finds a type by its name.
        /// </summary>
        public Task<TypeClass?> FindTypeAsync(string name)
        {
            return UseReaderAsync(reader => reader.FindType(name));
        }

        /// <summary>
        /// Finds a type by its ID.
        /// </summary>
        public Task<TypeClass?> FindTypeByIDAsync(int typeID)
        {
            return UseReaderAsync(reader => reader.FindTypeByID(typeID));
        }

        /// <summary>
        /// Finds a subject by its name and category.
        /// </summary>
        public async Task<SubjectClass?> FindSubjectAsync(string subject, LOG.CATEGORY category, int appID, int projectID)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.FindSubject(subject, category, user.UserId, appID, projectID));
        }

        /// <summary>
        /// Finds the ID of a subject.
        /// </summary>
        public async Task<int> FindSubjectIDAsync(SubjectClass subject)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.FindSubjectID(subject, user.UserId));
        }














        #region Log Storage & Inserts






        /// <summary>
        /// Inserts a logCacheDTO into the database.
        /// </summary>
        /// <param name="logCacheDTO">The logCacheDTO to insert.</param>
        /// <returns>Returns true if the logCacheDTO was successfully created; otherwise false.</returns>
        public async Task<bool> CreateLOGAsync(LogCacheDTO logCacheDTO)
        {
            var user = await EnsureValidUserAsync();
            return await UseWriterAsync(writer => writer.CreateLOGAsync(logCacheDTO, user.UserId));
        }

        /// <summary>
        /// Inserts a note DTO into the database.
        /// </summary>
        /// <param name="noteDTO">The note DTO to insert.</param>
        /// <returns>Returns a tuple: (success flag, the created note data DTO).</returns>
        public async Task<(bool Success, NoteDataDTO? NoteDataDTO)> CreateNoteAsync(NoteDTO noteDTO)
        {
            var user = await EnsureValidUserAsync();
            return await UseWriterAsync(writer => writer.CreateNoteAsync(noteDTO, user.UserId));
        }

        /// <summary>
        /// Inserts a checklist DTO into the database.
        /// </summary>
        /// <param name="checkListDTO">The checklist DTO to insert.</param>
        /// <returns>Returns a tuple: (success flag, the created checklist data DTO).</returns>
        public async Task<(bool Success, CheckListDataDTO? CheckListDataDTO)> CreateCheckListAsync(CheckListDTO checkListDTO)
        {
            var user = await EnsureValidUserAsync();
            return await UseWriterAsync(writer => writer.CreateCheckListAsync(checkListDTO, user.UserId));
        }

        /// <summary>
        /// Adds the provided user to the database.
        /// </summary>
        /// <param name="account">The user to add to the database.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<(bool, int)> AddAccountAsync(ACCOUNT account)
        {
            return await UseWriterAsync(writer => writer.AddAccount(account));
        }





        #endregion




        #region Log Retrieval





        /// <summary>
        /// Retrieves logs from the database for a specific cache context.
        /// </summary>
        /// <param name="context">The logCacheDTO context to filter logs.</param>
        /// <returns>Returns an IEnumerable of LOG objects, or null if an error occurs.</returns>
        public async Task<IEnumerable<LOG>?> RetrieveLogsAsync(LogContext context)
        {
            var user = await EnsureValidUserAsync();
            int id = user.UserId;

            return await UseReaderAsync<IEnumerable<LOG>>(async reader =>
            {
                return context switch
                {
                    LogContext.Qt => await reader.RetrieveQtCodingLogsAsync(id),
                    LogContext.AndroidStudio => await reader.RetrieveAndroidCodingLogsAsync(id),
                    LogContext.Coding => await reader.RetrieveCodingLogsAsync(id),
                    LogContext.Graphics => await reader.RetrieveGraphicsLogsAsync(id),
                    LogContext.Film => await reader.RetrieveFilmLogsAsync(id),
                    LogContext.Flexi => await reader.RetrieveFlexiNotesLogsAsync(id),
                    _ => Enumerable.Empty<LOG>()
                };
            });

        }

        /// <summary>
        /// Retrieves notes from the database that belong to a specific user.
        /// </summary>
        /// <param name="id">The logged in user's id.</param>
        /// <returns>Returns an IEnumerable of note items, or null if an error occurs.</returns>
        public async Task<IEnumerable<NoteItem>?> RetrieveGenericNotesAsync(int id)
        {
            return await UseReaderAsync<IEnumerable<NoteItem>>(async reader => await reader.RetrieveGenericNotesAsync(id));
        }





        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The total count of logs, or 0 if an error occurs.</returns>
        public Task<int> LogCountAsync()
        {
            return UseReaderAsync(reader => reader.LogCount());
        }

        /// <summary>
        /// Counts the number of logs in a specific category.
        /// </summary>
        /// <param name="category">The logCacheDTO category to count.</param>
        /// <returns>The count of logs in the specified category, or 0 if an error occurs.</returns>
        public async Task<int> LogCountAsync(LOG.CATEGORY category)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.LogCountAsync(category, user.UserId));
        }

        /// <summary>
        /// Counts the total number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs, or 0 if an error occurs.</returns>
        public async Task<int> QtLogCountAsync()
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.QtLogCountAsync(user.UserId));
        }

        /// <summary>
        /// Counts the total number of Android Studio logs.
        /// </summary>
        /// <returns>The count of AS logs, or 0 if an error occurs.</returns>
        public async Task<int> ASLogCountAsync()
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.ASLogCountAsync(user.UserId));
        }

        /// <summary>
        /// Counts the total number of Flexi Notes logs.
        /// </summary>
        /// <returns>The count of Flexi Notes logs, or 0 if an error occurs.</returns>
        public async Task<int> FlexiLogCountAsync()
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.FlexiNotesLogCountAsync(user.UserId));
        }











        // ---------------- Qt Logs ----------------

        /// <summary>
        /// Searches Qt logs based on a search text.
        /// </summary>
        public Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText) =>
            UseReaderAsync(reader => reader.SearchQtLogsAsync(searchBarText));

        /// <summary>
        /// Searches Qt logs within a specific project.
        /// </summary>
        public Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID) =>
            UseReaderAsync(reader => reader.SearchQtLogsAsync(searchBarText, projectID));

        /// <summary>
        /// Searches Qt logs within a specific project and application.
        /// </summary>
        public Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText, int projectID, int appID) =>
            UseReaderAsync(reader => reader.SearchQtLogsAsync(searchBarText, projectID, appID));


        // ---------------- Coding Logs ----------------

        /// <summary>
        /// Searches coding logs based on a search text.
        /// </summary>
        public async Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.SearchCodingLogsAsync(searchBarText, user.UserId));
        }
        /// <summary>
        /// Searches coding logs within a specific project.
        /// </summary>
        public async Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int projectID)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.SearchCodingLogsAsync(searchBarText, projectID, user.UserId));
        }

        /// <summary>
        /// Searches coding logs within a specific project and application.
        /// </summary>
        public async Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int projectID, int appID)
        {
            var user = await EnsureValidUserAsync();
            return await UseReaderAsync(reader => reader.SearchCodingLogsAsync(searchBarText, user.UserId, projectID, appID));
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
        /// Saves all pending changes on the current logCacheDTO to the database through the reader.
        /// </summary>
        /// <param name="log">The edited logCacheDTO</param>
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
        /// Deletes a logCacheDTO from the database.
        /// </summary>
        /// <param name="log">The logCacheDTO to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        public Task<bool> DeleteLOGAsync(LOG log)
        {
            return UseHandlerAsync(handler => handler.DeleteLOGAsync(log));
        }

        /// <summary>
        /// Deletes a logCacheDTO from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the logCacheDTO to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        public Task<bool> DeleteLOGByIDAsync(int ID) =>
            UseHandlerAsync(handler => handler.DeleteLOGByIDAsync(ID));

        /// <summary>
        /// Deletes a note from the database by its ID.
        /// </summary>
        /// <param name="ID">The ID of the note to delete.</param>
        /// <returns>True if the deletion succeeded, false otherwise.</returns>
        public Task<bool> DeleteNoteAsync(int ID) =>
            UseHandlerAsync(handler => handler.DeleteNoteAsync(ID));

        /// <summary>
        /// Deletes a user's user from the database using the provided user ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Returns true if the user was deleted successfully, false otherwise.</returns>
        public Task<bool> DeleteAccountByIDAsync(int ID) =>
            UseHandlerAsync(handler => handler.DeleteAccountAsync(ID));














        #endregion






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
