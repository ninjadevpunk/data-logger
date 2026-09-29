using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Services;


namespace Data_Logger_1._3.ViewModels.Dashboard
{
    internal class CodingAndroidViewModel : CodingViewModel
    {
        public override LogContext Context => LogContext.AndroidStudio;

        public CodingAndroidViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = [];
        }

        public CodingAndroidViewModel(string logCount, NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {

            LogCount = logCount;

        }


        public async Task AutoStart()
        {
            if (CacheItems is not null)
                await UpdateLogCount();
        }

        public override async Task UpdateLogCount()
        {
            if (CacheItems is not null)
            {
                var count = await _dataService.ASLogCountAsync();
                LogCount = $"{CacheItems.Count} android logs cached | {count} total logs";
            }
        }

    }
}
