using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Data_Logger_1._3.ViewModels.Reporter.Result;
using Data.Interfaces;
using Core.Enums;
using Core.DTOs.Data;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Performs a query on the database.
    /// </summary>
    internal class SearchCommand : AsyncCommandBase
    {
        private readonly ReportDeskViewModel _reportDesk;
        private readonly IDataService _dataService;
        private readonly NavigationService _navigationService;
        private readonly LogContext _context;

        public SearchCommand(ReportDeskViewModel reportDesk, IDataService dataService, NavigationService navigationService, LogContext logContext)
        {
            _reportDesk = reportDesk;
            _navigationService = navigationService;
            _dataService = dataService;
            _context = logContext;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // TODO
                string projectFilter = _reportDesk.Project;
                string appFilter = _reportDesk.Application;


                switch (_context)
                {
                    case LogContext.Qt:
                        {
                            List<QtDataDTO> list = [];

                            var qtReportDesk = (QtReportDeskViewModel)_reportDesk;

                            if (qtReportDesk.Project == "NONE")
                            {
                                qtReportDesk.SearchBarItems.Clear();
                                return;
                            }

                            if (qtReportDesk.Project == "ANY" && !string.IsNullOrEmpty(qtReportDesk.Query))
                            {
                                list = await _dataService.SearchQtLogsAsync(qtReportDesk.Query) ?? [];
                            }
                            else
                            {
                                //list = string.IsNullOrEmpty(projectFilter) || string.IsNullOrEmpty(appFilter) ?
                                //    throw new ArgumentNullException("Project not found.") :
                                //    await _dataService.SearchQtLogsAsync(qtReportDesk.Query, projectFilter, appFilter) ?? [];
                            }

                            var items = new ObservableCollection<SearchResultViewModel>();

                            if (list.Count == 0)
                                return;

                            foreach (var record in list)
                            {
                                items.Add(new qt_SearchResultViewModel(_navigationService, _dataService, record, qtReportDesk));
                            }

                            qtReportDesk.SearchBarItems = items;

                            break;
                        }
                    case LogContext.AndroidStudio:
                        {
                            // TODO: Handle Android case
                            break;
                        }
                    case LogContext.Graphics:
                        {
                            break;
                        }
                    case LogContext.Film:
                        {
                            break;
                        }
                    case LogContext.Flexi:
                        {
                            break;
                        }
                    default:
                        {
                            List<CodingDataDTO> list = [];

                            var codeReportDesk = (CodeReportDeskViewModel)_reportDesk;

                            if (codeReportDesk.Project == "NONE")
                            {
                                codeReportDesk.SearchBarItems.Clear();
                                return;
                            }

                            if (!string.IsNullOrEmpty(codeReportDesk.Query))
                            {
                                if (codeReportDesk.Project == "ANY" && codeReportDesk.Application == "ANY")
                                {
                                    list = await _dataService.SearchCodingLogsAsync(codeReportDesk.Query);
                                }
                                else if (codeReportDesk.Project == "ANY")
                                {
                                    var appId = (await _dataService.FindApplicationAsync(appFilter))?.appID ?? -1;
                                    list = await _dataService.SearchCodingLogsAsync(codeReportDesk.Query, appId);
                                }
                                else
                                {
                                    var appId = (await _dataService.FindApplicationAsync(appFilter))?.appID ?? -1;
                                    var projectId = (await _dataService.FindProjectAsync(projectFilter, appId))?.projectID ?? -1;
                                    list = await _dataService.SearchCodingLogsAsync(codeReportDesk.Query, projectId, appId);
                                }
                            }


                            var items = new ObservableCollection<SearchResultViewModel>();

                            if (!list.Any())
                            {
                                codeReportDesk.SearchBarItems.Clear();
                                return;
                            }

                            foreach (var record in list)
                            {
                                items.Add(new code_SearchResultViewModel(_navigationService, _dataService, record, codeReportDesk));
                            }

                            codeReportDesk.SearchBarItems = items;

                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception occurred near SearchCommand.Execute(): {ex.Message}");
            }
        }
    }

}
