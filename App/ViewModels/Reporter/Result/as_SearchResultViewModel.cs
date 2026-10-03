using Core.Enums;
using Core.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    internal class as_SearchResultViewModel : SearchResultViewModel
    {
        public override LogContext SearchResultContext => LogContext.AndroidStudio;

        public as_SearchResultViewModel(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : 
            base(log, reportDeskViewModel, navigationService)
        {
        }
    }
}
