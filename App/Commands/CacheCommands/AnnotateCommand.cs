using Core.DTOs.Cache;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
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

        private ApplicationClass? QtCreator;
        private ApplicationClass? AndroidStudio;
        private readonly LogContext _type;

        public AnnotateCommand(NavigationService navigationService, LoggerViewModel loggerCreateViewModel, LogCacheViewModel logCacheViewModel,
            IAuthService authService, IDataService dataService, ICacheService cacheService, LogContext type = LogContext.Coding)
        {
            _type = type;
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "AnnotateCommand");
            _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel), "AnnotateCommand");
            _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel), "AnnotateCommand");
            _authService = authService ?? throw new ArgumentNullException(nameof(authService), "AnnotateCommand");
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService), "AnnotateCommand");
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService), "AnnotateCommand");
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (_type == LogContext.Qt)
                    QtCreator = await _dataService.FindApplicationByIDAsync(1);

                if (_type == LogContext.AndroidStudio)
                    AndroidStudio = await _dataService.FindApplicationByIDAsync(2);

                var userId = _authService.GetUser().UserId;
                var codeCreateViewModel = (codeCreateViewModel)_loggerCreateViewModel;

                switch(_type)
                {
                    case LogContext.Qt:
                        {
                            var input = new QtCacheDTO
                            {
                                UserId = userId,
                                ProjectName = codeCreateViewModel.ProjectName,
                                Start = codeCreateViewModel.StartDate,
                                End = codeCreateViewModel.EndDate,
                                Bugs = codeCreateViewModel.BugsFound,
                                Success = codeCreateViewModel.ApplicationOpened,
                                PostIts = codeCreateViewModel.PostIts.Select(postIt => new PostItCacheDTO
                                {
                                    Subject = postIt.Subject,
                                    Error = postIt.Error,
                                    Solution = postIt.Solution,
                                    Suggestion = postIt.Suggestion,
                                    Comment = postIt.Comment,
                                    ERCaptureTime = DateCapturer(postIt.Error),
                                    SOCaptureTime = DateCapturer(postIt.Solution)
                                }).ToList()
                            };

                            var qtCodingViewModel = (CodingQtViewModel)_dashboard;
                            var qtCodingCache = qtCodingViewModel.CacheItems;

                            qtCodingCache.Add(new QtLOGViewModel(_navigationService, input, qtCodingViewModel, _dataService, _cacheService));
                            qtCodingViewModel.CacheItems = qtCodingCache;

                            break;
                        }
                    case LogContext.AndroidStudio:
                        {
                            var input = new AndroidCacheDTO
                            {
                                UserId = userId,
                                ProjectName = codeCreateViewModel.ProjectName,
                                Start = codeCreateViewModel.StartDate,
                                End = codeCreateViewModel.EndDate,
                                Bugs = codeCreateViewModel.BugsFound,
                                Success = codeCreateViewModel.ApplicationOpened,
                                PostIts = codeCreateViewModel.PostIts.Select(postIt => new PostItCacheDTO
                                {
                                    Subject = postIt.Subject,
                                    Error = postIt.Error,
                                    Solution = postIt.Solution,
                                    Suggestion = postIt.Suggestion,
                                    Comment = postIt.Comment,
                                    ERCaptureTime = DateCapturer(postIt.Error),
                                    SOCaptureTime = DateCapturer(postIt.Solution)
                                }).ToList(),
                                Scope = ((AScodeCreateViewModel)codeCreateViewModel).IsSimple,
                                Sync = ((AScodeCreateViewModel)codeCreateViewModel).SyncTime,
                                StartingGradleDaemon = ((AScodeCreateViewModel)codeCreateViewModel).GradleDaemonTime,
                                RunBuild = ((AScodeCreateViewModel)codeCreateViewModel).RunBuildTime,
                                LoadBuild = ((AScodeCreateViewModel)codeCreateViewModel).LoadBuildTime,
                                ConfigureBuild = ((AScodeCreateViewModel)codeCreateViewModel).ConfigureBuildTime,
                                AllProjects = ((AScodeCreateViewModel)codeCreateViewModel).AllProjectsTime
                            };

                            var androidCodingViewModel = (CodingAndroidViewModel)_dashboard;
                            var androidCache = androidCodingViewModel.CacheItems;

                            androidCache.Add(new AndroidLOGViewModel(_navigationService, (AndroidCacheDTO)input, androidCodingViewModel, _dataService, _cacheService));
                            androidCodingViewModel.CacheItems = androidCache;
                            break;
                        }
                    default:
                        {
                            var input = new CodingCacheDTO
                            {
                                UserId = userId,
                                ApplicationName = codeCreateViewModel.ApplicationName,
                                ProjectName = codeCreateViewModel.ProjectName,
                                Start = codeCreateViewModel.StartDate,
                                End = codeCreateViewModel.EndDate,
                                Bugs = codeCreateViewModel.BugsFound,
                                Success = codeCreateViewModel.ApplicationOpened,
                                PostIts = codeCreateViewModel.PostIts.Select(postIt => new PostItCacheDTO
                                {
                                    Subject = postIt.Subject,
                                    Error = postIt.Error,
                                    Solution = postIt.Solution,
                                    Suggestion = postIt.Suggestion,
                                    Comment = postIt.Comment,
                                    ERCaptureTime = DateCapturer(postIt.Error),
                                    SOCaptureTime = DateCapturer(postIt.Solution)
                                }).ToList()
                            };

                            var codingViewModel = (CodingViewModel)_dashboard;
                            var codingCache = codingViewModel.CacheItems;

                            codingCache.Add(new CodeLOGViewModel(_navigationService, input, codingViewModel, _dataService, _cacheService));
                            codingViewModel.CacheItems = codingCache;

                            break;
                        }
                }

            }
            catch (InvalidCastException castx)
            {
                await _dataService.HandleExceptionAsync(castx);
                Debug.WriteLine($"Invalid cast canceled exception found in AnnotateCommand.Execute(): {castx.Message}");
                _navigationService.GoBack(false);
            }
            catch (TaskCanceledException taskx)
            {
                await _dataService.HandleExceptionAsync(taskx);
                Debug.WriteLine($"Task canceled exception found in AnnotateCommand.Execute(): {taskx.Message}");
                _navigationService.GoBack(false);
            }
            catch (ArgumentNullException nullx)
            {
                await _dataService.HandleExceptionAsync(nullx);
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Argument null exception found in AnnotateCommand.Execute(): {nullx.Message}");

                _navigationService.GoBack(false);
            }
            catch (IndexOutOfRangeException index)
            {
                await _dataService.HandleExceptionAsync(index);
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Index out of range exception found in AnnotateCommand.Execute(): {index.Message}");

                _navigationService.GoBack(false);
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

                _navigationService.GoBack(false);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Exception found in AnnotateCommand.Execute(): {ex.Message}");

                _navigationService.GoBack(false);
            }

        }

        private DateTime DateCapturer(string fieldContent) => string.IsNullOrWhiteSpace(RtfConverter.ToPlainText(fieldContent).Text) ? default :
            DateTime.Now;
    }
}
