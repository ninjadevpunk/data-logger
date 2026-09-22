using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class FlexiReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Flexi;

        public FlexiReportDeskViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
        }

        public FlexiReportDeskViewModel(NavigationService navigationService, IDataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
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
