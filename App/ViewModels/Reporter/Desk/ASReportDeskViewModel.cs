using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class ASReportDeskViewModel : ReportDeskViewModel
    {
        public override LogContext Context => LogContext.AndroidStudio;


        public ASReportDeskViewModel(NavigationService navigationService, IDataService dataService, PDFService pdfService) 
            : base(navigationService, dataService, pdfService)
        {
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);
        }

        public override async Task UpdateLogsListAsync()
        {
            
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
