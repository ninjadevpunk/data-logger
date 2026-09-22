using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Reporter.Result;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Searches for the query inside the database.
    /// </summary>
    public class SearchCommand : AsyncCommandBase
    {
        private readonly ReportDeskViewModel _reportDesk;
        private readonly IDataService _dataService;
        private readonly NavigationService _navigationService;

        public CacheContext Context { get; set; } = CacheContext.Coding;

        public SearchCommand(ReportDeskViewModel reportDesk, IDataService dataService, NavigationService navigationService, CacheContext cacheContext)
        {
            _reportDesk = reportDesk ?? throw new ArgumentNullException(nameof(reportDesk));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            Context = cacheContext;
        }

        public SearchCommand()
        {
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                ProjectClass projectFilter = null;
                ApplicationClass appFilter = null;

                await _dataService.InitialiseProjectsLISTAsync();
                if (_reportDesk.Project != "ANY" && _reportDesk.Application != "ANY")
                {
                    foreach (ProjectClass project in _dataService.SQLITE_PROJECTS)
                    {
                        if (_reportDesk.Project == project.Name && _reportDesk.Application == project.Application.Name)
                        {
                            projectFilter = project;
                            appFilter = project.Application;

                            break;
                        }
                    }
                }
                else if (_reportDesk.Application != "ANY")
                {
                    foreach (ApplicationClass app in _dataService.SQLITE_APPLICATIONS)
                    {
                        if (_reportDesk.Application == app.Name)
                        {
                            appFilter = app;
                        }
                    }
                }

                switch (Context)
                {
                    case CacheContext.Qt:
                        {
                            List<CodingLOG> list;

                            var qtReportDesk = (CodeReportDeskViewModel)_reportDesk;

                            if (qtReportDesk.Project == "NONE")
                            {
                                qtReportDesk.SearchBarItems.Clear();
                                return;
                            }

                            if (qtReportDesk.Project == "ANY" && !string.IsNullOrEmpty(qtReportDesk.Query))
                            {
                                list = await _dataService.SearchQtLogs(qtReportDesk.Query);
                            }
                            else
                            {
                                list = projectFilter is null || appFilter is null ?
                                    throw new ArgumentNullException("Project not found.") :
                                    await _dataService.SearchQtLogs(qtReportDesk.Query, projectFilter.projectID, appFilter.appID);

                            }

                            var items = new ObservableCollection<SearchResultViewModel>();

                            if (list == null)
                                return;

                            foreach (var record in list)
                            {
                                items.Add(new qt_SearchResultViewModel(record, qtReportDesk, _navigationService));
                            }

                            qtReportDesk.SearchBarItems = items;

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            break;
                        }
                    case CacheContext.Film:
                        {
                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            break;
                        }
                    default:
                        {
                            List<CodingLOG>? list = new();

                            var codeReportDesk = (CodeReportDeskViewModel)_reportDesk;

                            if(codeReportDesk.Project == "NONE")
                            {
                                codeReportDesk.SearchBarItems.Clear();
                                return;
                            }

                            if (!string.IsNullOrEmpty(codeReportDesk.Query))
                            {
                                if (codeReportDesk.Project == "ANY" && codeReportDesk.Application == "ANY")
                                {
                                    list = await _dataService.SearchCodingLogs(codeReportDesk.Query);
                                }
                                else if (codeReportDesk.Project == "ANY")
                                {
                                    list = await _dataService.SearchCodingLogs(codeReportDesk.Query, appFilter.appID);
                                }
                                else
                                {
                                    list = projectFilter is null || appFilter is null
                                        ? throw new ArgumentNullException("Project not found.")
                                        : await _dataService.SearchCodingLogs(codeReportDesk.Query,
                                            projectFilter.projectID, appFilter.appID);
                                }
                            }


                            var items = new ObservableCollection<SearchResultViewModel>();

                            if (list == null)
                                return;

                            foreach (var record in list)
                            {
                                items.Add(new code_SearchResultViewModel(_navigationService, _dataService, record, codeReportDesk));
                            }

                            codeReportDesk.SearchBarItems = items;

                            break;
                        }

                }

            }
            catch (ArgumentNullException nullex)
            {
                Debug.WriteLine($"ArgumentNullException occurred near SearchCommand.Execute(): {nullex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near SearchCommand.Execute(): {ex.Message}");
            }
        }
    }

}
