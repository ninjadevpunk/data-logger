using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using System.Collections.ObjectModel;
using Data.Interfaces;
using Core.Enums;
using Core.DTOs.Data;
using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using Core.Utilities;

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
        public QtReportDeskViewModel(NavigationService navigationService, IDataService dataService, qtUpdateViewModel qtUpdateViewModel, PDFService pdfService) 
            : base(navigationService, dataService, pdfService)
        {

            Application = "Qt Creator";
            ApplicationEnabled = false;
            ApplicationToolTip = "You CANNOT edit this field at this stage for only Qt logs are shown on this page.";
            Applications.Add("Qt Creator");


            SearchCommand = new SearchCommand(this, _dataService, _navigationService, Context);
            ExportCommand = new ExportCommand();
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);
        }

        /// <summary>
        /// Updates logs in the Qt Report Desk.
        /// </summary>
        public override async Task UpdateLogsListAsync()
        {
            Logs.Clear();
            ObservableCollection<REPORTViewModel> list = [];

            List<QtDataDTO> qtDataDTOs = [.. (await _dataService.RetrieveLogsAsync(LogContext.Qt)).Cast<QtDataDTO>()];

            foreach (var qtDTO in qtDataDTOs)
            {
                list.Add(new qtREPORTViewModel(_navigationService, _dataService, this, qtDTO));
            }

            Logs = list;
        }

        public override async Task InitialiseAppsAsync()
        {
            Application = Constants.Qt;
            Applications = [];
        }

        public override async Task InitialiseProjectsAsync()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
