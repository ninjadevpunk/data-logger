using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    public class film_SearchResultViewModel : SearchResultViewModel
    {

        public override CacheContext SearchResultContext => CacheContext.Film;

        public film_SearchResultViewModel(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : 
            base(log, reportDeskViewModel, navigationService)
        {
        }



    }
}
