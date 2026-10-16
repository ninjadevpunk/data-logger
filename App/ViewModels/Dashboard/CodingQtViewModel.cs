using Core.Enums;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    /// <summary>
    /// Represents UI managing Qt log cache.
    /// </summary>
    internal class CodingQtViewModel : LogCacheViewModel
    {
        public override LogContext Context => LogContext.Qt;

        public CodingQtViewModel(NavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            CacheItems = [];
        }

        
        internal override async Task UpdateLogCount()
        {
            var count = await _dataService.QtLogCountAsync();

            if (count == -1)
                throw new InvalidOperationException($"Count failed in {LogContext.Coding} UpdateLogCount");

            LogCount = $"{CacheItems.Count} qt logs cached | {count} total logs";
        }

        /// <summary>
        /// Retrieves Qt log cache as read-only list.
        /// </summary>
        public IReadOnlyList<QtLOGViewModel> CodingCacheItems => [.. CacheItems.Cast<QtLOGViewModel>()];
    }
}
