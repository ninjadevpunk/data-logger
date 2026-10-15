using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Notes;
using Core.Enums;
using Core.Models;
using Core.Models.App_Models;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using static Core.Models.FeedbackMessage;

namespace Data.Services
{
    internal class EntityWriter
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
        /// <param name="userId">The user's ID</param>
        /// <returns>Returns whether the account is set successfully.</returns>
        public async Task<bool> SetCurrentUserAsync(int userId)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            try
            {
                var all = await master.Accounts.ToListAsync();

                foreach (var a in all)
                    a.IsOnline = (a.accountID == userId);

                await master.SaveChangesAsync();

                var user = await master.Accounts.FirstOrDefaultAsync(a => a.accountID == userId);

                return user?.IsOnline ?? false;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "SetCurrentUserAsync(account)");
            }

            return false;
        }

        /// <summary>
        /// Sets the currently online user to offline.
        /// </summary>
        /// <param name="account">The account to sign out</param>
        /// <returns>Returns whether the account is unset successfully.</returns>
        public async Task<bool> UnsetCurrentUserAsync(int userId)
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
                    if (u.accountID == userId)
                        u.IsOnline = false;
                }

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "UnsetCurrentUserAsync(account)");
            }

            return false;
        }


        /// <summary>
        /// Adds an account that is provided as an argument to the database.
        /// </summary>
        /// <param name="account">The account that will be added to the database.</param>
        /// <returns">A boolean value that indicated if the process was successful or not.</returns>
        public async Task<(bool, int)> AddAccount(ACCOUNT account)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            bool accountCreated = false;

            try
            {
                if (account == null)
                    throw new ArgumentNullException(nameof(account), "Account is null!");


                if (await EntityReader.EmailExists(scope, account.Email))
                    throw new EmailConflictException("Email exists!");

                account.IsOnline = false;

                var hiddenPassword = account.Password;
                account.Password = string.Empty;


                await master.Accounts.AddAsync(account);
                await master.SaveChangesAsync();

                account.Password = SaltedSHA256Hash(hiddenPassword, account.accountID.ToString());

                await master.SaveChangesAsync();


                accountCreated = true;
            }
            catch (EmailConflictException mailex)
            {
                await HandleExceptionAsync(mailex);
            }
            catch (ArgumentNullException nullex)
            {
                await HandleExceptionAsync(nullex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }

            return (accountCreated, account.accountID);

        }










        #region Store Log



        /// <summary>
        /// Creates a log entity in the database from data in a DTO.
        /// </summary>
        /// <param name="logCacheDTO">The log DTO for log info</param>
        /// <returns>Returns true if the log was successfully created; otherwise false.</returns>
        public async Task<bool> CreateLOGAsync(LogCacheDTO logCacheDTO)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();


            try
            {
                await using var transaction = await master.Database.BeginTransactionAsync();

                var log = EntityLogService.Create(logCacheDTO);

                await PrepareLogDetailsAsync(log, scope, master);
                await PreparePostItDetailsAsync(log, scope, master);

                await InsertSubLogDetails(log, logCacheDTO.Context, master);

                await master.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }

            return false;
        }


        private static async Task PrepareLogDetailsAsync(LOG log, AsyncServiceScope scope, EntityMaster master)
        {
            var user = await EntityReader.FindAccountByID(scope, master, log.accountID) ?? throw new InvalidOperationException("No user logged in!");
            var userId = user.accountID;
            log.Author = user;

            // Resolve Application
            var existingApp = await EntityReader.FindApplication(scope, master, userId, log.Application.Name);

            if (existingApp != null)
                log.Application = existingApp;
            else
            {
                log.Application.User = user;
            }

            // Resolve Project
            var existingProject = await EntityReader.FindProject(scope, master, userId, log.Project.Name, log.Application.appID);

            if (existingProject != null)
                log.Project = existingProject;
            else
            {
                log.Project.User = user;
                log.Project.Application = log.Application;
            }

            // Resolve Output
            var existingOutput = await EntityReader.FindOutput(scope, log.Output.Name, log.Category) ?? throw new ArgumentNullException(null, "Output not found!");
            log.Output = existingOutput;

            // Resolve Type
            var existingType = await EntityReader.FindType(scope, log.Type.Name, log.Category) ?? throw new ArgumentNullException(null, "Type not found!");
            log.Type = existingType;
        }



        private static async Task PreparePostItDetailsAsync(LOG log, AsyncServiceScope scope, EntityMaster master)
        {
            var user = log.Author;
            var userId = user.accountID;

            // Resolve PostIts
            foreach (var postIt in log.PostItList)
            {
                postIt.Author = user;

                var existingSubject = await EntityReader.FindSubject(master, postIt.Subject.Subject, log.Category, userId, log.Application.appID,
                        log.Project.projectID);

                if (existingSubject != null)
                {
                    postIt.Subject = existingSubject;
                }
                else
                {
                    postIt.Subject.User = user;
                    postIt.Subject.Application = log.Application;
                    postIt.Subject.Project = log.Project;
                }
            }
        }

        private static async Task InsertSubLogDetails(LOG log, LogContext context, EntityMaster master)
        {

            switch (context)
            {
                case LogContext.Qt when log is QtLOG qtLog:
                    {
                        await master.CodingLogs.AddAsync(qtLog);
                        break;
                    }
                case LogContext.AndroidStudio when log is AndroidCodingLOG androidCodingLog:
                    {
                        await master.AndroidCodingLogs.AddAsync(androidCodingLog);
                        break;
                    }
                default:
                    {
                        if (log is CodingLOG codingLog)
                            await master.CodingLogs.AddAsync(codingLog);
                        break;
                    }
            }
        }


        /// <summary>
        /// Inserts a note DTO into the database.
        /// </summary>
        /// <param name="noteDTO">The noteDTO to insert.</param>
        /// <param name="id">The user ID of logged in user.</param>
        /// <returns>Returns a tuple: (success flag, the created note data DTO).</returns>
        public async Task<(bool Success, NoteDataDTO? NoteDataDTO)> CreateNoteAsync(NoteDTO noteDTO, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();


            try
            {
                await using var transaction = await master.Database.BeginTransactionAsync();

                // TODO: Create entity with NoteDTO and use EntityNoteService

                await master.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, new NoteDataDTO { });
            }
            catch (OperationCanceledException opex)
            {
                await HandleExceptionAsync(opex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }

            return (false, null);
        }

        /// <summary>
        /// Inserts a checklist DTO into the database.
        /// </summary>
        /// <param name="checkListDTO">The checklist DTO to insert.</param>
        /// <param name="id">The ID of the entity to insert.</param>
        /// <returns>Returns a tuple: (success flag, the created checklist data DTO).</returns>
        public async Task<(bool Success, CheckListDataDTO? CheckListDataDTO)> CreateCheckListAsync(CheckListDTO checkListDTO, int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();


            try
            {
                await using var transaction = await master.Database.BeginTransactionAsync();

                // TODO: Create entity with CheckListDTO and use EntityCheckListService
                // TODO: Create a checklist data DTO with a checklist retrieved from the database after saving changes
                // and return it.

                await master.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, new CheckListDataDTO { });
            }
            catch (OperationCanceledException opex)
            {
                await HandleExceptionAsync(opex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }

            return (false, null);
        }




        #endregion




        #region Security



        public static string SaltedSHA256Hash(string value, string accountID)
        {
            Encoding enc = Encoding.UTF8;

            string input = value + accountID;
            byte[] result = SHA256.HashData(enc.GetBytes(input));

            StringBuilder hashedStringBuilder = new StringBuilder();
            foreach (byte b in result)
            {
                hashedStringBuilder.Append(b.ToString("x2"));
            }

            return hashedStringBuilder.ToString();
        }




        #endregion





        #region Feedback Creation


        /// <summary>
        /// Creates a feedback message and saves it to the database.
        /// </summary>
        /// <param name="accountID">The ID of the account reporting the feedback.</param>
        /// <param name="description">The description of the feedback or issue being reported.</param>
        /// <param name="canContact">Indicates whether the user can be contacted regarding the feedback.</param>
        /// <param name="isAutoFeed">Indicates whether the feedback was generated automatically.</param>
        /// <param name="category">The category assigned to the feedback.</param>
        /// <returns>Returns the ID of the created feedback message, or -1 if the feedback could not be created.</returns>
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



        /// <summary>
        /// Handles an exception by creating an automatic exception feedback report.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="methodName">The name of the method where the exception occurred.</param>
        /// <returns>A task representing the asynchronous feedback creation operation.</returns>
        public async Task HandleExceptionAsync(Exception ex, [CallerMemberName] string methodName = "")
        {
            // Build the exception description
            var exceptionName = ex?.GetType().Name ?? "Exception";
            var message = ex?.Message ?? "Unknown cause.";
            var description = $"{exceptionName} occurred near {methodName}: {message}";

            if (ex?.InnerException is Exception inner)
            {
                description += $" Inner exception: {inner.Message}";
            }

            // Log to CreateFeedback
            await CreateFeedback(1, description, false, true, FeedbackType.Exception);
        }



        /// <summary>
        /// Handles a data error by creating an automatic exception feedback report.
        /// </summary>
        /// <param name="error">The data error containing the exception information.</param>
        /// <param name="methodName">The name of the method where the error occurred.</param>
        /// <returns>A task representing the asynchronous feedback creation operation.</returns>
        public async Task HandleExceptionAsync(DataError error, [CallerMemberName] string methodName = "")
        {
            var exceptionName = error.Exception?.GetType().Name ?? "Exception";
            var message = error.Exception?.Message ?? "Unknown cause.";
            var description = $"{exceptionName} occurred near {methodName}: {message}";

            if (error.Exception?.InnerException is Exception inner)
            {
                description += $" Inner exception: {inner.Message}";
            }

            await CreateFeedback(1, description, false, category: FeedbackType.Exception);
        }







        #endregion


    }
}
