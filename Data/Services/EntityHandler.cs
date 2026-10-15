using Core.DTOs.Data;
using Core.DTOs.Identity;
using Core.Enums;
using Core.Models;
using Core.Models.App_Models;
using Data.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Services
{
    internal class EntityHandler
    {
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// The Entity Framework updater and deleter for EntityMaster.
        /// </summary>
        public EntityHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        #region Updates


        // TODO: Update Qt Logs
        public async Task<bool> UpdateQtLog(QtDataDTO qtDataDTO)
        {
            bool isUpdated = false;


            return isUpdated;
        }

        /// <summary>
        /// Updates a logDataDTO in the database.
        /// </summary>
        /// <param name="logDataDTO">The logDataDTO needing updates.</param>
        /// <returns>True if the log has been updated; otherwise false.</returns>
        public async Task<bool> UpdateLogAsync(LogDataDTO logDataDTO)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var reader = scope.ServiceProvider.GetRequiredService<EntityReader>();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

                var existingLog = await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstOrDefaultAsync(l => l.ID == logDataDTO.LogId) ?? throw new ArgumentNullException(null, "Log not found!");

                var user = existingLog.Author;
                var userId = logDataDTO.UserId;
                // Resolve Application

                ApplicationClass? newApp = null;

                if (logDataDTO.AppId != existingLog.Application.appID)
                {
                    var existingApp = await EntityReader.FindApplicationByID(scope, master, logDataDTO.AppId);

                    if (existingApp != null)
                        existingLog.Application = existingApp;
                    else
                    {
                        newApp = new ApplicationClass
                        {
                            User = user,
                            Name = logDataDTO.ApplicationName,
                            Category = Extensions.GetLogCategory(logDataDTO.Context)
                        };

                        existingLog.Application = newApp;
                    }
                }


                // Resolve Project

                ProjectClass? newProject = null;

                if (newApp == null)
                {
                    if (logDataDTO.ProjectId != existingLog.Project.projectID)
                    {
                        var existingProject = await EntityReader.FindProjectByID(scope, master, logDataDTO.ProjectId);

                        if (existingProject != null)
                            existingLog.Project = existingProject;
                        else
                        {
                            newProject = new ProjectClass
                            {
                                User = user,
                                Application = existingLog.Application,
                                Name = logDataDTO.Project.ProjectName
                            };

                            existingLog.Project = newProject;
                        }
                    }
                }
                else
                {
                    newProject = new ProjectClass
                    {
                        User = user,
                        Application = newApp,
                        Name = logDataDTO.Project.ProjectName
                    };

                    existingLog.Project = newProject;
                }

                existingLog.Start = logDataDTO.Start;
                existingLog.End = logDataDTO.End;

                if (logDataDTO.OutputID != existingLog.outputID)
                    existingLog.Output = master.Outputs.FirstOrDefault(o => o.outputID == logDataDTO.OutputID)
                        ?? throw new ArgumentNullException(null, "Output not found!");

                if (logDataDTO.TypeID != existingLog.typeID)
                    existingLog.Type = master.Types.FirstOrDefault(t => t.typeID == logDataDTO.TypeID)
                        ?? throw new ArgumentNullException(null, "Type not found!");

                IReadOnlyList<PostItDataDTO> postIts = [.. logDataDTO.PostIts.Cast<PostItDataDTO>()];

                // Remove Deleted PostIts

                HashSet<int> incomingIds = [.. postIts
                    .Where(p => p.PostItId > 0)
                    .Select(p => p.PostItId)];

                foreach (var postIt in existingLog.PostItList.ToList())
                {
                    if (!incomingIds.Contains(postIt.postItID))
                    {
                        master.PostIts.Remove(postIt);
                    }
                }


                // Prepare existing PostIts
                var existingPostIts = existingLog.PostItList
                    .ToDictionary(p => p.postItID);


                var newSubjects = new Dictionary<string, SubjectClass>();

                foreach (var postIt in postIts)
                {
                    // Resolve Subject

                    var subjectDataDTO = (SubjectDataDTO)postIt.Subject;

                    SubjectClass? subject = null;

                    // Look for an existing Subject when the target application and project already exist.
                    if (newApp == null && newProject == null)
                    {
                        subject = await EntityReader.FindSubject(master, subjectDataDTO.SubjectName, Extensions.GetLogCategory(logDataDTO.Context),
                            userId, existingLog.Application.appID, existingLog.Project.projectID);
                    }

                    // Reuse a Subject created earlier during this update if no persisted Subject was found.
                    if (subject == null && newSubjects.TryGetValue(subjectDataDTO.SubjectName, out var newSubject))
                    {
                        subject = newSubject;
                    }

                    // Create a new Subject when neither lookup found a suitable match.
                    if (subject == null)
                    {
                        subject = new SubjectClass
                        {
                            User = user,
                            Category = Extensions.GetLogCategory(logDataDTO.Context),
                            Subject = subjectDataDTO.SubjectName,
                            Application = existingLog.Application,
                            Project = existingLog.Project
                        };

                        newSubjects[subjectDataDTO.SubjectName] = subject;
                    }


                    // ADD or UPDATE

                    // Existing PostIt
                    if (existingPostIts.TryGetValue(postIt.PostItId, out var existingPostIt))
                    {
                        existingPostIt.Subject = subject;

                        existingPostIt.Error = postIt.Error;
                        existingPostIt.ERCaptureTime = postIt.ERCaptureTime;
                        existingPostIt.Solution = postIt.Solution;
                        existingPostIt.SOCaptureTime = postIt.SOCaptureTime;
                        existingPostIt.Suggestion = postIt.Suggestion;
                        existingPostIt.Comment = postIt.Comment;
                    }

                    // New PostIt
                    else
                    {
                        var newPostIt = new PostIt
                        {
                            Author = user,
                            Subject = subject,
                            Error = postIt.Error,
                            ERCaptureTime = postIt.ERCaptureTime,
                            Solution = postIt.Solution,
                            SOCaptureTime = postIt.SOCaptureTime,
                            Suggestion = postIt.Suggestion,
                            Comment = postIt.Comment
                        };

                        existingLog.PostItList.Add(newPostIt);
                    }
                }

                switch (logDataDTO.Context)
                {
                    case LogContext.Qt when logDataDTO is QtDataDTO qtDataDTO:
                    case LogContext.AndroidStudio when logDataDTO is AndroidDataDTO androidDataDTO:
                    case LogContext.Coding when logDataDTO is CodingDataDTO codingDataDTO:
                        {
                            var codingLog = (CodingLOG)existingLog;
                            codingDataDTO = (CodingDataDTO)logDataDTO;
                            codingLog.Bugs = codingDataDTO.Bugs;
                            codingLog.Success = codingDataDTO.Success;

                            break;
                        }
                }

                await master.SaveChangesAsync();

                return true;
            }
            catch (ArgumentNullException nullex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(nullex);
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }

            return false;
        }

        public async Task<bool> UpdateNotesLog(NoteItem noteItem)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            bool isUpdated = false;

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
                master.NoteItems.Update(noteItem);

                await master.SaveChangesAsync();
                isUpdated = true;

            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }

            return isUpdated;
        }


        /// <summary>
        /// Updates the profile picture for a user based on their email address and the provided file path.
        /// </summary>
        /// <param name="id">The user's email</param>
        /// <param name="filePath">The file path of the current profile picture</param>
        /// <returns>Returns true if the profile pic change succeeded, false otherwise.</returns>
        public async Task<bool> UpdateProfilePicAsync(int id, string filePath)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            bool isUpdated = false;

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

                var account = await master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == id) 
                    ?? throw new ArgumentNullException(null, "Account not found!");
                account.ProfilePic = filePath;

                await master.SaveChangesAsync();

                isUpdated = true;
            }
            catch (ArgumentNullException nullex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(nullex);
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }

            return isUpdated;
        }


        /// <summary>
        /// Updates a user's information in the database based on the provided UserSettings object.
        /// </summary>
        /// <param name="user">The user DTO containing update data</param>
        /// <returns>Returns true if the user information was updated successfully, false otherwise.</returns>
        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            bool isUpdated = false;
            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
                var existingUser = await master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == user.UserId) ?? throw new ArgumentNullException(null, "Account not found!");
                existingUser.FirstName = user.Name;
                existingUser.LastName = user.Surname;
                existingUser.Email = user.Email;
                existingUser.IsEmployee = user.IsEmployee;
                existingUser.CompanyName = user.CompanyName;
                existingUser.CompanyAddress = user.CompanyAddress;
                await master.SaveChangesAsync();
                isUpdated = true;
            }
            catch (ArgumentNullException nullex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(nullex);
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }
            return isUpdated;
        }



        /// <summary>
        /// Updates a user's password in the database based on the provided email and new password.
        /// </summary>
        /// <param name="newPassword">The user's new password</param>
        /// <param name="email">The user's email address</param>
        /// <returns>Returns true if the password was updated successfully, false otherwise.</returns>
        public async Task<bool> ChangePasswordAsync(string newPassword, string email)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            bool isUpdated = false;
            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
                var account = await master.Accounts
                    .FirstOrDefaultAsync(a => a.Email == email) 
                    ?? throw new ArgumentNullException(null, "Account not found!");
                account.Password = EntityWriter.SaltedSHA256Hash(newPassword, account.accountID.ToString());
                await master.SaveChangesAsync();

                isUpdated = true;
            }
            catch (ArgumentNullException nullex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(nullex);
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }
            return isUpdated;
        }



        #endregion












        #region Deletions



        public async Task<bool> DeleteLOGAsync(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

                if (log == null)
                    return false;

                master.Remove(log);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }

            return false;
        }

        public async Task<bool> DeleteLOGByIDAsync(int ID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

                var logDeletionCandidate = await master.Logs
                    .FirstOrDefaultAsync(log => log.ID == ID);

                if (logDeletionCandidate == null)
                    return false;

                master.Logs.Remove(logDeletionCandidate);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }

            return false;
        }

        public async Task<bool> DeleteNoteAsync(int ID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

            try
            {
                var noteToDelete = await master.NoteItems
                    .FirstOrDefaultAsync(n => n.ID == ID);

                if (noteToDelete == null)
                    return false;

                master.NoteItems.Remove(noteToDelete);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex);
            }

            return false;
        }



        /// <summary>
        /// Deletes a user's account from the database using the provided account ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>Returns true if the account was deleted successfully, false otherwise.</returns>
        public async Task<bool> DeleteAccountAsync(int id)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

            try
            {
                var accountToDelete = await master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == id);

                if (accountToDelete == null)
                    return false;

                master.Accounts.Remove(accountToDelete);
                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex);
            }

            return false;
        }








        #endregion




    }
}
