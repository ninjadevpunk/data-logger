using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class ASReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.AndroidStudio;

        public ASReportDeskViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);
        }

        public ASReportDeskViewModel(NavigationService navigationService, IDataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
            ReturnToDashboard = new ReporterReturnCommand(_navigationService, Context);
        }

        public override async Task UpdateLogsListAsync()
        {
            //
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
