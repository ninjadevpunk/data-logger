using Data_Logger_1._3.Commands.ReporterCommands;
using Core.Models;
using Data_Logger_1._3.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using Core.Enums;
using Core.Utilities;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class CodeReportDeskViewModel : ReportDeskViewModel
    {
        public override LogContext Context => LogContext.Coding;
        private bool _isBusy = false;


        public CodeReportDeskViewModel(NavigationService navigationService, IDataService dataService, PDFService pdfService) :
            base(navigationService, dataService, pdfService)
        {

            SearchCommand = new SearchCommand(this, _dataService, _navigationService, Context);
            ExportCommand = new ExportCommand();
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);

        }

        /// <summary>
        /// Populate the application input bar and project input bar. Show logs based on app and project filter on start up of ReportDesk.
        /// </summary>
        public async Task AutoStartAsync()
        {
            // INITIALISE APPS
            await InitialiseAppsAsync();

            // INITIALISE PROJECTS
            await InitialiseProjectsAsync();

            // UPDATE LOGS
            await UpdateLogsListAsync();
        }

        /// <summary>
        /// Initialises the dashboard's application options to filter with.
        /// </summary>
        public override async Task InitialiseAppsAsync()
        {
            if (_isBusy) return;
            _isBusy = true;

            Applications.Clear();
            Applications.Add("ANY");
            string holdApp = "ANY";

            try
            {
                foreach (var app in await _dataService.InitialiseApplicationsLISTAsync(LOG.CATEGORY.CODING))
                {
                    Applications.Add(app);

                    if (string.IsNullOrEmpty(Application) && Applications.Count > 0)
                        holdApp = Applications.ElementAt(1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AutoStartAsync: {ex.Message}");
            }

            Application = holdApp;
            _isBusy = false;
        }

        /// <summary>
        /// Initialises the project options for filtering. Used in conjunction with a set application. Call this method whenever the application filter is changed.
        /// </summary>
        public override async Task InitialiseProjectsAsync()
        {
            if (_isBusy) return;
            _isBusy = true;


            Projects.Clear();
            Projects.Add("NONE");
            Projects.Add("ANY");
            string holdProjectName = "NONE";

            try
            {
                foreach (var project in await _dataService.InitialiseProjectsLISTAsync(LOG.CATEGORY.CODING))
                {
                    Projects.Add(project);

                    if (string.IsNullOrEmpty(Project) && Projects.Count > 2)
                        holdProjectName = Projects.ElementAt(2);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} InitialiseProjectsAsync(): {ex.Message}");
            }

            Project = holdProjectName;
            _isBusy = false;
        }

        /// <summary>
        /// Updates logs in the Report Desk.
        /// </summary>
        public override async Task UpdateLogsListAsync()
        {

            try
            {
                Logs.Clear();

                ObservableCollection<REPORTViewModel> list = new ObservableCollection<REPORTViewModel>();
                

                foreach (LOG log in await _dataService.RetrieveLogsAsync(LogContext.Coding))
                {
                    
                }

                Logs = list;
                SetNoLogsMessageVisibility();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} UpdateLogsListAsync: {ex.Message}");
            }

            _isBusy = false;
        }

    }
}
