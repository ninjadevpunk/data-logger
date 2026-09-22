using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    public class flexi_SearchResultViewModel : SearchResultViewModel
    {
        public override CacheContext SearchResultContext => CacheContext.Flexi;

        public flexi_SearchResultViewModel(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : 
            base(log, reportDeskViewModel, navigationService)
        {
        }
    }
}
