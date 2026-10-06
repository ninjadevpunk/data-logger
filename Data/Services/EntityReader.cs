using Core.DTOs.Identity;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using static Core.Models.FeedbackMessage;
using static Core.Models.LOG;

namespace Data.Services
{
    internal class EntityReader
    {
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// The Entity Framework data reader for EntityMaster.
        /// </summary>
        public EntityReader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<AsyncServiceScope> SaveChangesAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            await master.SaveChangesAsync();

            return scope;
        }

        public async Task<AsyncServiceScope> SaveChangesAsync(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            if (!master.Logs.Local.Contains(log))
            {
                master.Attach(log);
            }

            await master.SaveChangesAsync();

            return scope;
        }

        /// <summary>
        /// Retrieve a profile pic from on an account with the matching email address.
        /// </summary>
        /// <param name="emailAddress">The email of the account in the database.</param>
        /// <returns>Returns a profile pic file path or an empty string if the path is null.</returns>
        public async Task<string> RetrieveProfilePic(string emailAddress)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.Accounts
                .Where(a => a.Email == emailAddress)
                .Select(a => a.ProfilePic)
                .FirstOrDefaultAsync() ?? string.Empty;
        }



        /// <summary>
        /// Retrieves a single log from the database.
        /// </summary>
        /// <param name="id">The ID of the log.</param>
        /// <returns>Returns only 1 log with the specified ID.</returns>
        public async Task<LOG?> RetrieveLog(int id, int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.Logs
                .Where(l => l.accountID == userId && l.ID == id)
                .SingleOrDefaultAsync();
        }


        public async Task<List<LOG>> RetrieveLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.Logs
                .Where(l => l.accountID == userId)
                .OrderBy(l => l.Start)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves Coding logs.
        /// </summary>
        /// <returns>Returns a list of CodingLOG objects.</returns>
        public async Task<List<CodingLOG>> RetrieveCodingLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.CodingLogs
                .Where(c => c.accountID == userId && !new[] { 1, 2 }.Contains(c.Application!.appID))
                .Include(l => l.Author)
                .Include(l => l.Project)
                .Include(l => l.Application)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                .ThenInclude(p => p.Subject)
                .OrderBy(c => c.Start)
                .ToListAsync();
        }

        public async Task<List<CodingLOG>> RetrieveQtCodingLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.CodingLogs
                .Where(qt => qt.accountID == userId && qt.Application!.appID == 1)
                .OrderBy(qt => qt.Start)
                .ToListAsync();
        }

        public async Task<List<AndroidCodingLOG>> RetrieveAndroidCodingLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.AndroidCodingLogs
                .Where(a => a.accountID == userId && a.Application!.appID == 2)
                .OrderBy(a => a.Start)
                .ToListAsync();
        }

        public async Task<List<GraphicsLOG>> RetrieveGraphicsLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.GraphicsLogs
                .Where(g => g.accountID == userId)
                .OrderBy(g => g.Start)
                .ToListAsync();
        }

        public async Task<List<FilmLOG>> RetrieveFilmLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.FilmLogs
                .Where(f => f.accountID == userId)
                .OrderBy(f => f.Start)
                .ToListAsync();
        }

        public async Task<List<FlexiNotesLOG>> RetrieveFlexiNotesLogsAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.FlexiNotesLogs
                .Where(fn => fn.accountID == userId)
                .OrderBy(fn => fn.Start)
                .ToListAsync();
        }


        public async Task<List<NoteItem>> RetrieveGenericNotesAsync(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.NoteItems
                .Where(n => n.accountID == id)
                .ToListAsync();
        }



        /// <summary>
        /// Retrieves notes from the database that belong to a specific user.
        /// </summary>
        /// <param name="id">The logged in user's id.</param>
        /// <returns>Returns an IEnumerable of note items, or null if an error occurs.</returns>
        public async Task<List<CheckList>> RetrieveCheckList(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.Checklists
                .Where((cl => cl.accountID == userId))
                .ToListAsync();
        }

        /// <summary>
        /// Find the account ID of the user who's online.
        /// </summary>
        /// <returns>Returns a single account ID</returns>
        public async Task<int> GetOnlineAccountIDAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var onlineAccount = await master.Accounts
                    .SingleAsync(a => a.accountID == userId);

                return onlineAccount.accountID;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return 0;
        }

        /// <summary>
        /// Find the account ID of the user who's online.
        /// </summary>
        /// <returns>Returns a single account ID</returns>
        public static async Task<int?> GetOnlineAccountIDAsync(AsyncServiceScope scope, EntityMaster master, int userId)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var onlineAccount = await master.Accounts
                    .SingleAsync(a => a.accountID == userId);

                return onlineAccount?.accountID;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }


        public async Task<ACCOUNT?> FindAccountByID(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == id);
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception near FindAccountByID(id): {ex.Message}");

                return null;
            }
        }

        public static async Task<ACCOUNT?> FindAccountByID(AsyncServiceScope scope, EntityMaster master, int id)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == id);
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception near FindAccountByID(id): {ex.Message}");

                return null;
            }
        }

        public async Task<UserDTO?> FindAccountByEmailAsync(string emailAddress, string password)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var account = await master.Accounts
                    .SingleAsync(a => a.Email == emailAddress);

                if (account != null && VerifyPassword(account, password))
                {
                    return new UserDTO
                    {
                        UserId = account.accountID,
                        ProfilePic = account.ProfilePic,
                        Name = account.FirstName,
                        Surname = account.LastName,
                        Email = account.Email,
                        IsEmployee = account.IsEmployee,
                        CompanyName = account.CompanyName,
                        CompanyAddress = account.CompanyAddress,
                        CompanyLogo = account.CompanyLogo,
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception near FindAccountByEmailAsync(email,password): {ex.Message}");

                return null;
            }
        }

        public async Task<ApplicationClass?> FindApplication(string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var application = await master.Applications
                    .FirstOrDefaultAsync(app => app.Name == name);

                return application;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public async Task<ApplicationClass?> FindApplication(int? ID, string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                if (ID == null)
                    throw new ArgumentNullException(nameof(ID), "ID is needed to find application's from Data Logger or user!");

                var application = await master.Applications
                    .Where(app => app.accountID == ID || app.accountID == 1)
                    .FirstOrDefaultAsync(app => app.Name == name);

                return application;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<ApplicationClass?> FindApplication(AsyncServiceScope scope, EntityMaster master, int? ID, string name)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                if (ID == null)
                    throw new ArgumentNullException(nameof(ID), "ID is needed to find application's from Data Logger or user!");

                var application = await master.Applications
                    .Where(app => (app.accountID == ID || app.accountID == 1) && app.Name == name)
                    .FirstOrDefaultAsync();

                return application;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }


        public async Task<ApplicationClass?> FindApplicationByID(int appID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var application = await master.Applications
                    .FirstOrDefaultAsync(app => app.appID == appID);

                return application;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public async Task<ProjectClass?> FindProjectAsync(int? userID, string projectName, int appID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                if (userID == null)
                    throw new ArgumentNullException(nameof(userID), "userID is required to find a user's projects!");

                var project = await master.Projects
                    .Where(p => (p.accountID == userID || p.accountID == 1)
                                && p.Name == projectName
                                && p.appID == appID)
                    .FirstOrDefaultAsync();

                return project;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }


        public async Task<ProjectClass?> FindProjectAsync(int? userID, string projectName, string appName)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                if (userID == null)
                    throw new ArgumentNullException(nameof(userID), "userID is required to find a user's projects!");

                var project = await master.Projects
                    .Where(p => (p.accountID == userID || p.accountID == 1)
                                && p.Name == projectName
                                && p.Application.Name == appName)
                    .FirstOrDefaultAsync();

                return project;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<ProjectClass?> FindProject(AsyncServiceScope scope, EntityMaster master, int? userID, string projectName, int appID)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {

                var project = await master.Projects
                    .Where(p => (p.accountID == userID || p.accountID == 1)
                                && p.Name == projectName
                                && p.appID == appID)
                    .FirstOrDefaultAsync();

                return project;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }


        public async Task<ProjectClass?> FindProjectByID(int projectID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var project = await master.Projects
                    .FirstOrDefaultAsync((pro => pro.projectID == projectID));

                return project;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public async Task<OutputClass?> FindOutput(string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Outputs
                    .Where(o => o.accountID == 1 && o.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<OutputClass?> FindOutput(AsyncServiceScope scope, string name, LOG.CATEGORY category = LOG.CATEGORY.CODING)
        {
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Outputs
                    .Where(o => o.accountID == 1 && o.Name == name)
                    .Where(o => o.Category == category)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<OutputClass?> FindOutput(AsyncServiceScope scope, EntityMaster master, string name)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Outputs
                    .Where(o => o.accountID == 1 && o.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public async Task<OutputClass?> FindOutputByID(int outputID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var output = await master.Outputs
                    .FirstOrDefaultAsync(output => output.outputID == outputID);

                return output;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public async Task<TypeClass?> FindType(string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Types
                    .Where(t => t.accountID == 1 && t.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<TypeClass?> FindType(AsyncServiceScope scope, string name, LOG.CATEGORY category = LOG.CATEGORY.CODING)
        {
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Types
                    .Where(t => t.accountID == 1 && t.Name == name)
                    .Where(t => t.Category == category)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<TypeClass?> FindType(AsyncServiceScope scope, EntityMaster master, string name)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Types
                    .Where(t => t.accountID == 1 && t.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public async Task<TypeClass?> FindTypeByID(int typeID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var type = await master.Types
                    .FirstOrDefaultAsync(type => type.typeID == typeID);

                return type;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }





        private static bool VerifyPassword(ACCOUNT account, string password)
        {
            try
            {
                string hashedPassword = EntityWriter.SaltedSHA256Hash(password, account.accountID.ToString());
                return hashedPassword == account.Password;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near VerifyPassword(account,password): {ex.Message}");

                // TODO

            }

            return false;
        }

        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="email">The email provided by the user signing up.</param>
        /// <returns>Returns whether the email exists or not. Will throw an ExmailConflictException if the email exists.</returns>
        /// <exception cref="EmailConflictException">An exception thrown when the email already exists.</exception>
        public async Task<bool> EmailExists(string email)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var account = await master.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);

                if (account is not null)
                {
                    throw new EmailConflictException("This email already exists.");
                }

                return false;
            }
            catch (EmailConflictException mailex)
            {
                await writer.HandleExceptionAsync(mailex);

                return true;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);

                return true;
            }
        }

        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="scope">The scope for the DbContext.</param>
        /// <param name="email">The email provided by the user signing up.</param>
        /// <returns>Returns whether the email exists or not. Will throw an ExmailConflictException if the email exists.</returns>
        /// <exception cref="EmailConflictException"></exception>
        public static async Task<bool> EmailExists(AsyncServiceScope scope, string email)
        {
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var account = await master.Accounts
                    .FirstOrDefaultAsync(a => a.Email == email);

                if (account is not null)
                {
                    throw new EmailConflictException("This email already exists.");
                }

                return false;
            }
            catch (EmailConflictException mailex)
            {
                await writer.HandleExceptionAsync(mailex);

                return true;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);

                return true;
            }
        }

        /// <summary>
        /// Checks if a log with a given logID has PostIts in the PostIt table.
        /// </summary>
        /// <param name="logID">The log ID of the log that is being updated.</param>
        /// <returns>Returns whether the log with the specified logID has PostIts.</returns>
        public async Task<bool> PostItsExists(int logID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            return await master.PostIts.AnyAsync(p =>
                    p.logID == logID);
        }



        /// <summary>
        /// Finds an account's ID.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Returns an account ID.</returns>
        public async Task<int> FindAccountIDAsync(ACCOUNT account)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var result = await master.Accounts
                    .Where(a => a.Email == account.Email)
                    .Select(a => a.accountID)
                    .FirstOrDefaultAsync();

                return result != 0 ? result : -1;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception near FindAccountID(account): {ex.Message}");

                return -1;
            }
        }



        /// <summary>
        /// Will find an ApplicationClass's ID.
        /// </summary>
        /// <param name="app">The application being searched.</param>
        /// <returns>An app ID if found; -1 if not.</returns>
        public async Task<int> FindAppID(ApplicationClass app)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var appID = await master.Applications
                    .Where(a => a.Name == app.Name
                                && a.Category == app.Category
                                && (a.User!.accountID == 1 || a.User.accountID == app.User!.accountID))
                    .Select(a => a.appID)
                    .FirstOrDefaultAsync();

                return appID != 0 ? appID : -1;
            }
            catch (ArgumentNullException nullex)
            {
                await writer.HandleExceptionAsync(nullex);
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return -1;
        }


        /// <summary>
        /// Finds a project's ID.
        /// </summary>
        /// <param name="project">The project being searched.</param>
        /// <returns>Returns an ID for the given project. Returns 1 (Unknown) if not found.</returns>
        public async Task<int> FindProjectID(ProjectClass project, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            if (string.IsNullOrEmpty(project.Name) || project.Name == "Unknown")
                return 1;


            try
            {
                if (project.Application == null)
                    return 1;

                // Retrieve APPLICATION ID
                int appID = await FindAppID(project.Application);

                // If application ID is not 3, perform search and add project if not found
                if (appID != 3)
                {
                    var projectID = await master.Projects
                        .Where(p => p.Name == project.Name
                        && p.Category == project.Category
                        && p.Application != null
                        && p.Application.appID == project.Application.appID
                        && p.User != null
                        && (p.User.accountID == 1 || p.User.accountID == id))
                        .Select(p => p.projectID)
                        .FirstOrDefaultAsync();

                    if (projectID != 0)
                        return projectID;
                }

            }
            catch (OperationCanceledException opex)
            {
                await writer.HandleExceptionAsync(opex);
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return 1;
        }



        public async Task<SubjectClass?> FindSubject(string name, LOG.CATEGORY category, int id, int appID, int projectID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Subjects
                    .Where(s => new[] { 1, id }.Contains(s.accountID))
                    .Where(s => s.Subject == name)
                    .Where(s => s.Category == category)
                    .Where(s => s.projectID == projectID)
                    .Where(s => s.appID == appID)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }

        public static async Task<SubjectClass?> FindSubject(AsyncServiceScope scope, EntityMaster master, string name, LOG.CATEGORY category,
            int userId, int appID, int projectID)
        {
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Subjects
                    .Where(s => new[] { 1, userId }.Contains(s.accountID))
                    .Where(s => s.Subject == name)
                    .Where(s => s.Category == category)
                    .Where(s => s.appID == appID)
                    .Where(s => s.projectID == projectID)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }



        /// <summary>
        /// Looks for a subject ID for the given SubjectClass.
        /// </summary>
        /// <param name="subject">The SubjectClass being searched.</param>
        /// <returns>Returns an ID for the SubjectClass if it exists. 1 returned for nothing found.</returns>
        public async Task<int> FindSubjectID(SubjectClass subject, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            int subjectKey = 1;

            try
            {
                subjectKey = await master.Subjects
                    .Where(s => s.Subject == subject.Subject
                        && (s.accountID == 1 || s.accountID == id)
                        && s.Category == subject.Category
                        && s.Application != null && s.Project != null
                        && subject.Application != null && subject.Project != null
                            && s.Application.appID == subject.Application.appID
                                && s.Project.projectID == subject.Project.projectID)
                    .Select(s => s.subjectID)
                    .FirstOrDefaultAsync();

                if (subjectKey != 0)
                    return subjectKey;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return subjectKey;
        }


        /// <summary>
        /// Counts all logs in the database regardless of category.
        /// </summary>
        /// <returns>Returns the log count.</returns>
        public async Task<int> LogCount()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Logs.CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception in LogCountAsync: {ex.Message}");

                return -1;
            }
        }

        /// <summary>
        /// Counts all logs in the database regardless of category.
        /// </summary>
        /// <returns>Returns the log count.</returns>
        public async Task<int> LogCountAsync(CATEGORY category, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {

                return await master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.Category == category)
                    .Where(l => !new[] { 1, 2 }.Contains(l.appID))
                    .CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception in LogCountAsync: {ex.Message}");

            }

            return -1;
        }


        /// <summary>
        /// Counts the number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs ONLY.</returns>
        public async Task<int> QtLogCountAsync(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var qtLogs = master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.appID == 1)
                    .Where(l => l.Category == LOG.CATEGORY.CODING);

                return await qtLogs.CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception in QtLogCountAsync: {ex.Message}");

                return -1;
            }
        }

        /// <summary>
        /// Counts the number of Android Studio logs.
        /// </summary>
        /// <returns>The count of Android Studio logs ONLY.</returns>
        public async Task<int> ASLogCountAsync(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var androidLogs = master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.appID == 2)
                    .Where(l => l.Category == LOG.CATEGORY.CODING);

                return await androidLogs.CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ASLogCountAsync: {ex.Message}");
            }

            return -1;
        }

        /// <summary>
        /// Counts the number of coding logs.
        /// </summary>
        /// <returns>Returns the count of coding logs.</returns>
        public async Task<int> CodingLogCount(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var codingLogs = master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.Category == LOG.CATEGORY.CODING)
                    .Where(l => l.appID > 2);

                return await codingLogs.CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near CodingLogCount: {ex.Message}");

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of gfx logs.
        /// </summary>
        /// <returns>Returns the count of gfx logs.</returns>
        public async Task<int> GraphicsLogCount(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var graphicsLogs = master.Logs.Where(l => l.accountID == id)
                    .Where(l => l.Category == LOG.CATEGORY.GRAPHICS);

                return await graphicsLogs.CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near GraphicsLogCount: {ex.Message}");

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of film logs.
        /// </summary>
        /// <returns>Returns the count of film logs.</returns>
        public async Task<int> FilmLogCount(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.Category == LOG.CATEGORY.FILM).CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near FilmLogCount: {ex.Message}");

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of notes logs.
        /// </summary>
        /// <returns>Returns the count of notes logs.</returns>
        public async Task<int> NoteItemCount(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.Category == LOG.CATEGORY.NOTES)
                    .Where(l => l.appID == 15 || l.appID == 16).CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near NoteItemCount: {ex.Message}");

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of flexible notes logs.
        /// </summary>
        /// <returns>Returns the count of flexible notes logs.</returns>
        public async Task<int> FlexiNotesLogCountAsync(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Logs
                    .Where(l => l.accountID == id)
                    .Where(l => l.Category == LOG.CATEGORY.NOTES)
                    .Where(l => l.appID != 15 && l.appID != 16).CountAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);

                return -1;
            }
        }






        /// <summary>
        /// Retrieves applications from the database that belong to the currently online user.
        /// </summary>
        /// <returns>A List of ApplicationClass objects.</returns>
        public async Task<IEnumerable<ApplicationClass>> ListApplications(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Applications
                    .Where(a => new[] { 1, id }.Contains(a.accountID))
                    .Where(a => a.Name != "Unknown")
                    .Include(a => a.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return [];

        }


        /// <summary>
        /// Retrieves applications from the database that belong to the currently online user. Only retrieves by the given category.
        /// </summary>
        /// <param name="category">The category in which the application falls under in terms of logs.</param>
        /// <returns>A List of ApplicationClass objects.</returns>
        public async Task<IEnumerable<ApplicationClass>> ListApplications(CATEGORY category, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Applications
                    .Where(a => new[] { 1, id }.Contains(a.accountID))
                    .Where(a => !new[] { 1, 2 }.Contains(a.appID))
                    .Where(a => a.Category == category)
                    .Where(a => a.Name != "Unknown")
                    .Include(a => a.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return [];
        }



        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user.
        /// </summary>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<IEnumerable<ProjectClass>> ListProjects(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Projects
                    .Where(p => new[] { 1, id }.Contains(p.accountID))
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListProjects(category): {ex.Message}");
            }

            return [];
        }


        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user. Only retrieves by the given category.
        /// </summary>
        /// <param name="category">The category in which the project falls under in terms of logs.</param>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<IEnumerable<ProjectClass>> ListProjects(CATEGORY category, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Projects
                    .Where(p => new[] { 1, id }.Contains(p.accountID))
                    .Where(p => p.Category == category)
                    .Where(p => !new[] { 1, 2 }.Contains(p.Application.appID) && !new[] { Constants.Qt, Constants.Android }.Contains(p.Application.Name))
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListProjects(category): {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user for a specific application.
        /// Excludes projects with the name "Unnamed Project".
        /// </summary>
        /// <param name="appName">The application to filter projects by.</param>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<IEnumerable<ProjectClass>> ListProjects(string appName, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Projects
                    .Where(p => new[] { 1, id }.Contains(p.accountID))
                    .Where(p => p.Application.Name == appName)
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return [];
        }



        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user for a specific application.
        /// Excludes projects with the name "Unnamed Project".
        /// </summary>
        /// <param name="app">The application to filter projects by.</param>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<IEnumerable<ProjectClass>> ListProjects(ApplicationClass app, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Projects
                    .Where(p => new[] { 1, id }.Contains(p.accountID))
                    .Where(p => p.Application == app)
                    .Where(p => p.Name != "Unnamed Project")
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return [];
        }






        /// <summary>
        /// Retrieves subjects in the database based on its category.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SubjectClass>> ListSubjects(CATEGORY category, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Subjects
                    .Where(s => new[] { 1, id }.Contains(s.accountID))
                    .Where(s => s.Category == category)
                    .Where(s => s.Subject != "No Subject")
                    .Include(s => s.User)
                    .Include(s => s.Application)
                    .Include(s => s.Project)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListSubjects(category): {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Retrieves subjects in the database.
        /// </summary>
        /// <param name="project">The project in which the subjects preside.</param>
        /// <returns>A List with SubjectClass objects.</returns>
        public async Task<IEnumerable<SubjectClass>> ListSubjects(ProjectClass project, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Subjects
                    .Where(s => new[] { 1, id }.Contains(s.accountID))
                    .Where(s => s.Category == project.Category)
                    .Where(s => s.appID == project.appID)
                    .Where(s => s.projectID == project.projectID)
                    .Where(s => s.Subject != "No Subject")
                    .Include(s => s.User)
                    .Include(s => s.Application)
                    .Include(s => s.Project)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListSubjects(project): {ex.Message}");
            }

            return [];
        }



        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<IEnumerable<string>> ListQtOutputs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Outputs
                    .Where(o => o.appID == 1)
                    .Include(o => o.Application)
                    .Select(o => o.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListQtOutputsAsync(): {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<IEnumerable<string>> ListASOutputs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Outputs
                    .Where(o => o.appID == 2)
                    .Include(o => o.Application)
                    .Select(o => o.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListASOutputsAsync(): {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Retrieves outputs from the database.
        /// </summary>
        /// <param name="application">The outputs are only found in a particular application.</param>
        /// <returns>A List of OutputClass objects.</returns>
        public async Task<IEnumerable<string>> ListOutputs(CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Outputs
                    .Where(o => o.Category == category)
                    .Where(o => !new[] { 1, 2 }.Contains(o.appID))
                    .Include(o => o.Application)
                    .Select(o => o.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListOutputsAsync(application): {ex.Message}");
            }

            return [];
        }







        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<IEnumerable<string>> ListQtTypes()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Types
                    .Where(t => t.appID == 1)
                    .Include(t => t.Application)
                    .Select(t => t.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListQtTypesAsync(): {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<IEnumerable<string>> ListASTypes()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Types
                    .Where(t => t.appID == 2)
                    .Include(t => t.Application)
                    .Select(t => t.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListASTypesAsync(): {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Retrieves outputs from the database.
        /// </summary>
        /// <param name="application">The outputs are only found in a particular application.</param>
        /// <returns>A List of OutputClass objects.</returns>
        public async Task<IEnumerable<string>> ListTypes(CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                return await master.Types
                    .Where(t => t.Category == category)
                    .Where(t => !new[] { 1, 2 }.Contains(t.appID))
                    .Include(t => t.Application)
                    .Select(t => t.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near ListOutputsAsync(application): {ex.Message}");
            }

            return [];
        }



        public async Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText)
        {
            return null;
        }
        public async Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText, string projectName)
        {
            return null;
        }

        public async Task<List<CodingLOG>?> SearchQtLogsAsync(string searchBarText, string projectName, string appName)
        {
            return null;
        }

        /// <summary>
        /// Searches for logs that match the text provided. All fields are checked to see if they contain the text. Use this method if the PROJECT and APP is not specified.
        /// </summary>
        /// <param name="searchBarText">The query for the database.</param>
        /// <returns>Returns a list of logs that match the search bar text.</returns>
        public async Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                searchBarText = searchBarText.Trim();

                var logs = await master.CodingLogs
                    .Where(l => l.accountID == id)
                    .Include(l => l.Project)
                    .Include(l => l.Application)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                    .ThenInclude(postIt => postIt.Subject)
                    .OrderBy(l => l.Start)
                    .ToListAsync();

                var result = new List<CodingLOG>();

                foreach (var log in logs)
                {
                    bool matches =
                           log.Project!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Application!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Start.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.End.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Output!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Type!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase);

                    if (!matches && log.PostItList != null)
                    {
                        foreach (var postIt in log.PostItList)
                        {
                            var error = RtfConverter.ToPlainText(postIt.Error);
                            var solution = RtfConverter.ToPlainText(postIt.Solution);
                            var suggestion = RtfConverter.ToPlainText(postIt.Suggestion);
                            var comment = RtfConverter.ToPlainText(postIt.Comment);

                            if (error.Exception != null)
                                await writer.CreateFeedback(1, error.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (solution.Exception != null)
                                await writer.CreateFeedback(1, solution.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (suggestion.Exception != null)
                                await writer.CreateFeedback(1, suggestion.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (comment.Exception != null)
                                await writer.CreateFeedback(1, comment.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            matches =
                                   (!string.IsNullOrEmpty(error.Text) && error.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(solution.Text) && solution.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(suggestion.Text) && suggestion.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(comment.Text) && comment.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (postIt.Subject != null && !string.IsNullOrEmpty(postIt.Subject.Subject) && postIt.Subject.Subject.Contains(searchBarText, StringComparison.OrdinalIgnoreCase));

                            if (matches)
                                break;
                        }
                    }

                    if (!matches)
                    {
                        matches =
                               (log.Success && searchBarText.Contains("True", StringComparison.OrdinalIgnoreCase))
                            || (searchBarText.Equals("apps opened", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that launched", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("launch successful", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("launch succesful", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that didnt crash", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that didn't crash", StringComparison.OrdinalIgnoreCase) && log.Success);
                    }

                    if (matches)
                        result.Add(log);
                }

                return result;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }



        /// <summary>
        /// Searches for logs that match the text provided. All fields are checked to see if they contain the text. Use this method if the PROJECT is not specified.
        /// </summary>
        /// <param name="searchBarText">The query for the database.</param>
        /// <param name="appID">The application that logs use.</param>
        /// <returns>A list of logs that match the search bar text that were created with the provided app filter.</returns>
        public async Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int appID, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                searchBarText = searchBarText.Trim();

                // Fetch only logs for current user first
                var logs = await master.CodingLogs
                    .Where(l => l.accountID == id)
                    .Include(l => l.Project)
                    .Include(l => l.Application)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                    .ThenInclude(postIt => postIt.Subject)
                    .OrderBy(l => l.Start)
                    .ToListAsync();

                // In-memory filtering for search + IDs
                var result = new List<CodingLOG>();

                foreach (var log in logs.Where(l =>
                    l.Application!.appID == appID))
                {
                    bool matches =
                           log.Project!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Application!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Start.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.End.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Output!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Type!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase);

                    if (!matches && log.PostItList != null)
                    {
                        foreach (var postIt in log.PostItList)
                        {
                            var error = RtfConverter.ToPlainText(postIt.Error);
                            var solution = RtfConverter.ToPlainText(postIt.Solution);
                            var suggestion = RtfConverter.ToPlainText(postIt.Suggestion);
                            var comment = RtfConverter.ToPlainText(postIt.Comment);

                            if (error.Exception != null)
                                await writer.CreateFeedback(1, error.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (solution.Exception != null)
                                await writer.CreateFeedback(1, solution.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (suggestion.Exception != null)
                                await writer.CreateFeedback(1, suggestion.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (comment.Exception != null)
                                await writer.CreateFeedback(1, comment.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            matches =
                                   (!string.IsNullOrEmpty(error.Text) && error.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(solution.Text) && solution.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(suggestion.Text) && suggestion.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(comment.Text) && comment.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (postIt.Subject != null && !string.IsNullOrEmpty(postIt.Subject.Subject) && postIt.Subject.Subject.Contains(searchBarText, StringComparison.OrdinalIgnoreCase));

                            if (matches)
                                break;
                        }
                    }

                    if (!matches)
                    {
                        matches =
                               (log.Success && searchBarText.Contains("True", StringComparison.OrdinalIgnoreCase))
                            || (searchBarText.Equals("apps opened", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that launched", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("launch successful", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("launch succesful", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that didnt crash", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that didn't crash", StringComparison.OrdinalIgnoreCase) && log.Success);
                    }

                    if (matches)
                        result.Add(log);
                }

                return result;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }



        /// <summary>
        /// Searches for logs that match the text provided. All fields are checked to see if they contain the text.
        /// </summary>
        /// <param name="searchBarText">The query for the database.</param>
        /// <param name="projectID">The project that the log was created for.</param>
        /// <param name="appID">The application that logs use.</param>
        /// <returns>A list of logs that match the search bar text that were created with the provided app and project filter.</returns>
        public async Task<List<CodingLOG>?> SearchCodingLogsAsync(string searchBarText, int id, int projectID, int appID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                searchBarText = searchBarText.Trim();

                // Fetch only logs for current user first
                var logs = await master.CodingLogs
                    .Where(l => l.accountID == id)
                    .Include(l => l.Project)
                    .Include(l => l.Application)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                    .ThenInclude(postIt => postIt.Subject)
                    .OrderBy(l => l.Start)
                    .ToListAsync();

                // In-memory filtering for search + IDs
                var result = new List<CodingLOG>();

                foreach (var log in logs.Where(l =>
                    l.Project!.projectID == projectID &&
                    l.Application!.appID == appID))
                {
                    bool matches =
                           log.Project!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Application!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Start.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.End.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Output!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || log.Type!.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase);

                    if (!matches && log.PostItList != null)
                    {
                        foreach (var postIt in log.PostItList)
                        {
                            var error = RtfConverter.ToPlainText(postIt.Error);
                            var solution = RtfConverter.ToPlainText(postIt.Solution);
                            var suggestion = RtfConverter.ToPlainText(postIt.Suggestion);
                            var comment = RtfConverter.ToPlainText(postIt.Comment);

                            if (error.Exception != null)
                                await writer.CreateFeedback(1, error.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (solution.Exception != null)
                                await writer.CreateFeedback(1, solution.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (suggestion.Exception != null)
                                await writer.CreateFeedback(1, suggestion.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            if (comment.Exception != null)
                                await writer.CreateFeedback(1, comment.Exception?.Message ?? "Unknown cause", false, true, FeedbackType.Exception);

                            matches =
                                   (!string.IsNullOrEmpty(error.Text) &&
                                    error.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(solution.Text) &&
                                    solution.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(suggestion.Text) &&
                                    suggestion.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (!string.IsNullOrEmpty(comment.Text) &&
                                    comment.Text.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                                || (postIt.Subject != null &&
                                    !string.IsNullOrEmpty(postIt.Subject.Subject) &&
                                    postIt.Subject.Subject.Contains(searchBarText, StringComparison.OrdinalIgnoreCase));

                            if (matches)
                                break;
                        }
                    }

                    if (!matches)
                    {
                        matches =
                               (log.Success && searchBarText.Contains("True", StringComparison.OrdinalIgnoreCase))
                            || (searchBarText.Equals("apps opened", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that launched", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("launch successful", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("launch succesful", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that didnt crash", StringComparison.OrdinalIgnoreCase) && log.Success)
                            || (searchBarText.Equals("apps that didn't crash", StringComparison.OrdinalIgnoreCase) && log.Success);
                    }

                    if (matches)
                        result.Add(log);
                }

                return result;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return null;
        }





    }
}
