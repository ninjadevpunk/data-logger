using Data_Logger_1._3.Commands.ReporterCommands;
using Core.Models;
using Core.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Data.Interfaces;
using Core.Enums;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class QtReportDeskViewModel : ReportDeskViewModel
    {
        public override LogContext Context => LogContext.Qt;

        /// <summary>
        /// For database CRUD operations only.
        /// </summary>
        /// <param name="navigationService">The service for navigating in adn out of the Qt Report desk.</param>
        /// <param name="dataService">The service required to perform database operations.</param>
        public QtReportDeskViewModel(NavigationService navigationService, IDataService dataService, PDFService pdfService) 
            : base(navigationService, dataService, pdfService)
        {

            Application = "Qt Creator";
            ApplicationEnabled = false;
            ApplicationToolTip = "You CANNOT edit this field at this stage for only Qt logs are shown on this page.";
            Applications.Add("Qt Creator");


            SearchCommand = new SearchCommand(this, _dataService, _navigationService, Context);
            ExportCommand = new ExportCommand();
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);


            AwaitCall = false;
        }

        /// <summary>
        /// Updates logs in the Qt Report Desk.
        /// </summary>
        /// <param name="project">The Qt project you want logs from.</param>
        public override async Task UpdateLogsListAsync()
        {
            Logs.Clear();
            ObservableCollection<REPORTViewModel> list = new ObservableCollection<REPORTViewModel>();


            foreach (LOG log in await _dataService.RetrieveLogsAsync(LogContext.Qt))
            {
                
            }

            Logs = list;
        }

        public override async Task InitialiseAppsAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task InitialiseProjectsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
