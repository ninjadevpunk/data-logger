using Core.DTOs;
using Core.DTOs.Cache;
using Core.Enums;
using Core.Interfaces;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.CacheCommands
{

    internal class AnnotateCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;
        private readonly LoggerViewModel _loggerCreateViewModel;
        private readonly LogCacheViewModel _dashboard;

        private readonly LogContext _context;

        public AnnotateCommand(NavigationService navigationService, LoggerViewModel loggerCreateViewModel, LogCacheViewModel logCacheViewModel,
            IAuthService authService, IDataService dataService, ICacheService cacheService, LogContext type = LogContext.Coding)
        {
            _context = type;
            _navigationService = navigationService;
            _loggerCreateViewModel = loggerCreateViewModel;
            _dashboard = logCacheViewModel;
            _authService = authService;
            _dataService = dataService;
            _cacheService = cacheService;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var userId = _authService.GetUser().Id;


                switch (_context)
                {
                    case LogContext.Qt:
                        {
                            var qtCreateViewModel = (qtCreateViewModel)_loggerCreateViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> postIts = [.. qtCreateViewModel.PostIts.Cast<CacheStickyPostItViewModel>()];

                            var input = new QtCacheDTO
                            {
                                UserId = userId,
                                Project = new ProjectDTO
                                {
                                    ProjectName = qtCreateViewModel.ProjectName,
                                    ApplicationName = Constants.Qt
                                },
                                Start = qtCreateViewModel.StartDate,
                                End = qtCreateViewModel.EndDate,
                                OutputName = qtCreateViewModel.Output,
                                TypeName = qtCreateViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItCacheDTO
                                {
                                    UserId = userId,
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
                                Bugs = qtCreateViewModel.BugsFound,
                                Success = qtCreateViewModel.ApplicationOpened,
                            };

                            var qtCodingViewModel = (CodingQtViewModel)_dashboard;
                            var qtCodingCache = qtCodingViewModel.CacheItems;

                            qtCodingCache.Add(new QtLOGViewModel(_navigationService, input, qtCodingViewModel, _dataService, _cacheService));
                            qtCodingViewModel.CacheItems = qtCodingCache;

                            break;
                        }
                    case LogContext.AndroidStudio:
                        {
                            var androidCreateViewModel = (AScodeCreateViewModel)_loggerCreateViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> postIts = [.. androidCreateViewModel.PostIts.Cast<CacheStickyPostItViewModel>()];

                            var input = new AndroidCacheDTO
                            {
                                UserId = userId,
                                Project = new ProjectDTO
                                {
                                    ProjectName = androidCreateViewModel.ProjectName,
                                    ApplicationName = Constants.Android
                                },
                                Start = androidCreateViewModel.StartDate,
                                End = androidCreateViewModel.EndDate,
                                OutputName = androidCreateViewModel.Output,
                                TypeName = androidCreateViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItCacheDTO
                                {
                                    UserId = userId,
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
                                Bugs = androidCreateViewModel.BugsFound,
                                Success = androidCreateViewModel.ApplicationOpened,
                                Scope = androidCreateViewModel.IsSimple,
                                IsConsidered = androidCreateViewModel.IsConsidered,
                                Sync = androidCreateViewModel.SyncTime,
                                StartingGradleDaemon = androidCreateViewModel.GradleDaemonTime,
                                RunBuild = androidCreateViewModel.RunBuildTime,
                                LoadBuild = androidCreateViewModel.LoadBuildTime,
                                ConfigureBuild = androidCreateViewModel.ConfigureBuildTime,
                                AllProjects = androidCreateViewModel.AllProjectsTime
                            };

                            var androidCodingViewModel = (CodingAndroidViewModel)_dashboard;
                            var androidCache = androidCodingViewModel.CacheItems;

                            androidCache.Add(new AndroidLOGViewModel(_navigationService, (AndroidCacheDTO)input, androidCodingViewModel, _dataService, _cacheService));
                            androidCodingViewModel.CacheItems = androidCache;
                            break;
                        }
                    default:
                        {
                            var codeCreateViewModel = (codeCreateViewModel)_loggerCreateViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> postIts = [.. codeCreateViewModel.PostIts.Cast<CacheStickyPostItViewModel>()];

                            var input = new CodingCacheDTO
                            {
                                UserId = userId,
                                ApplicationName = codeCreateViewModel.ApplicationName,
                                Project = new ProjectDTO
                                {
                                    ProjectName = codeCreateViewModel.ProjectName,
                                    ApplicationName = codeCreateViewModel.ApplicationName
                                },
                                Start = codeCreateViewModel.StartDate,
                                End = codeCreateViewModel.EndDate,
                                OutputName = codeCreateViewModel.Output,
                                TypeName = codeCreateViewModel.Type,
                                Bugs = codeCreateViewModel.BugsFound,
                                Success = codeCreateViewModel.ApplicationOpened,
                                PostIts = postIts.Select(postIt => new PostItCacheDTO
                                {
                                    UserId = userId,
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
                                }).ToList()
                            };

                            var codingViewModel = (CodingViewModel)_dashboard;
                            var codingCache = codingViewModel.CacheItems;

                            codingCache.Add(new CodeLOGViewModel(_navigationService, input, codingViewModel, _dataService, _cacheService));
                            codingViewModel.CacheItems = codingCache;

                            break;
                        }
                }

                await _navigationService.NavigateToLogCachePage(_context);

            }
            catch (TaskCanceledException taskx)
            {
                await _dataService.HandleExceptionAsync(taskx);
                Debug.WriteLine($"Task canceled exception found in AnnotateCommand.Execute(): {taskx.Message}");
                _navigationService.GoBack();
            }
            catch (FormatException formx)
            {
                await _dataService.HandleExceptionAsync(formx);
                if (formx.Message.Equals("The input string '' was not in a correct format."))
                {
                    MessageBox.Show($"An error occurred. Please ensure you entered numbers only in fields that require numeric values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

                Debug.WriteLine($"Format exception found in AnnotateCommand.Execute(): {formx.Message}");

                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Exception found in AnnotateCommand.Execute(): {ex.Message}");
                _navigationService.GoBack();
            }

        }

    }
}
