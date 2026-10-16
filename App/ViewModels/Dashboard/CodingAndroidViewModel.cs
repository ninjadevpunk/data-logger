using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;


namespace Data_Logger_1._3.ViewModels.Dashboard
{
    /// <summary>
    /// Represents UI managing Android log cache.
    /// </summary>
    internal class CodingAndroidViewModel : LogCacheViewModel
    {
        public override LogContext Context => LogContext.AndroidStudio;

        public CodingAndroidViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = [];
        }


        internal override async Task UpdateLogCount()
        {
            var count = await _dataService.ASLogCountAsync();
            if (count == -1)
                throw new InvalidOperationException($"Count failed in {LogContext.Coding} UpdateLogCount");
            LogCount = $"{CacheItems.Count} android logs cached | {count} total logs";
        }

        /// <summary>
        /// Retrieves Android log cache as read-only list.
        /// </summary>
        public IReadOnlyList<AndroidLOGViewModel> CodingCacheItems => [.. CacheItems.Cast<AndroidLOGViewModel>()];

    }
}
