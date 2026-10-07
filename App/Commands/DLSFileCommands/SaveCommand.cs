using Core.DTOs;
using Core.DTOs.Cache;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.DLSFileCommands
{
    /// <summary>
    /// Saving a .dls file in the logger UI
    /// </summary>
    internal class SaveCommand : AsyncCommandBase
    {
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cachemaster;


        public SaveCommand(LoggerViewModel loggerCreateViewModel, IAuthService authService, IDataService dataService, ICacheService cacheService)
        {
            _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _cachemaster = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        protected async override Task ExecuteAsync(object parameter)
        {
            try
            {

                var user = _authService.GetUser() ?? throw new InvalidOperationException("User is null");

                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.DefaultExt = ".dls";


                switch (_loggerCreateViewModel.Context)
                {
                    case LogContext.Qt:
                        {
                            dialog.Filter = "Qt LOG file (.dls)|*.dls|JSON (.json)|*.json";

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var qtCreator = (qtCreateViewModel)_loggerCreateViewModel;
                                _cachemaster.SaveLOGFile(new QtCacheDTO
                                {
                                    UserId = user.Id,
                                    Project = new ProjectDTO
                                    {
                                        ProjectName = qtCreator.ProjectName,
                                        ApplicationName = Constants.Qt
                                    },
                                    Start = qtCreator.StartDate,
                                    End = qtCreator.EndDate,
                                    OutputName = qtCreator.Output,
                                    TypeName = qtCreator.Type,
                                    PostIts = qtCreator.PostIts.Select(p =>
                                    {
                                        var postIt = (CacheStickyPostItViewModel)p;

                                        return new PostItCacheDTO
                                        {
                                            Subject = new SubjectDTO
                                            {
                                                UserId = user.Id,
                                                SubjectName = postIt.Subject
                                            },
                                            Error = postIt.RtfPostIt.Error,
                                            Solution = postIt.RtfPostIt.Solution,
                                            Suggestion = postIt.RtfPostIt.Suggestion,
                                            Comment = postIt.RtfPostIt.Comment,
                                        };
                                    }).ToList(),
                                    Bugs = qtCreator.BugsFound,
                                    Success = qtCreator.ApplicationOpened
                                }, dialog.FileName);
                            }
                            break;
                        }
                    case LogContext.AndroidStudio:
                        {
                            dialog.Filter = "Android LOG file (.dls)|*.dls|JSON (.json)|*.json";

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var androidStudio = (AScodeCreateViewModel)_loggerCreateViewModel;
                                _cachemaster.SaveLOGFile(new AndroidCacheDTO
                                {
                                    UserId = user.Id,
                                    Project = new ProjectDTO
                                    {
                                        ProjectName = androidStudio.ProjectName,
                                        ApplicationName = Constants.Android
                                    },
                                    Start = androidStudio.StartDate,
                                    End = androidStudio.EndDate,
                                    OutputName = androidStudio.Output,
                                    TypeName = androidStudio.Type,
                                    PostIts = androidStudio.PostIts.Select(p =>
                                    {
                                        var postIt = (CacheStickyPostItViewModel)p;

                                        return new PostItCacheDTO
                                        {
                                            Subject = new SubjectDTO
                                            {
                                                UserId = user.Id,
                                                SubjectName = postIt.Subject
                                            },
                                            Error = postIt.RtfPostIt.Error,
                                            Solution = postIt.RtfPostIt.Solution,
                                            Suggestion = postIt.RtfPostIt.Suggestion,
                                            Comment = postIt.RtfPostIt.Comment,
                                        };
                                    }).ToList(),
                                    Bugs = androidStudio.BugsFound,
                                    Success = androidStudio.ApplicationOpened,
                                    Scope = androidStudio.IsSimple,
                                    IsConsidered = androidStudio.IsConsidered,
                                    Sync = androidStudio.SyncTime,
                                    StartingGradleDaemon = androidStudio.GradleDaemonTime,
                                    RunBuild = androidStudio.RunBuildTime,
                                    LoadBuild = androidStudio.LoadBuildTime,
                                    ConfigureBuild = androidStudio.ConfigureBuildTime,
                                    AllProjects = androidStudio.AllProjectsTime
                                }, dialog.FileName);
                            }

                            break;
                        }
                    case LogContext.Coding:
                        {
                            dialog.Filter = "LOG file (.dls)|*.dls|JSON (.json)|*.json";

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var codingCreator = (codeCreateViewModel)_loggerCreateViewModel;
                                _cachemaster.SaveLOGFile(new CodingCacheDTO
                                {
                                    UserId = user.Id,
                                    Project = new ProjectDTO
                                    {
                                        ProjectName = codingCreator.ProjectName,
                                        ApplicationName = codingCreator.ApplicationName
                                    },
                                    ApplicationName = codingCreator.ApplicationName,
                                    Start = codingCreator.StartDate,
                                    End = codingCreator.EndDate,
                                    OutputName = codingCreator.Output,
                                    TypeName = codingCreator.Type,
                                    PostIts = codingCreator.PostIts.Select(p =>
                                    {
                                        var postIt = (CacheStickyPostItViewModel)p;

                                        return new PostItCacheDTO
                                        {
                                            Subject = new SubjectDTO
                                            {
                                                UserId = user.Id,
                                                SubjectName = postIt.Subject
                                            },
                                            Error = postIt.RtfPostIt.Error,
                                            Solution = postIt.RtfPostIt.Solution,
                                            Suggestion = postIt.RtfPostIt.Suggestion,
                                            Comment = postIt.RtfPostIt.Comment,
                                        };
                                    }).ToList(),
                                    Bugs = codingCreator.BugsFound,
                                    Success = codingCreator.ApplicationOpened
                                }, dialog.FileName);
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception found in SaveCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
