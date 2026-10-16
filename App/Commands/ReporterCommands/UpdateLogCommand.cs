using Core.DTOs.Data;
using Core.Enums;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Allows the user to edit a log retrieved from the database.
    /// </summary>
    internal class UpdateLogCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly LoggerViewModel _loggerUpdateViewModel;
        private readonly LogDataDTO _logDataDTO;



        public UpdateLogCommand(NavigationService navigationService, IDataService dataService, ReportDeskViewModel reportDeskViewModel, 
            LoggerViewModel loggerUpdateViewModel, LogDataDTO logDataDTO)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _reportDeskViewModel = reportDeskViewModel;
            _loggerUpdateViewModel = loggerUpdateViewModel;
            _logDataDTO = logDataDTO;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var userId = _logDataDTO.UserId;

                switch (_logDataDTO.Context)
                {
                    case LogContext.Qt when _logDataDTO is QtDataDTO qtDataDTO:
                        {
                            var qtUpdaterViewModel = (qtUpdateViewModel)_loggerUpdateViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> cachedDataPostIts = [.. qtUpdaterViewModel.PostIts.OfType<CacheStickyPostItViewModel>()];
                            IReadOnlyList<DataStickyPostItViewModel> postIts = [.. qtUpdaterViewModel.PostIts.OfType<DataStickyPostItViewModel>()];
                            bool projectHasChanged = _logDataDTO.Project.ProjectName != qtUpdaterViewModel.ProjectName, 
                                outputHasChanged = _logDataDTO.OutputName != qtUpdaterViewModel.Output,
                                typeHasChanged = _logDataDTO.TypeName != qtUpdaterViewModel.Type;
                            int? projectId = null, outputId = null, typeId = null;

                            if (projectHasChanged)
                            {
                                projectId = (await _dataService.FindProjectAsync(qtUpdaterViewModel.ProjectName, Constants.Qt))?.projectID;
                            }
                            else
                                projectId = _logDataDTO.ProjectId;

                            if (outputHasChanged)
                                outputId = (await _dataService.FindOutputAsync(qtUpdaterViewModel.Output))?.outputID;
                            else
                                outputId = _logDataDTO.OutputID;

                            if (typeHasChanged)
                                typeId = (await _dataService.FindTypeAsync(qtUpdaterViewModel.Type))?.typeID;
                            else
                                typeId = _logDataDTO.TypeID;

                            qtDataDTO = new QtDataDTO
                            {
                                LogId = _logDataDTO.LogId,
                                UserId = _logDataDTO.UserId,
                                ProjectId = projectId ?? default,
                                Project = projectHasChanged ? new ProjectDataDTO
                                {
                                    ProjectId = projectId ?? default,
                                    ProjectName = qtUpdaterViewModel.ProjectName,
                                    AppId = _logDataDTO.AppId,
                                    ApplicationName = Constants.Qt,
                                } : _logDataDTO.Project,
                                Start = qtUpdaterViewModel.StartDate,
                                End = qtUpdaterViewModel.EndDate,
                                OutputID = outputHasChanged ? outputId ?? default: _logDataDTO.OutputID,
                                OutputName = qtUpdaterViewModel.Output,
                                TypeID = typeHasChanged ? typeId ?? default : _logDataDTO.TypeID,
                                TypeName = qtUpdaterViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItDataDTO
                                {
                                    LogId = _logDataDTO.LogId,
                                    UserId = _logDataDTO.UserId,
                                    PostItId = postIt.RtfPostIt.PostItId,
                                    SubjectId = postIt.RtfPostIt.SubjectId,
                                    Subject = new SubjectDataDTO
                                    {
                                        SubjectId = postIt.RtfPostIt.SubjectId,
                                        UserId = userId,
                                        SubjectName = postIt.Subject,
                                        ProjectId = projectHasChanged ? projectId ?? default : _logDataDTO.ProjectId,
                                        AppId = _logDataDTO.AppId
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                })
                                // New PostIts
                                .Concat(cachedDataPostIts.Select(postIt => new PostItDataDTO
                                {
                                    LogId = _logDataDTO.LogId,
                                    UserId = _logDataDTO.UserId,
                                    Subject = new SubjectDataDTO
                                    {
                                        UserId = userId,
                                        SubjectName = postIt.Subject,
                                        ProjectId = projectHasChanged ? projectId ?? default : _logDataDTO.ProjectId,
                                        AppId = _logDataDTO.AppId
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                })).ToList(),
                                Bugs = qtUpdaterViewModel.BugsFound,
                                Success = qtUpdaterViewModel.ApplicationOpened
                            };

                            await _dataService.UpdateLogAsync(qtDataDTO);

                            break;
                        }
                    case LogContext.AndroidStudio when _logDataDTO is AndroidDataDTO androidDataDTO:
                        {
                            // TODO: Handle Android case
                            break;
                        }
                    default:
                        {
                            var codingDataDTO = _logDataDTO as CodingDataDTO ?? throw new InvalidOperationException("Coding data DTO required!");
                            var codingUpdaterViewModel = (codeUpdateViewModel)_loggerUpdateViewModel;
                            IReadOnlyList<CacheStickyPostItViewModel> cachedDataPostIts = [.. codingUpdaterViewModel.PostIts.OfType<CacheStickyPostItViewModel>()];
                            IReadOnlyList<DataStickyPostItViewModel> postIts = [.. codingUpdaterViewModel.PostIts.OfType<DataStickyPostItViewModel>()];
                            bool appHasChanged = _logDataDTO.ApplicationName != codingUpdaterViewModel.ApplicationName, 
                                projectHasChanged = _logDataDTO.Project.ProjectName != codingUpdaterViewModel.ProjectName,
                                outputHasChanged = _logDataDTO.OutputName != codingUpdaterViewModel.Output,
                                typeHasChanged = _logDataDTO.TypeName != codingUpdaterViewModel.Type;
                            int? appId = null, projectId = default, outputId = null, typeId = null;

                            if (appHasChanged)
                            {
                                appId = (await _dataService.FindApplicationAsync(codingUpdaterViewModel.ApplicationName))?.appID;
                                projectHasChanged = true;

                                if (appId.HasValue)
                                    projectId = (await _dataService.FindProjectAsync(codingUpdaterViewModel.ProjectName, 
                                        codingUpdaterViewModel.ApplicationName))?.projectID;
                            }
                            else
                            {
                                appId = _logDataDTO.AppId;

                                if (projectHasChanged)
                                    projectId = (await _dataService.FindProjectAsync(codingUpdaterViewModel.ProjectName, codingUpdaterViewModel.ApplicationName))?.projectID;
                                else
                                    projectId = _logDataDTO.ProjectId;
                            }

                            if (outputHasChanged)
                                outputId = (await _dataService.FindOutputAsync(codingUpdaterViewModel.Output))?.outputID;
                            else
                                outputId = _logDataDTO.OutputID;

                            if (typeHasChanged)
                                typeId = (await _dataService.FindTypeAsync(codingUpdaterViewModel.Type))?.typeID;
                            else
                                typeId = _logDataDTO.TypeID;

                            codingDataDTO = new CodingDataDTO
                            {
                                LogId = _logDataDTO.LogId,
                                UserId = _logDataDTO.UserId,
                                AppId = appId ?? default,
                                ApplicationName = codingUpdaterViewModel.ApplicationName,
                                ProjectId = projectId ?? default,
                                Project = projectHasChanged ? new ProjectDataDTO
                                {
                                    ProjectId = projectId ?? default,
                                    ProjectName = codingUpdaterViewModel.ProjectName,
                                    AppId = appId ?? default,
                                    ApplicationName = codingUpdaterViewModel.ApplicationName,
                                } : _logDataDTO.Project,
                                Start = codingUpdaterViewModel.StartDate,
                                End = codingUpdaterViewModel.EndDate,
                                OutputID = outputHasChanged ? outputId ?? default : _logDataDTO.OutputID,
                                OutputName = codingUpdaterViewModel.Output,
                                TypeID = typeHasChanged ? typeId ?? default : _logDataDTO.TypeID,
                                TypeName = codingUpdaterViewModel.Type,
                                PostIts = postIts.Select(postIt => new PostItDataDTO
                                {
                                    LogId = _logDataDTO.LogId,
                                    UserId = _logDataDTO.UserId,
                                    PostItId = postIt.RtfPostIt.PostItId,
                                    SubjectId = postIt.RtfPostIt.SubjectId,
                                    Subject = new SubjectDataDTO
                                    {
                                        SubjectId = postIt.RtfPostIt.SubjectId,
                                        UserId = userId,
                                        SubjectName = postIt.Subject,
                                        ProjectId = projectId ?? default,
                                        AppId = appId ?? default
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                })
                                // New PostIts
                                .Concat(cachedDataPostIts.Select(postIt => new PostItDataDTO
                                {
                                    LogId = _logDataDTO.LogId,
                                    UserId = _logDataDTO.UserId,
                                    Subject = new SubjectDataDTO
                                    {
                                        UserId = userId,
                                        SubjectName = postIt.Subject,
                                        ProjectId = projectId ?? default,
                                        AppId = appId ?? default
                                    },
                                    Error = postIt.RtfPostIt.Error,
                                    Solution = postIt.RtfPostIt.Solution,
                                    Suggestion = postIt.RtfPostIt.Suggestion,
                                    Comment = postIt.RtfPostIt.Comment,
                                    ERCaptureTime = postIt.RtfPostIt.ERCaptureTime,
                                    SOCaptureTime = postIt.RtfPostIt.SOCaptureTime
                                })).ToList(),
                                Bugs = codingUpdaterViewModel.BugsFound,
                                Success = codingUpdaterViewModel.ApplicationOpened
                            };

                            await _dataService.UpdateLogAsync(codingDataDTO);

                            break;
                        }
                }

                await _reportDeskViewModel.UpdateLogsListAsync();
                await _navigationService.NavigateToReporter();
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near EditCommand.Execute(): {ex.Message}");
            }
        }
    }

}
