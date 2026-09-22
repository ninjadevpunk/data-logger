using Data_Logger_1._3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using static Data_Logger_1._3.Models.FeedbackMessage;
using static Data_Logger_1._3.Models.NotesLOG;
using static Data_Logger_1._3.Services.EntityReader;

namespace Data_Logger_1._3.Services
{
    public class EntityWriter
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Official Entity Framework data writer.
        /// </summary>
        public EntityWriter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }















        /// <summary>
        /// Sets the currently online user to online.
        /// </summary>
        /// <param name="account">The account that is online.</param>
        /// <returns>Returns whether the account is set successfully.</returns>
        public async Task<bool> SetCurrentUser(ACCOUNT account)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            try
            {
                var all = await master.Accounts.ToListAsync();

                foreach (var a in all)
                    a.IsOnline = (a.accountID == account.accountID);

                await master.SaveChangesAsync();

                var user = await master.Accounts.FirstOrDefaultAsync(a => a.accountID == account.accountID);

                return user?.IsOnline ?? false;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "SetCurrentUser(account)");
            }

            return false;
        }

        /// <summary>
        /// Sets the currently online user to offline.
        /// </summary>
        /// <param name="account">The account to sign out</param>
        /// <returns>Returns whether the account is unset successfully.</returns>
        public async Task<bool> UnsetCurrentUser(ACCOUNT account)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            try
            {
                var onlineUsers = await master.Accounts.Where(a => a.IsOnline).ToListAsync();

                if (!onlineUsers.Any())
                    return true;

                foreach (var u in onlineUsers)
                {
                    if(u.accountID == account.accountID)
                        u.IsOnline = false;
                }

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "UnsetCurrentUser(account)");
            }

            return false;
        }


        /// <summary>
        /// Adds an account that is provided as an argument to the database.
        /// </summary>
        /// <param name="account">The account that will be added to the database.</param>
        /// <returns">A boolean value that indicated if the process was successful or not.</returns>
        public async Task<bool> AddAccount(ACCOUNT account)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            bool accountCreated = false;

            try
            {
                if (account is null)
                    throw new ArgumentNullException($"\"{nameof(account)}\" - Account not initialised. Operation aborted.");


                if (!await reader.EmailExists(scope, account.Email))
                {
                    account.IsOnline = false;

                    var hiddenPassword = account.Password;
                    account.Password = string.Empty;


                    await master.Accounts.AddAsync(account);
                    await master.SaveChangesAsync();

                    account.Password = SaltedSHA256Hash(hiddenPassword, account.accountID.ToString());

                    await master.SaveChangesAsync();


                    accountCreated = true;
                }
                else
                    throw new EmailConflictException("Email exists!");

            }
            catch (EmailConflictException mailex)
            {
                await HandleExceptionAsync("AddAccount(account)", mailex, "Error", "The email you entered has been taken. Please use a different one.", 
                    "EmailConflictException");
            }
            catch (ArgumentNullException nullex)
            {
                await HandleExceptionAsync("AddAccount(account)", nullex, "Error", exceptionType: "ArgumentNullException");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync("AddAccount(account)", ex, "Error", "A problem occurred on our end so please try again later. We " +
                    "apologise for any inconvenience caused. Feedback will automatically be sent to us.");
            }

            return accountCreated;

        }










        #region Store Log



        /// <summary>
        /// Creates a log in the database.
        /// </summary>
        /// <param name="log">The log being stored.</param>
        public async Task<bool> CreateLOG(LOG log, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();


            try
            {
                await using var transaction = await master.Database.BeginTransactionAsync();
                
                await PrepareLogDetails(log, scope, master, id);
                await PreparePostItDetails(log, scope, master, id);

                await InsertSubLogDetails(log, scope, master);

                await master.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (InvalidCastException castx)
            {
                await HandleExceptionAsync(castx, "CreateLOG(log)", "InvalidCastException");
            }
            catch (OperationCanceledException opex)
            {
                await HandleExceptionAsync(opex, "CreateLOG(log)", "OperationCanceledException");

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "CreateLOG(log)");
            }

            return false;
        }


        private async Task PrepareLogDetails(LOG log, AsyncServiceScope scope, EntityMaster master, int userId)
        {
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            var onlineUser = await reader.FindAccountByID(scope, master, userId) ?? new();
            log.Author = onlineUser;
            var app = log.Application;
            app.User = onlineUser;

            var existingApp = await reader.FindApplication(scope, master, userId, app.Name);

            var project = log.Project;
            project.User = onlineUser;
            var output = log.Output;
            log.Output = await reader.FindOutput(scope, master, log.Output.Name) ?? output;
            var type = log.Type;
            log.Type = await reader.FindType(scope, master, log.Type.Name) ?? type;

            if (existingApp != null)
            {
                app = existingApp;

                var existingProject = await reader.FindProject(scope, master, userId, project.Name, app.appID);

                if (existingProject != null)
                {
                    project = existingProject;
                }

            }
            
            if (project.Name.Contains("Unnamed Project", StringComparison.OrdinalIgnoreCase))
            {
                var existingProject = await reader.FindProject(scope, master, userId, project.Name, app.appID);

                if (existingProject != null)
                {
                    project = existingProject;
                }
            }

            log.Application = app;
            project.Application = app;
            log.Project = project;
        }

        private string ValidateString(string input)
        {
            string pattern = @"[\s\S]+";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input) ? input : string.Empty;
        }

        private async Task PreparePostItDetails(LOG log, AsyncServiceScope scope, EntityMaster master, int id)
        {
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            var onlineUser = await reader.FindAccountByID(scope, master, id);

            if (onlineUser == null)
                onlineUser = new();

            foreach (PostIt postIt in log.PostItList)
            {

                if (postIt.Subject != null)
                {
                    var appID = log.Application.appID;
                    var projectID = log.Project.projectID;

                    var existingSubject = await reader.FindSubject(scope, master, postIt.Subject.Subject, log.Category, id, appID, projectID);

                    if (existingSubject != null)
                    {
                        postIt.Subject = existingSubject;
                    }
                    else
                    {
                        postIt.Subject.User = onlineUser;
                        postIt.Subject.Application = log.Application;
                        postIt.Subject.Project = log.Project;
                    }

                    postIt.Author = onlineUser;
                }
            }
        }

        private async Task InsertSubLogDetails(LOG log, AsyncServiceScope scope, EntityMaster master)
        {

            switch (log.Category)
            {
                case LOG.CATEGORY.CODING when log is CodingLOG codingLog:

                    if (codingLog is AndroidCodingLOG androidCodingLog)
                    {
                        await master.AndroidCodingLogs.AddAsync(androidCodingLog);
                    }
                    else
                    {
                        await master.CodingLogs.AddAsync(codingLog);
                    }

                    break;
                case LOG.CATEGORY.GRAPHICS when log is GraphicsLOG graphicsLog:
                    await master.GraphicsLogs.AddAsync(graphicsLog);
                    break;
                case LOG.CATEGORY.FILM when log is FilmLOG filmLog:
                    await master.FilmLogs.AddAsync(filmLog);
                    break;
                case LOG.CATEGORY.NOTES when log is NotesLOG notesLog:

                    switch (notesLog.notelogtype)
                    {
                        case NOTELOGType.FLEXI when notesLog is FlexiNotesLOG flexiNotesLog:
                            {
                                await master.FlexiNotesLogs.AddAsync(flexiNotesLog);

                                break;
                            }
                        default:
                            {
                                var genericNotesLog = (NoteItem)notesLog;


                                if (genericNotesLog.Checklist is not null && genericNotesLog.Checklist.Items.Count > 0)
                                {
                                    master.Checklists.Add(genericNotesLog.Checklist);

                                    foreach (var item in genericNotesLog.Checklist.Items)
                                        await master.ChecklistItems.AddAsync(item);
                                }
                                else
                                {
                                    await master.NoteItems.AddAsync(genericNotesLog);
                                }
                                break;
                            }
                    }
                    break;
            }
        }




        #endregion




        #region Security



        public static string SaltedSHA256Hash(string value, string accountID)
        {
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;

                // The account ID is the salt. 
                // So 2 users with the same password have different hashes. 
                // For example, if someone knows their own hash, they can't see who has the same password.
                string input = value + accountID;
                byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder hashedStringBuilder = new StringBuilder();
                foreach (byte b in result)
                {
                    hashedStringBuilder.Append(b.ToString("x2"));
                }

                return hashedStringBuilder.ToString();
            }
        }




        #endregion





        #region Feedback Creation



        public async Task<int> CreateFeedback(int accountID, string description, bool canContact, bool isAutoFeed = true, FeedbackType category = FeedbackType.Bug)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            try
            {
                var feedback = new FeedbackMessage
                {
                    accountID = accountID,
                    Description = description,
                    CanContact = canContact,
                    Category = category,
                    DateReported = DateTime.Now,
                    IsAutoFeed = isAutoFeed
                };

                master.AllFeedback.Add(feedback);
                await master.SaveChangesAsync();

                return feedback.feedbackID;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    Debug.WriteLine($"CREATE FEEDBACK FAILED: {ex.Message}.");
                else
                    Debug.WriteLine($"CREATE FEEDBACK FAILED: {ex.Message}. Inner exception: {ex.InnerException.Message ?? ""}");

            }

            return -1;
        }

        public async Task HandleExceptionAsync(string methodName, Exception ex, string messageBoxCaption, string messageBoxMessage = "A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.", string exceptionType = "Exception")
        {
            // Build the exception description
            var description = $"{exceptionType ?? "Exception"} occurred near {methodName}: {ex.Message}" +
                  (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");


            // Log to CreateFeedback
            await CreateFeedback(1, description, false, true, FeedbackType.Exception);

            // Show a message box only if showMessageBox is true
            if (!string.IsNullOrEmpty(messageBoxCaption))
            {
                MessageBox.Show(messageBoxMessage,
                                messageBoxCaption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        public async Task HandleExceptionAsync(Exception ex, string methodName, string exceptionType = "")
        {
            // Build the exception description
            var description = $"{exceptionType ?? "Exception"} occurred near {methodName}: {ex.Message}" +
                  (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");

            // Log to CreateFeedback
            await CreateFeedback(1, description, false, true, FeedbackType.Exception);
        }






        #endregion


    }
}
