using Core.DTOs.Data;
using Core.Enums;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class ASReportDeskViewModel : ReportDeskViewModel
    {
        public override LogContext Context => LogContext.AndroidStudio;


        public ASReportDeskViewModel(NavigationService navigationService, IDataService dataService, AScodeUpdateViewModel androidUpdateViewModel, PDFService pdfService) 
            : base(navigationService, dataService, pdfService)
        {
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);
        }

        /// <summary>
        /// Updates logs in the Android Report Desk.
        /// </summary>
        public override async Task UpdateLogsListAsync()
        {
            Logs.Clear();
            ObservableCollection<REPORTViewModel> list = [];

            List<AndroidDataDTO> androidDataDTOs = [.. (await _dataService.RetrieveLogsAsync(LogContext.AndroidStudio)).Cast<AndroidDataDTO>()];

            foreach (var androidDTO in androidDataDTOs)
            {
                list.Add(new androidREPORTViewModel(_navigationService, _dataService, this, androidDTO));
            }

            Logs = list;
        }

        public override async Task InitialiseAppsAsync()
        {
            Application = Constants.Android;
            Applications = [];
        }

        public override async Task InitialiseProjectsAsync()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
