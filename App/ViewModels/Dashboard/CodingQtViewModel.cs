using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    internal class CodingQtViewModel : CodingViewModel
    {
        public override LogContext Context => LogContext.Qt;

        public CodingQtViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = [];
        }

        
        public override async Task UpdateLogCount()
        {
            var count = await _dataService.QtLogCountAsync();

            // TODO: Handle count failure

            LogCount = $"{CacheItems.Count} qt logs cached | {count} total logs";
        }

    }
}
