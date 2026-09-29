using Core.DTOs.Cache;
using Core.Enums;
using Core.Models;
using Core.Models.App_Models;

namespace Data.Services
{
    internal class EntityCodingService
    {

        public CodingLOG Create(CodingCacheDTO input, ACCOUNT account)
        {
            return input.Context switch
            {
                LogContext.Coding => CreateGeneric(input, account),
                LogContext.Qt => CreateQt(input),
                LogContext.AndroidStudio => CreateAndroid(input),
                _ => throw new ArgumentOutOfRangeException(nameof(input.Context))
            };
        }

        private CodingLOG CreateGeneric(CodingCacheDTO input, ACCOUNT account)
        {
            var log = new CodingLOG();
            

            return log;
        }

        private CodingLOG CreateQt(CodingCacheDTO input)
        {
            throw new NotImplementedException();
        }

        private CodingLOG CreateAndroid(CodingCacheDTO input)
        {
            throw new NotImplementedException();
        }

        private void AddProjectDetails(CodingLOG log, ACCOUNT account, string appName, string projectName, string output, string type)
        {
            
        }

        private void AddCodingDetails(CodingLOG log, int bugs, bool launchedSuccessfully)
        {
            log.Bugs = bugs;
            log.Success = launchedSuccessfully;
        }

        private void AddPostIts(CodingLOG log, List<PostItCacheDTO> postIts, ACCOUNT account, ApplicationClass app, ProjectClass project)
        {
            foreach (var postIt in postIts)
            {
                var subject = new SubjectClass
                {
                    User = account,
                    Application = app,
                    Project = project,
                    Subject = postIt.Subject
                };

                log.PostItList.Add(new PostIt
                {
                    Subject = subject,
                    Error = postIt.Error,
                    Solution = postIt.Solution,
                    Suggestion = postIt.Suggestion,
                    Comment = postIt.Comment,
                    ERCaptureTime = postIt.ERCaptureTime,
                    SOCaptureTime = postIt.SOCaptureTime
                });
            }
        }
    }
}
