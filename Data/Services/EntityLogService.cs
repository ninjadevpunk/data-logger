using Core.DTOs;
using Core.DTOs.Cache;
using Core.DTOs.Notes;
using Core.Enums;
using Core.Models;
using Core.Models.App_Models;

namespace Data.Services
{
    internal static class EntityLogService
    {
        /// <summary>
        /// Creates a log from a DTO statelessly. Throws an <see cref="ArgumentOutOfRangeException"/> if the LogContext is invalid.
        /// </summary>
        /// <param name="input">User's UI input as a DTO</param>
        /// <returns>A <see cref="CodingLOG"/> object to be stored.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws an <see cref="ArgumentOutOfRangeException"/> if the LogContext is invalid.</exception>
        public static CodingLOG Create(LogCacheDTO input)
        {
            return input.Context switch
            {
                LogContext.Coding => CreateCoding((CodingCacheDTO)input),
                LogContext.Qt => CreateQt((QtCacheDTO)input),
                LogContext.AndroidStudio => CreateAndroid((AndroidCacheDTO)input),
                _ => throw new ArgumentOutOfRangeException(nameof(input))
            };
        }

        private static CodingLOG CreateCoding(CodingCacheDTO input)
        {
            CodingLOG log = new();
            var userId = input.UserId;
            log.accountID = userId;
            
            ACCOUNT account = new()
            {
                accountID = userId,
            };

            log.Author = account;

            BuildApp(log, account, input.ApplicationName);
            BuildProject(log, account, log.Application, input.ProjectName);
            log.Start = input.Start;
            log.End = input.End;
            AddProjectDetails(log, account, input.OutputName, input.TypeName);
            AddPostIts(log, [.. input.PostIts], account, log.Application, log.Project);
            AddCodingDetails(log, input.Bugs, input.Success);

            return log;
        }

        private static CodingLOG CreateQt(CodingCacheDTO input)
        {
            throw new NotImplementedException();
        }

        private static AndroidCodingLOG CreateAndroid(CodingCacheDTO input)
        {
            throw new NotImplementedException();
        }

        public static NoteItem CreateNote(NoteDTO noteDTO)
        {
            // TODO: Implement by taking DTO and create a new NoteItem
            throw new NotImplementedException();
        }

        private static void BuildApp(CodingLOG log, ACCOUNT account, string appName)
        {
            log.Application = new ApplicationClass
            {
                User = account,
                Name = appName,
            };
        }

        private static void BuildProject(CodingLOG log, ACCOUNT account, ApplicationClass app, string projectName)
        {
            log.Project = new ProjectClass
            {
                User = account,
                Application = app,
                Name = projectName,
            };
        }

        private static void AddProjectDetails(CodingLOG log, ACCOUNT account, string outputName, string typeName)
        {
            log.Output = new OutputClass
            {
                User = account,
                Name = outputName
            };

            log.Type = new TypeClass
            {
                User = account,
                Name = typeName
            };
        }

        private static void AddCodingDetails(CodingLOG log, int bugs, bool launchedSuccessfully)
        {
            log.Bugs = bugs;
            log.Success = launchedSuccessfully;
        }

        private static void AddPostIts(CodingLOG log, List<PostItDTO> postIts, ACCOUNT account, ApplicationClass app, ProjectClass project)
        {
            log.PostItList = [.. postIts.Select(postIt => new PostIt
            {
                Author = account,
                Subject = new SubjectClass
                {
                    User = account,
                    Application = app,
                    Project = project,
                    Subject = postIt.Subject
                },
                Error = postIt.Error,
                Solution = postIt.Solution,
                Suggestion = postIt.Suggestion,
                Comment = postIt.Comment,
                ERCaptureTime = postIt.ERCaptureTime,
                SOCaptureTime = postIt.SOCaptureTime
            })];
        }
    }
}
