using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    /// <summary>
    /// This class will be used to update database logs from the log editor.
    /// </summary>
    public class UpdateCommand : AsyncCommandBase
    {
        private readonly CacheContext? _cacheContext = CacheContext.Coding;
        private readonly ReporterUpdaterViewModel? _editor;
        private readonly ReportDeskViewModel? _desk;
        private readonly LOG? _log;
        private readonly NavigationService? _navigationService;
        private readonly IDataService? _dataService;


        public UpdateCommand()
        {

        }

        public UpdateCommand(CacheContext context, ReporterUpdaterViewModel reporterEditorViewModel, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService, IDataService dataService, LOG log)
        {
            try
            {
                _cacheContext = context;
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _editor = reporterEditorViewModel ?? throw new ArgumentNullException(nameof(reporterEditorViewModel));
                _desk = reportDeskViewModel ?? throw new ArgumentNullException(nameof(reportDeskViewModel));
                _log = log ?? throw new ArgumentNullException(nameof(log));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near UpdateCommand(context, reportEditorViewModel, reportDeskViewModel) constructor: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (_navigationService == null || _dataService == null || _editor == null || _log == null || _desk == null)
                    throw new ArgumentNullException("Important properties cannot be null.");
                var account = _dataService.GetUser();
                var userID = account.accountID;


                bool appIsNew = false;
                bool projectIsNew = false;

                // APPLICATION

                if (string.IsNullOrEmpty(_editor.ApplicationName))
                {
                    // Set to "Unknown" default
                    _log.appID = 3;
                    _log.Application = null;
                }
                else if (_log.Application.Name != _editor.ApplicationName)
                {
                    // App may be the same or changed. Changed app may be new or existing
                    var app = await _dataService.FindApplication(userID, _editor.ApplicationName);

                    if (app == null)
                    {
                        app = new ApplicationClass(_editor.ApplicationName);
                        app.accountID = _log.accountID;
                    }

                    appIsNew = app.appID == 0;

                    if (appIsNew)
                        _log.Application = app;
                    else
                    {
                        _log.appID = app.appID;
                        _log.Application = null;
                    }
                }



                // PROJECT

                if (!appIsNew && _log.Project != null)
                {
                    bool projectBelongsToNewApp = _log.Project.appID == _log.appID;

                    if (string.IsNullOrEmpty(_editor.ProjectName))
                    {
                        // Set to "Unnamed Project" default
                        _log.projectID = 1;
                        _log.Project = null;
                    }
                    else if (_log.Project.Name != _editor.ProjectName || !projectBelongsToNewApp)
                    {
                        var project = await _dataService.FindProject(account.accountID, _editor.ProjectName, _log.appID);

                        if (project == null)
                        {
                            project = new ProjectClass(_editor.ProjectName);
                            project.accountID = account.accountID;
                            project.appID = _log.appID;
                        }

                        projectIsNew = project.projectID == 0;

                        if (project.projectID == 0)
                            _log.Project = project;
                        else
                        {
                            _log.projectID = project.projectID;
                            _log.Project = null;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_editor.ProjectName))
                    {
                        // Set to "Unnamed Project" default
                        _log.projectID = 1;
                        _log.Project = null;
                    }
                    else
                    {
                        _log.Project = new ProjectClass(account, _editor.ProjectName, _log.Application!);
                        projectIsNew = true;
                    }
                }


                _log.Start = _editor.StartDate;
                _log.End = _editor.EndDate;

                if (_log.Output.Name != _editor.Output)
                {
                    _log.Output = await _dataService.FindOutput(_editor.Output) ?? new(41, "EXE (*.exe)");
                }

                if (_log.Type.Name != _editor.Type)
                {
                    _log.Type = await _dataService.FindType(_editor.Type) ?? new(39, "Note");
                }

                foreach (var editorPostIt in _editor.PostIts)
                {
                    var subjectIsNew = false;
                    var subject = await _dataService.FindSubject(editorPostIt.Subject, _log.Category, _log.appID, _log.projectID);

                    if (subject == null || appIsNew || projectIsNew)
                    {
                        subjectIsNew = true;
                        subject = new(
                                      _log.Category,
                                      account,
                                      editorPostIt.Subject);
                    }


                    if (editorPostIt.ID == 0)
                    {
                        PostIt newPostIt;

                        if (subjectIsNew)
                        {
                            newPostIt = new PostIt
                            {
                                // Both PostIt and Subject is NEW
                                Subject = subject,
                                Error = editorPostIt.Error,
                                ERCaptureTime = editorPostIt.DateFound,
                                Solution = editorPostIt.Solution,
                                SOCaptureTime = editorPostIt.DateSolved,
                                Suggestion = editorPostIt.Suggestion,
                                Comment = editorPostIt.Comment,
                                logID = _log.ID,
                                accountID = _log.accountID
                            };
                        }
                        else
                        {
                            // PostIt is NEW, Subject EXISTS
                            newPostIt = new PostIt
                            {
                                subjectID = subject.subjectID,
                                Error = editorPostIt.Error,
                                ERCaptureTime = editorPostIt.DateFound,
                                Solution = editorPostIt.Solution,
                                SOCaptureTime = editorPostIt.DateSolved,
                                Suggestion = editorPostIt.Suggestion,
                                Comment = editorPostIt.Comment,
                                logID = _log.ID,
                                accountID = _log.accountID
                            };
                        }

                        _log.PostItList.Add(newPostIt);
                    }
                    else
                    {
                        var existingPostIt = _log.PostItList.First(p => p.postItID == editorPostIt.ID);

                        // existingPostIt.Subject is NOT null here

                        if (subjectIsNew)
                        {
                            // PostIt Exists, Subject is NEW
                            existingPostIt.Subject = subject;
                        }
                        else
                        {
                            // PostIt Exists, Subject Exists
                            existingPostIt.subjectID = subject.subjectID;
                            existingPostIt.Subject = null;
                        }

                        existingPostIt.Error = editorPostIt.Error;
                        existingPostIt.ERCaptureTime = editorPostIt.DateFound;
                        existingPostIt.Solution = editorPostIt.Solution;
                        existingPostIt.SOCaptureTime = editorPostIt.DateSolved;
                        existingPostIt.Suggestion = editorPostIt.Suggestion;
                        existingPostIt.Comment = editorPostIt.Comment;
                    }
                }

                // Remove PostIts deleted in the editor
                var editorIds = _editor.PostIts.Select(p => p.ID).ToList();
                var toRemove = _log.PostItList.Where(p => !editorIds.Contains(p.postItID)).ToList();

                foreach (var removed in toRemove)
                    _log.PostItList.Remove(removed);

                var postIts = new ObservableCollection<EF_PostItViewModel>(_editor.PostIts);

                _log.Content = REPORTViewModel.EF_PostItContent(postIts);

                switch (_cacheContext)
                {
                    case CacheContext.Qt:
                        {





                            break;
                        }
                    default:
                        {
                            var _codingLog = (CodingLOG)_log;
                            var editor = (codeUpdateViewModel)_editor;

                            _codingLog.Bugs = editor.BugsFound;
                            _codingLog.Success = editor.ApplicationOpened;

                            var desk = (CodeReportDeskViewModel)_desk;


                            break;
                        }
                }


                await _desk.UpdateLogsListAsync();

                await _dataService.UpdateLogAsync(_log);
                await _navigationService.NavigateToReporter();



            }
            catch (InvalidCastException castx)
            {
                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(castx, "ExecuteAsync(parameter)", "InvalidCastException");
            }
            catch (TaskCanceledException taskx)
            {
                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(taskx, "ExecuteAsync(parameter)", "TaskCanceledException");
            }
            catch (ArgumentNullException nullx)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(nullx, "ExecuteAsync(parameter)", "ArgumentNullException");

            }
            catch (IndexOutOfRangeException index)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(index, "ExecuteAsync(parameter)", "IndexOutOfRangeException");

            }
            catch (FormatException formx)
            {
                if (formx.Message.Equals("The input string '' was not in a correct format."))
                {
                    MessageBox.Show($"An error occurred. Please ensure you entered numbers only in fields that require numeric values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(formx, "ExecuteAsync(parameter)", "FormatException");

            }
            catch (Exception ex)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                if (_dataService != null)
                    await _dataService.HandleExceptionAsync(ex, "ExecuteAsync(parameter)");

            }
        }
    }
}
