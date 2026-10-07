using Core.DTOs;
using Core.DTOs.Cache;
using Core.Enums;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Reflection.Metadata;

namespace Data_Logger_1._3.Commands.EditCacheCommands
{
    /// <summary>
    /// Caching an edited cached log.
    /// </summary>
    internal class EditAnnotateCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly LoggerViewModel _loggerEditViewModel;
        private readonly LogCacheViewModel _logCacheViewModel;
        private readonly LOGViewModel _logViewModel;
        private readonly LogCacheDTO _logCacheDTO;
        private readonly int _timeRemaining;


        public EditAnnotateCommand(NavigationService navigationService, IDataService dataService, ICacheService cacheService,
            LoggerViewModel loggerEditViewModel, LogCacheViewModel logCacheViewModel, LOGViewModel logViewModel, LogCacheDTO logCacheDTO, int timeRemaining)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _cacheService = cacheService;
            _loggerEditViewModel = loggerEditViewModel;
            _logCacheViewModel = logCacheViewModel;
            _logViewModel = logViewModel;
            _logCacheDTO = logCacheDTO;
            _timeRemaining = timeRemaining;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var userId = _logCacheDTO.UserId;

                switch (_logCacheDTO.Context)
                {
                    case LogContext.Qt when _logCacheDTO is QtCacheDTO qtCacheDTO:
                        {
                            var qtLogViewModel = (QtLOGViewModel)_logViewModel;
                            var qtEditViewModel = (qtEditViewModel)_loggerEditViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> postIts = [.. qtEditViewModel.PostIts.Cast<CacheStickyPostItViewModel>()];

                            qtCacheDTO = new QtCacheDTO
                            {
                                UserId = _logCacheDTO.UserId,
                                Project = new ProjectDTO
                                {
                                    ProjectName = qtEditViewModel.ProjectName,
                                    ApplicationName = Constants.Qt
                                },
                                Start = qtEditViewModel.StartDate,
                                End = qtEditViewModel.EndDate,
                                OutputName = qtEditViewModel.Output,
                                TypeName = qtEditViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItCacheDTO
                                {
                                    UserId = _logCacheDTO.UserId,
                                    Subject = new SubjectDTO
                                    {
                                        UserId = userId,
                                        SubjectName = postIt.Subject
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                }).ToList(),
                                Bugs = qtEditViewModel.BugsFound,
                                Success = qtEditViewModel.ApplicationOpened
                            };

                            if (_logCacheViewModel is not CodingQtViewModel codingQtViewModel)
                                throw new InvalidOperationException("The log cache view model is not a CodingQtViewModel.");
                            var qtCodingCache = codingQtViewModel.CacheItems;
                            var index = qtCodingCache.IndexOf(qtLogViewModel);
                            var newQtLogViewModel = new QtLOGViewModel(_navigationService, qtCacheDTO, codingQtViewModel, _dataService,
                                _cacheService)
                            {
                                TimeRemaining = _timeRemaining
                            };
                            qtLogViewModel.Dispose();
                            qtCodingCache[index] = newQtLogViewModel;

                            codingQtViewModel.CacheItems = qtCodingCache;

                            break;
                        }
                    case LogContext.AndroidStudio when _logCacheDTO is AndroidCacheDTO androidCacheDTO:
                        {
                            var androidLogViewModel = (AndroidLOGViewModel)_logViewModel;
                            var androidEditViewModel = (AScodeEditViewModel)_loggerEditViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> postIts = [.. androidEditViewModel.PostIts.Cast<CacheStickyPostItViewModel>()];

                            androidCacheDTO = new AndroidCacheDTO
                            {
                                UserId = _logCacheDTO.UserId,
                                Project = new ProjectDTO
                                {
                                    ProjectName = androidEditViewModel.ProjectName,
                                    ApplicationName = Constants.Android
                                },
                                Start = androidEditViewModel.StartDate,
                                End = androidEditViewModel.EndDate,
                                OutputName = androidEditViewModel.Output,
                                TypeName = androidEditViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItCacheDTO
                                {
                                    UserId = _logCacheDTO.UserId,
                                    Subject = new SubjectDTO
                                    {
                                        UserId = userId,
                                        SubjectName = postIt.Subject
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                }).ToList(),
                                Bugs = androidEditViewModel.BugsFound,
                                Success = androidEditViewModel.ApplicationOpened,
                                Scope = androidEditViewModel.IsSimple,
                                IsConsidered = androidEditViewModel.IsConsidered,
                                Sync = androidEditViewModel.SyncTime,
                                StartingGradleDaemon = androidEditViewModel.GradleDaemonTime,
                                RunBuild = androidEditViewModel.RunBuildTime,
                                LoadBuild = androidEditViewModel.LoadBuildTime,
                                ConfigureBuild = androidEditViewModel.ConfigureBuildTime,
                                AllProjects = androidEditViewModel.AllProjectsTime
                            };

                            if (_logCacheViewModel is not CodingAndroidViewModel codingAndroidViewModel)
                                throw new InvalidOperationException("The log cache view model is not a CodingAndroidViewModel.");
                            var androidCodingCache = codingAndroidViewModel.CacheItems;
                            var index = androidCodingCache.IndexOf(androidLogViewModel);
                            var newAndroidLogViewModel = new AndroidLOGViewModel(_navigationService, androidCacheDTO, codingAndroidViewModel,
                                _dataService, _cacheService)
                            {
                                TimeRemaining = _timeRemaining
                            };
                            androidLogViewModel.Dispose();
                            androidCodingCache[index] = newAndroidLogViewModel;

                            codingAndroidViewModel.CacheItems = androidCodingCache;

                            break;
                        }
                    default:
                        {
                            var codeLogViewModel = (CodeLOGViewModel)_logViewModel;
                            var codeEditViewModel = (codeEditViewModel)_loggerEditViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> postIts = [.. codeEditViewModel.PostIts.Cast<CacheStickyPostItViewModel>()];

                            CodingCacheDTO codingCacheDTO = new CodingCacheDTO
                            {
                                UserId = _logCacheDTO.UserId,
                                ApplicationName = codeEditViewModel.ApplicationName,
                                Project = new ProjectDTO
                                {
                                    ProjectName = codeEditViewModel.ProjectName,
                                    ApplicationName = codeEditViewModel.ApplicationName
                                },
                                Start = codeEditViewModel.StartDate,
                                End = codeEditViewModel.EndDate,
                                OutputName = codeEditViewModel.Output,
                                TypeName = codeEditViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItCacheDTO
                                {
                                    UserId = _logCacheDTO.UserId,
                                    Subject = new SubjectDTO
                                    {
                                        UserId = userId,
                                        SubjectName = postIt.Subject
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                }).ToList(),
                                Bugs = codeEditViewModel.BugsFound,
                                Success = codeEditViewModel.ApplicationOpened
                            };

                            if (_logCacheViewModel is not CodingViewModel codingViewModel)
                                throw new InvalidOperationException("The log cache view model is not a CodingViewModel.");
                            var codingCache = codingViewModel.CacheItems;

                            var index = codingCache.IndexOf(codeLogViewModel);
                            var newCodeLogViewModel = new CodeLOGViewModel(_navigationService, codingCacheDTO, codingViewModel,
                                _dataService, _cacheService)
                            {
                                TimeRemaining = _timeRemaining
                            };
                            codeLogViewModel.Dispose();
                            codingCache[index] = newCodeLogViewModel;

                            codingViewModel.CacheItems = codingCache;

                            break;
                        }
                }


                await _navigationService.NavigateToLogCachePage(_logCacheDTO.Context);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }

    }
}
